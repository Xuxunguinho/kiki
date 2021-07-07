using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Lex;

namespace Avax.Core
{
    public class Avaliator<T>
    {
        private readonly Dictionary<string, object> _collectionClass;
        private readonly string _script;

        public Avaliator(string script, Dictionary<string, object> collectionClass)
        {
            _collectionClass = collectionClass;
            _script = script;
        }
       

        public bool Evaluate(IEnumerable<T> source, Expression<Func<T, object>> pKeyDistinct, Func<T, T,
                bool> pKeyAll, Expression<Func<T, object>> notaKey,
            Expression<Func<T, object>> nomeKey, Expression<Func<T, object>> resultKey,
            Expression<Func<T, object>> obsKey, Expression<Func<T, object>> discPkKey,
            Expression<Func<T, object>> discNameKey, out string message)
        {
            try
            {
#if DEBUG

                var stow = new Stopwatch();
                stow.Start();
#endif
                // expressions

                var exprNota = notaKey.DeserializeExpression();
                var exprNome = nomeKey.DeserializeExpression();
                var exprResult = resultKey.DeserializeExpression();
                var exprObs = obsKey.DeserializeExpression();
                var exprDiscName = discNameKey.DeserializeExpression();
                var exprDiscPk = discPkKey.DeserializeExpression();
                
                // binding
                var helper = new ScriptCore<T>();
                helper.AddBind("idDisciplina", exprDiscPk);
                helper.AddBind("discId", exprDiscPk);
                helper.AddBind("nomeDisciplina", exprDiscName);
                helper.AddBind("discName", exprDiscName);

                helper.SetValueForBind("$nota", exprNota);
                helper.SetValueForBind("$result", exprResult);
                helper.SetValueForBind("$obs", exprObs);
                helper.SetValueForBind("$pkAll", pKeyAll);

                var enumerable = source as T[] ?? source.ToArray();

                foreach (var s in enumerable.Distinct(pKeyDistinct))
                {
                    var av = enumerable.Where(x => pKeyAll(x, s))?.ToArray();

                    helper.SetValueForBind("$ctxI", s);
                    helper.SetValueForBind("$ctxC", av);

                    helper.ClasseNotasBinder(_collectionClass, av, exprNota);

                    helper.Run(_script);
                }
#if DEBUG


                stow.Stop();
                message =
                    $"The results were presented in -> {new TimeSpan(stow.ElapsedMilliseconds).Milliseconds}";
#endif
                return true;
            }
            catch (Exception e)
            {
                message = (e.ToString());
                return false;
            }
        }
    }
}