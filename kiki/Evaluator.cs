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

namespace kiki
{
    public class Evaluator<T>
    {
        private Dictionary<string, string> _collectionClass;
        private string _script;
        public readonly Dictionary<string, List<T>> ColletionsByClassifications = new Dictionary<string, List<T>>();
        private EvaluatorScriptCore<T> _evaluatorScriptCore;
        private readonly EventHandler<EvaluatorXTrigger<T>> _afterAvail;
        private readonly EventHandler<EvaluatorXTrigger<T>> _beforeAvail;
        public List<string> Fields;
        public string ResultDescription { get; set; }

        public virtual void OnAfterAvail(object sender, EvaluatorXTrigger<T> e)
        {
            // execute aqui o codigo para antes de Inserir o Item [e.Inserted] 
        }

        public virtual void OnBeforeAvail(object sender, EvaluatorXTrigger<T> e)
        {
            // execute aqui o codigo para antes de Inserir o Item [e.Inserted] 
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
                    // list.Add(x.Name);
                    // list.AddRange(from PropertyDescriptor z in x.GetChildProperties() select z.Name);
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

        public EvaluatorResultMessage Run(IEnumerable<T> source,
            Expression<Func<T, object>> itemDisplayValue, Expression<Func<T, object>> itemKey, Func<T, T,
                bool> itemKeyDistinct, Expression<Func<T, object>> evalKey, Expression<Func<T, object>> evalBasedKey,
            Expression<Func<T, object>> evalBasedKeyDisplayValue, Expression<Func<T, object>> resultKey,
            Expression<Func<T, object>> obsKey, string script, Dictionary<string, string> collectionClass,
            Dictionary<string, string> collectionSubclasses = null)
        {
            try
            {
                
                
                _collectionClass = collectionClass;
                _script = script;
     
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

                    _evaluatorScriptCore.Execute(_collectionClass, collectionSubclasses, _script);


                    // Make collections based on results
                    if (ColletionsByClassifications.ContainsKey(ctxItem.GetDynValue(exprResult).ToString()))
                        ColletionsByClassifications[ctxItem.GetDynValue(exprResult).ToString()].Add(ctxItem);
                    else
                        ColletionsByClassifications.Add(ctxItem.GetDynValue(exprResult).ToString(), new List<T> {ctxItem});

                    OnAfterAvail(this, new EvaluatorXTrigger<T>(ctxItem, ctxCollection));
                    // helper.Run(_script);
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

                return  new EvaluatorResultMessage(message,Enums.EvaluatorMessageType.Success);
            }
            catch (TargetInvocationException e)
            {
                return new EvaluatorResultMessage(e.Message,Enums.EvaluatorMessageType.Error);
            }
            catch (Exception e)
            {
                return new EvaluatorResultMessage(e.Message,Enums.EvaluatorMessageType.Error);
            }
        }

        public string Run(IEnumerable<T> source
            , Expression<Func<T, object>> itemKey, Func<T, T,
                bool> itemKeyDistinct,
            string script, Dictionary<string, string> collectionClass,
            Dictionary<string, string> collectionSubclasses = null)
        {
            try
            {
                _collectionClass = collectionClass;
                _script = script;
#if DEBUG

                var stow = new Stopwatch();
                stow.Start();
#endif
                // expressions
                var enumerable1 = source as T[] ?? source.ToArray();
                var scount = enumerable1?.Distinct(itemKey).Count();

                _evaluatorScriptCore.SetValueForBind("$pkAll", itemKeyDistinct);
                ColletionsByClassifications.Clear();
                var enumerable = source as T[] ?? enumerable1.ToArray();

                foreach (var s in enumerable.Distinct(itemKey))
                {
                    var av = enumerable.Where(x => itemKeyDistinct(x, s))?.ToArray();

                    OnBeforeAvail(this, new EvaluatorXTrigger<T>(s, av));

                    _evaluatorScriptCore.SetValueForBind("$ctxI", s);
                    _evaluatorScriptCore.SetValueForBind("$ctxC", av);

                    // _evaluatorScriptCore.Execute(_collectionClass, collectionClass,  expreFieldForEvalKey, _script);

                    var exprResult = _evaluatorScriptCore.GetBindValue("resultKey") as string[];

                    // Make collections based on results
                    if (ColletionsByClassifications.ContainsKey(s.GetDynValue(exprResult).ToString()))
                        ColletionsByClassifications[s.GetDynValue(exprResult).ToString()].Add(s);
                    else
                        ColletionsByClassifications.Add(s.GetDynValue(exprResult).ToString(), new List<T> {s});

                    OnAfterAvail(this, new EvaluatorXTrigger<T>(s, av));
                    // helper.Run(_script);
                }


                var str = new StringBuilder();
                str.AppendLine();
                str.AppendLine("  Total");
             
                foreach (var x in ColletionsByClassifications.Keys)
                {
                    var count = ColletionsByClassifications[x].Count;
                    var percent = (count * 100) / (double)scount;

                    str.AppendLine();
                    str.AppendLine($"  {x}:{count} -> {percent:###.0} %");
                }

                ResultDescription = str.ToString();

#if DEBUG

                stow.Stop();
                var message = $"The results were presented in -> {new TimeSpan(stow.ElapsedMilliseconds).Milliseconds}";
#endif
                message = "Success";

                return message;
            }
            catch (TargetInvocationException e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}