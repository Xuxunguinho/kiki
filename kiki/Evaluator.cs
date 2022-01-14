/*
 * Copyright (c) 2020 Xuxunguinho - https://github.com/Xuxunguinho
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using kiki.lizzie.exceptions;

namespace kiki
{
    public class Evaluator<T>
    {
        private Dictionary<string, string> _collectionClass;

        public readonly Dictionary<string, List<T>> ColletionsByClassifications = new Dictionary<string, List<T>>();
        private EvaluatorScriptCore<T> _evaluatorScriptCore;
        private readonly EventHandler<EvaluatorXTrigger<T>> _afterAvail;
        private readonly EventHandler<EvaluatorXTrigger<T>> _beforeAvail;
        public readonly List<string> Fields;
        public string ResultDescription { get; set; }

        protected virtual void OnAfterAvail(object sender, EvaluatorXTrigger<T> e)
        {
            // execute aqui o codigo depois  de avaliar 
        }

        protected virtual void OnBeforeAvail(object sender, EvaluatorXTrigger<T> e)
        {
            // execute aqui o codigo para antes de avaliar
        }


        public Evaluator()
        {
            _afterAvail += OnAfterAvail;
            _beforeAvail += OnBeforeAvail;
            Fields = new List<string>();
            var fields = typeof(T).DeserializeProperties();
            _evaluatorScriptCore = new EvaluatorScriptCore<T>();
            foreach (PropertyDescriptor x in fields)
            {
                if (x.GetChildProperties()?.Count > 1)
                {
                    foreach (PropertyDescriptor z in x.GetChildProperties())
                    {
                        _evaluatorScriptCore.AddBind($"{x.Name}_{z.Name}", new[] {x.Name, z.Name});
                        Fields.Add($"{x.Name}_{z.Name}");
                    }
                }
                else
                {
                    _evaluatorScriptCore.AddBind(x.Name, new[] {x.Name});
                    Fields.Add(x.Name);
                }
            }
        }
        /// <summary>
        /// add a variable and its value
        /// </summary>
        /// <param name="keyName">var Name</param>
        /// <param name="value"> var value</param>
        public void AddVar(string keyName, dynamic value)
        {
            if (Fields.Contains(keyName) || _evaluatorScriptCore.ExistsVar(keyName))
                throw new KikiException("esta variável ja encontra declarada");
            _evaluatorScriptCore.AddBind(keyName, value);
            Fields.Add(keyName);
        }

        /// <summary>
        /// Run assessment based on subsets extracted from the same dataset 'source'
        /// </summary>
        /// <param name="source">dataset</param>
        /// <param name="itemDisplayValue">the value to show for each item evaluated, for example, the Name of a student when evaluating it</param>
        /// <param name="itemKey">key to identify each entity in the dataset(source) -> (context Item)</param>
        /// <param name="itemKeyDistinct">for the where condition, to create the entity's data subset -> (Context Collection)</param>
        /// <param name="evalKey">the field to be evaluated</param>
        /// <param name="evalBasedKey">the field on which the rating is based</param>
        /// <param name="evalBasedKeyDisplayValue">value to show in results or statistics for 'evalBasedKey' field</param>
        /// <param name="resultKey">the collection field where the result will be assigned</param>
        /// <param name="obsKey">the collection field where the evaluator will assign notes based on the result</param>
        /// <param name="script">the main evaluation script</param>
        /// <param name="collectionClass">sorted subsets extracted from the context collection (these will appear described in the 'obsKey' observation)</param>
        /// <param name="collectionSubclasses">sorted subsets extracted from the context collection (these will not appear described in the 'obsKey' observation as they are only auxiliaries)</param>
        /// <returns>EvaluatorResultMessage</returns>
        public KikiEvaluatorResultMessage Run(IEnumerable<T> source,
            Expression<Func<T, object>> itemDisplayValue, Expression<Func<T, object>> itemKey, Func<T, T,
                bool> itemKeyDistinct, Expression<Func<T, object>> evalKey, Expression<Func<T, object>> evalBasedKey,
            Expression<Func<T, object>> evalBasedKeyDisplayValue, Expression<Func<T, object>> resultKey,
            Expression<Func<T, object>> obsKey, string script, Dictionary<string, string> collectionClass,
            Dictionary<string, string> collectionSubclasses = null)
        {
            try
            {
                _collectionClass = collectionClass;


                var stow = new Stopwatch();
                stow.Start();

                // expressions
                var enumerable1 = source as T[] ?? source.ToArray();
                var scount = enumerable1?.Distinct(itemKey).Count();

                var expreFieldForEvalKey = evalKey.DeserializeExpression();
                var exprEvalKeyDisplayValue = itemDisplayValue.DeserializeExpression();
                var exprResult = resultKey.DeserializeExpression();
                var exprObs = obsKey.DeserializeExpression();
                var exprDiscName = evalBasedKeyDisplayValue.DeserializeExpression();
                var exprDiscPk = evalBasedKey.DeserializeExpression();

                // binding

                _evaluatorScriptCore.SetValueForBind("evalKey", expreFieldForEvalKey);
                _evaluatorScriptCore.AddBind("evalBasedKey", exprDiscPk);
                _evaluatorScriptCore.AddBind("evalBasedKeyDisplayValue", exprDiscName);
                _evaluatorScriptCore.AddBind("evalKeyDisplayValue", exprEvalKeyDisplayValue);
                _evaluatorScriptCore.SetValueForBind("$result", exprResult);
                _evaluatorScriptCore.SetValueForBind("$obs", exprObs);
                _evaluatorScriptCore.SetValueForBind("$pkAll", itemKeyDistinct);

                var enumerable = source as T[] ?? enumerable1.ToArray();

                foreach (var ctxItem in enumerable.Distinct(itemKey))
                {
                    var ctxCollection = enumerable.Where(x => itemKeyDistinct(x, ctxItem))?.ToList();

                    OnBeforeAvail(this, new EvaluatorXTrigger<T>(ctxItem, ctxCollection));

                    _evaluatorScriptCore.SetValueForBind("$ctxI", ctxItem);
                    _evaluatorScriptCore.SetValueForBind("$ctxC", ctxCollection);

                    _evaluatorScriptCore.Execute(_collectionClass, collectionSubclasses, script);

                    // Make collections based on results
                    if (ColletionsByClassifications.ContainsKey(ctxItem.GetDynValue(exprResult).ToString() ??
                                                                string.Empty))
                        ColletionsByClassifications[ctxItem.GetDynValue(exprResult).ToString() ?? string.Empty]
                            .Add(ctxItem);
                    else
                        ColletionsByClassifications.Add(ctxItem.GetDynValue(exprResult).ToString() ?? string.Empty,
                            new List<T> {ctxItem});

                    OnAfterAvail(this, new EvaluatorXTrigger<T>(ctxItem, ctxCollection));
                }


                var str = new StringBuilder();
                str.AppendLine();
                str.AppendLine($"  Total -> {scount}");
                foreach (var x in ColletionsByClassifications.Keys)
                {
                    var count = ColletionsByClassifications[x].Count;
                    var percent = (count * 100) / (double) scount;

                    str.AppendLine();
                    str.AppendLine($"  {x}:{count} -> {percent:###.000} %");
                }

                ResultDescription = str.ToString();


                stow.Stop();


                var msgBuilder = new StringBuilder();
                msgBuilder.AppendLine($"DataEvaluatorX");
                msgBuilder.AppendLine(
                    $"Executado com sucesso em {TimeSpan.FromMilliseconds(stow.ElapsedMilliseconds).TotalSeconds} segundos");
                var message = msgBuilder.ToString();

                return new KikiEvaluatorResultMessage(message, KikiEvaluatorMessageType.Success);
            }
            catch (TargetInvocationException e)
            {
                return new KikiEvaluatorResultMessage(e.Message, KikiEvaluatorMessageType.Error);
            }
            catch (Exception e)
            {
                return new KikiEvaluatorResultMessage(e.Message, KikiEvaluatorMessageType.Error);
            }
        }

        /// <summary>
        ///  simple script execution
        /// </summary>
        /// <param name="source"></param>
        /// <param name="script"></param>
        /// <param name="collectionClass"></param>
        /// <returns></returns>
        /// <exception cref="LizzieException"></exception>
        public KikiEvaluatorResultMessage Run(IEnumerable<T> source,
            string script, Dictionary<string, string> collectionClass
                = null)
        {
            try
            {
                var ctxCollection = source as T[] ?? source.ToArray();

                if (ctxCollection is null || ctxCollection?.Count() == 0)
                    throw new LizzieException("a colecao nao pode ser nula");

                _evaluatorScriptCore.SetValueForBind("$ctxC", ctxCollection.ToList());

                foreach (var ctxItem in ctxCollection)
                {
                    OnBeforeAvail(this, new EvaluatorXTrigger<T>(ctxItem, ctxCollection));
                    _evaluatorScriptCore.SetValueForBind("$ctxI", ctxItem);
                    _evaluatorScriptCore.Execute(collectionClass, null, script);

                    OnAfterAvail(this, new EvaluatorXTrigger<T>(ctxItem, ctxCollection));
                }
                return new KikiEvaluatorResultMessage("Success", KikiEvaluatorMessageType.Success);
            }
            catch (TargetInvocationException e)
            {
                return new KikiEvaluatorResultMessage(e.Message, KikiEvaluatorMessageType.Error);
            }
            catch (Exception e)
            {
                return new KikiEvaluatorResultMessage(e.Message, KikiEvaluatorMessageType.Error);
            }
        }
    }
}