using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Lex;

namespace Avax
{
    public class Core<T>
    {
        private  Dictionary<string,string> _collectionClass;
        private  string _script;
        public readonly Dictionary<string, List<T>> ColletionsByClassifications = new Dictionary<string, List<T>>();
        private ScriptCore<T> _scriptCore;
        private readonly EventHandler<AvaxTrigger<T>> _afterAvail;
        private readonly EventHandler<AvaxTrigger<T>> _beforeAvail;
        public List<string> Fields;
        public string ResultDescription { get; set; }

        public virtual void OnAfterAvail(object sender, AvaxTrigger<T> e)
        {
            // execute aqui o codigo para antes de Inserir o Item [e.Inserted] 
        }

        public virtual void OnBeforeAvail(object sender, AvaxTrigger<T> e)
        {
            // execute aqui o codigo para antes de Inserir o Item [e.Inserted] 
        }

        public Core()
        {
            _afterAvail += OnAfterAvail;
            _beforeAvail += OnBeforeAvail;
            Fields = new List<string>();
            var fields = typeof(T).DeserializeProperties();
             _scriptCore = new ScriptCore<T>();
            foreach (PropertyDescriptor x in fields)
            {
                if (x.GetChildProperties()?.Count > 1)
                {
                    // list.Add(x.Name);
                    // list.AddRange(from PropertyDescriptor z in x.GetChildProperties() select z.Name);
                    foreach (PropertyDescriptor z in x.GetChildProperties())
                    {
                        _scriptCore.AddBind($"{x.Name}_{z.Name}", new[] {x.Name, z.Name});
                        Fields.Add($"{x.Name}_{z.Name}");
                    }
                }
                else
                {
                    _scriptCore.AddBind(x.Name, new[] {x.Name});
                    Fields.Add(x.Name);
                }
            }
        }

        public string Run(IEnumerable<T> source,
            Expression<Func<T, object>> itemDisplayValue, Expression<Func<T, object>> itemKey, Func<T, T,
                bool> itemKeyDistinct, Expression<Func<T, object>> evalKey, Expression<Func<T, object>> evalBasedKey,
            Expression<Func<T, object>> evalBasedKeyDisplayValue, Expression<Func<T, object>> resultKey,
            Expression<Func<T, object>> obsKey,string script, Dictionary<string, string> collectionClass,Dictionary<string, string> collectionSubclasses = null)
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
                var exprNome = itemDisplayValue.DeserializeExpression();
                var exprResult = resultKey.DeserializeExpression();
                var exprObs = obsKey.DeserializeExpression();
                var exprDiscName = evalBasedKeyDisplayValue.DeserializeExpression();
                var exprDiscPk = evalBasedKey.DeserializeExpression();

                // binding

                _scriptCore.AddBind("idDisciplina", exprDiscPk);
                _scriptCore.AddBind("discId", exprDiscPk);
                _scriptCore.AddBind("nomeDisciplina", exprDiscName);
                _scriptCore.AddBind("discName", exprDiscName);

                _scriptCore.SetValueForBind("$nota", expreFieldForEvalKey);
                _scriptCore.SetValueForBind("$result", exprResult);
                _scriptCore.SetValueForBind("$obs", exprObs);
                _scriptCore.SetValueForBind("$pkAll", itemKeyDistinct);

                var enumerable = source as T[] ?? enumerable1.ToArray();

                foreach (var s in enumerable.Distinct(itemKey))
                {
                    var av = enumerable.Where(x => itemKeyDistinct(x, s))?.ToList();

                    OnBeforeAvail(this, new AvaxTrigger<T>(s, av));

                    _scriptCore.SetValueForBind("$ctxI", s);
                    _scriptCore.SetValueForBind("$ctxC", av);

                    _scriptCore.Execute(_collectionClass, collectionClass,  expreFieldForEvalKey, _script);


                    // Make collections based on results
                    if (ColletionsByClassifications.ContainsKey(s.GetDynValue(exprResult).ToString()))
                        ColletionsByClassifications[s.GetDynValue(exprResult).ToString()].Add(s);
                    else
                        ColletionsByClassifications.Add(s.GetDynValue(exprResult).ToString(), new List<T> {s});

                    OnAfterAvail(this, new AvaxTrigger<T>(s, av));
                    // helper.Run(_script);
                }


                var str = new StringBuilder();
                str.AppendLine();
                str.AppendLine("  Total");
                foreach (var x in ColletionsByClassifications.Keys)
                {
                    var count = ColletionsByClassifications[x].Count;
                    var percent = (count * 100) / (double) scount;

                    str.AppendLine();
                    str.AppendLine($"  {x}:{count} -> {percent} %");
                }

                ResultDescription = str.ToString();



                stow.Stop();
                var message = $"The results were presented in -> {new TimeSpan(stow.ElapsedMilliseconds).Milliseconds}";


                var msgBuilder = new StringBuilder();
                msgBuilder.AppendLine($"Avax.core");
                msgBuilder.AppendLine($"Executado com sucesso em {stow.ElapsedMilliseconds * 60} segundos");
                message = msgBuilder.ToString();

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
        
        public string Run(IEnumerable<T> source
            , Expression<Func<T, object>> itemKey, Func<T, T,
                bool> itemKeyDistinct,
           string script, Dictionary<string, string> collectionClass,Dictionary<string, string> collectionSubclasses = null)
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
                
                _scriptCore.SetValueForBind("$pkAll", itemKeyDistinct);

                var enumerable = source as T[] ?? enumerable1.ToArray();

                foreach (var s in enumerable.Distinct(itemKey))
                {
                    var av = enumerable.Where(x => itemKeyDistinct(x, s))?.ToArray();

                    OnBeforeAvail(this, new AvaxTrigger<T>(s, av));

                    _scriptCore.SetValueForBind("$ctxI", s);
                    _scriptCore.SetValueForBind("$ctxC", av);

                    // _scriptCore.Execute(_collectionClass, collectionClass,  expreFieldForEvalKey, _script);

                    var exprResult = _scriptCore._masterBinder["resultKey"] as string[];

                    // Make collections based on results
                    if (ColletionsByClassifications.ContainsKey(s.GetDynValue(exprResult).ToString()))
                        ColletionsByClassifications[s.GetDynValue(exprResult).ToString()].Add(s);
                    else
                        ColletionsByClassifications.Add(s.GetDynValue(exprResult).ToString(), new List<T> {s});

                    OnAfterAvail(this, new AvaxTrigger<T>(s, av));
                    // helper.Run(_script);
                }


                var str = new StringBuilder();
                str.AppendLine();
                str.AppendLine("  Total");
                foreach (var x in ColletionsByClassifications.Keys)
                {
                    var count = ColletionsByClassifications[x].Count;
                    var percent = (count * 100) / (double) scount;

                    str.AppendLine();
                    str.AppendLine($"  {x}:{count} -> {percent} %");
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