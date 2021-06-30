using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using lizzie;
using lizzie.exceptions;
using Lex;
namespace Avax.Core
{
    internal class ScriptHelper<T>
    {
        private static Func<IEnumerable<string>, object, double> Nota => (nota, x) => x.GetDynValue(nota).ToDouble();
        private static readonly Func<object, string> _map = (obj) => $"map({obj})";
        private static readonly Regex MapRegex = new Regex("map\\(('[^>]+',[a-zA-Z0-9]+)\\)");
        private readonly Binder<ScriptHelper<T>> _masterBinder;


        public ScriptHelper()
        {
            _masterBinder = new Binder<ScriptHelper<T>>();

            AddBind("$nota", new string[] { });
            AddBind("$result", new string[] { });
            AddBind("$obs", new string[] { });
            T aValue = default(T);
            AddBind("$aluno", aValue);
            AddBind("notasAluno", new T[] { });
            var pk = default(Func<T, T, bool>);
            AddBind("$pkAll", pk);
        }

        public string MapFromDictionary(Dictionary<string, object> source)
        {
            try
            {
                var str = string.Empty;
                var i = 0;
                if (source is null) return string.Empty;
                if (source.Keys.Count < 1) return string.Empty;
                foreach (var key in source.Keys)
                {
                    str += i == 0 ? source.ValueFromKey(key) : "," + source.ValueFromKey(key);
                    i++;
                }
                return _map(str);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Dictionary<string, object> DictionaryFormMap(string code)
        {
            try
            {
                if (!MapRegex.IsMatch(code))
                {
                    throw new Exception("o codigo naão se encontra no formato corecto");
                }
                var executeMapa = LambdaCompiler.Compile<ScriptHelper<T>>(new ScriptHelper<T>(), _masterBinder, code);
                var result = executeMapa() as Dictionary<string, object>;
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void AddBind(string name, object value)
        {
            _masterBinder[name] = value;
            LambdaCompiler.BindFunctions(_masterBinder);
        }

        public object SetBindedValue(string name, object value) => _masterBinder[name] = value;

        public object Run(string code)
        {
            var lambda = LambdaCompiler.Compile<ScriptHelper<T>>(new ScriptHelper<T>(), _masterBinder, code);
            return lambda();
        }

        public Dictionary<string, List<T>> BindTipoNotas(Dictionary<string, object> mapaTipoNota, T[] av,
            string[] notaField)
        {
            var dictionary = new Dictionary<string, List<T>>();
            if (mapaTipoNota?.Keys == null) return new Dictionary<string, List<T>>();
            foreach (var key in mapaTipoNota?.Keys)
            {
                var symbolName = key + "s";
                var expr = mapaTipoNota[key].ToString().SupressSpace();
                var exec = Run(expr) as Function<ScriptHelper<T>>;
                var data = av.Where(x => (bool) exec(this, _masterBinder, new Arguments {Nota(notaField, x)}))
                    ?.ToList();
                _masterBinder[symbolName] = data;
                dictionary.Add(symbolName, data);
            }

            AddBind("result_map", dictionary);
            return dictionary;
        }

        public string BindResultados(Dictionary<string, object> mapaResultados)
        {
            var code = @"if(@,{$R->(#)},{*})";
            if (mapaResultados?.Keys == null) return string.Empty;
            var i = 0;
            var str = code;
            foreach (var key in mapaResultados?.Keys)
            {
                var symbolName = "is_" + key;
                var exec = Run(mapaResultados[key].ToString().SupressSpace());
                var lambda = (bool) exec;

                _masterBinder[symbolName] = lambda;
                //str = str.Replace("@", symbolName).Replace("#",$"'{key}'").Replace("*", i < mapaResultados.Keys.Count - 1 ? code : "escreva('')");
                i++;
            }
            return str;
        }

        public bool Start()
        {
            return true;
        }

        #region Lizzie Extensions

        [Bind(Name = "=>")]
        private object Contains(Binder<ScriptHelper<T>> binder, Arguments args)
        {
            if (args.Count != 3)
                throw new LizzieException("o metodo não pode conter mais  nem menos do que 2 argumento");
            var list = args.Get(0);
            var equater = args.Get(1) as IEnumerable<object>;
            ;
            var field = args.Get(2) as string[];
            if (!(list is IEnumerable<T> source)) return false;
            var enumerable = source as T[] ?? source.ToArray();
            if (enumerable.IsNullOrEmpty()) return false;
            if (equater is null) return false;
            var count = enumerable.Count(x => equater.Contains(field.GetValue(x)));
            return count > 0;
        }

        [Bind(Name = "$R->")]
        private object Result(Binder<ScriptHelper<T>> binder, Arguments args)
        {
            if (args.Count > 1 || args.Count < 1)
                throw new Exception("Esta funcao nao pode ter mais nem menos do que 1 argumento");
            if (!(args.Get(0) is string resultado))
                throw new Exception("o argumento tem de ser uma string");

            var av = binder["notasAluno"] as T[];
            var storedData = binder["result_map"] as Dictionary<string, List<T>>;
            var aluno = av.FirstOrDefault();

            var nota = binder["$nota"] as string[];
            var discname = binder["discName"] as string[];
            var exprObs = binder["$obs"] as string[];
            var exprResult = binder["$result"] as string[];
            var pkeyAll = binder["$pkAll"] as Func<T, T, bool>;
            var obs = new StringBuilder();
            obs.Clear();
            obs.AppendLine();
            obs.AppendLine($"    Resultado -> {resultado}");
            foreach (var key in storedData.Keys)
            {
                var coletion = storedData[key];
                var colet = DiscShow(coletion, nota, discname);
                obs.AppendLine($"    {key} -> {coletion.Count} {colet}");
            }
            obs.AppendLine();
            av.Update(p =>
            {
                p.SetDynValue(obs.ToString(), exprObs);
                p.SetDynValue(resultado, exprResult);
            }, n => pkeyAll(n, aluno));

            return null;
        }

        private static string DiscShow(IEnumerable<T> pauta, IEnumerable<string> notaKey,
            IEnumerable<string> dicNameKeys)
        {
            var str = pauta.Aggregate(string.Empty,
                (current, x) => (current.IsNullOrEmpty() ? current : current + ",") +
                                x.GetDynValue(dicNameKeys) + $"({Nota(notaKey, x)})");
            return $"[ {str} ]";
        }

        [Bind(Name = "!=>")]
        private object NotContains(Binder<ScriptHelper<T>> binder, Arguments args)
        {
            if (args.Count != 3)
                throw new LizzieException("o metodo não pode conter mais  nem menos do que 2 argumento");

            var list = args.Get(0);
            var equater = args.Get(1) as IEnumerable<object>;
            Type tmp;
            var objects = equater as object[] ?? equater.ToArray();
            for(var i = 0 ;i < objects.Count();i++)
                if(i < objects.Count() -1)
                    if(objects[i].GetType() != objects[i +1].GetType())
                        throw new LizzieException("os paramentros da lista de compracao devem ser do mesmo tipo");

            var field = args.Get(2) as string[];
            if (!(list is IEnumerable<T> source)) return true;
            var enumerable = source as T[] ?? source.ToArray();
            if (enumerable.IsNullOrEmpty()) return true;
            if (equater is null) return true;
            var count = enumerable.Count(x => objects.Contains(field.GetValue(x)));
            return count <= 0;
        }

        [Bind(Name = "&")]
        private object And(Binder<ScriptHelper<T>> binder, Arguments args)
        {
            if (args.Count < 2)
                throw new LizzieException("o metodo não pode conter menos do que 2 argumentos");
            return args.Aggregate(true, (current, x) => current && (bool) x);
        }

        [Bind(Name = "ou")]
        private object Or(Binder<ScriptHelper<T>> binder, Arguments args)
        {
            if (args.Count < 2)
                throw new LizzieException("o metodo não pode conter menos do que 2 argumentos");
            return args.Aggregate(false, (current, x) => current || (bool) x);
        }

        [Bind(Name = "=")]
        private object setValue(Binder<ScriptHelper<T>> binder, Arguments args)
        {
            if (args.Count != 2)
                throw new LizzieException("o metodo não pode conter mais  nem menos do que 2 argumento");

            var v1 = args.Get(0);
            v1 = args.Get(1);

            return v1;
        }

        #endregion
    }

    public static class scriptBuilder
    {
        public static bool Alva<T>(IEnumerable<T> source)
        {
            return true;
        }
    }
}