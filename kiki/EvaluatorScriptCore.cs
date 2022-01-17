/*
 * Copyright (c) 2020 Xuxunguinho - https://github.com/Xuxunguinho
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using kiki.lizzie;
using kiki.lizzie.exceptions;
using Microsoft.VisualBasic;
using static kiki.Lex;

namespace kiki
{
    /// <summary>
    /// implementing Lizzie for application
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class EvaluatorScriptCore<T>
    {
        private static Func<IEnumerable<string>, object, object> GetValueFromKey => (key, x) => x.GetDynValue(key);
        private readonly lizzie.Binder<EvaluatorScriptCore<T>> _masterBinder;

        /// <summary>
        ///  class builder
        /// </summary>
        public EvaluatorScriptCore()
        {
            _masterBinder = new lizzie.Binder<EvaluatorScriptCore<T>>();

            //Extending Lizzie with new functions
            _masterBinder["$R->"] = ResultSetter;
            _masterBinder["!=>"] = NotContains;
            _masterBinder["=>"] = Contains;
            // portuguese
            _masterBinder["somaT"] = Summation;
            _masterBinder["entre"] = Between;
            _masterBinder["mesTerminou"] = DateIsMonthOver;
            // english
            _masterBinder["sumT"] = Summation;
            _masterBinder["or"] = Or;
            _masterBinder["&"] = And;
            _masterBinder["percent"] = Percent;
            _masterBinder["between"] = Between;
            _masterBinder["isMonthOver"] = DateIsMonthOver;
            // bind DateTime Functions in English
            _masterBinder["$daysInMonth"] = GetDateTimeDaysInMoth;
            _masterBinder["$year"] = GetDateTimeYear;
            _masterBinder["$day"] = GetDateTimeDay;
            _masterBinder["$month"] = GetDateTimeMonth;
            _masterBinder["$hour"] = GetDateTimeHour;
            _masterBinder["$minute"] = GetDateTimeMinute;
            _masterBinder["$second"] = GetDateTimeSecond;
            _masterBinder["$days"] = GetDateTimeCountDaysDiff;
            _masterBinder["$months"] = GetDateTimeCountMonthDiff;
            _masterBinder["$years"] = GetDateTimeCountYearDiff;
            _masterBinder["$minutes"] = GetDateTimeCountMinuteDiff;
            _masterBinder["$seconds"] = GetDateTimeCountSecondsDiff;
            // bind DateTime Functions in Portuguese
            _masterBinder["$diasDoMes"] = GetDateTimeDaysInMoth;
            _masterBinder["$ano"] = GetDateTimeYear;
            _masterBinder["$dia"] = GetDateTimeDay;
            _masterBinder["$mes"] = GetDateTimeMonth;
            _masterBinder["$hora"] = GetDateTimeHour;
            _masterBinder["$minuto"] = GetDateTimeMinute;
            _masterBinder["$segundo"] = GetDateTimeSecond;
            _masterBinder["$dias"] = GetDateTimeCountDaysDiff;
            _masterBinder["$meses"] = GetDateTimeCountMonthDiff;
            _masterBinder["$anos"] = GetDateTimeCountYearDiff;
            _masterBinder["$minutos"] = GetDateTimeCountMinuteDiff;
            _masterBinder["$segundos"] = GetDateTimeCountSecondsDiff;
            // portuguese
            _masterBinder["ou"] = Or;
            //initializing new reserved fields for Lizzie
            AddBind("$result", new string[] { });
            AddBind("$obs", new string[] { });
            AddBind("evalKey", new string[] { });
            AddBind("evalKeyDisplayValue", new string[] { });
            AddBind("evalBasedKey", new string[] { });
            AddBind("evalBasedKeyDisplayValue", new string[] { });
            AddBind("ratingCollection", new Dictionary<string, List<T>> { });
            AddBind("subCollection", new Dictionary<string, List<T>> { });
            AddBind("$ctxI", CreateInstance<T>());
            AddBind("$ctxC", new List<T>());
            AddBind("$pkAll", null);

           lizzie.LambdaCompiler.BindFunctions(_masterBinder);
        }

        /// <summary>
        /// set binders for Lizzie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddBind(string name, object value)
        {
            _masterBinder[name] = value;
        }

        public bool ExistsVar(string name)
        {
            return _masterBinder.StaticItems?.Contains(name) ?? false;
        }

        /// <summary>
        /// checking if the value assigned is a property field of type <T>, if yes,
        /// then get the value of the field, if not, keep the original value assigned
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="binder"></param>
        /// <returns></returns>
        private static object DeserializeValue(object arg1, Binder<EvaluatorScriptCore<T>> binder)
        {
            var contextItem = binder["$ctxI"].Cast<T>();
            var value = arg1 is string[] strings
                ? contextItem.GetDynValue(strings)
                : arg1;
            return value;
        }

        private static dynamic DeserializeValueDyn(object arg1, Binder<EvaluatorScriptCore<T>> binder)
        {
            var value = arg1 is string[] strings
                ? ((object) (binder["$ctxI"] as dynamic)).GetDynRuntimeValue(strings)
                : arg1;
            return value;
        }

        /// <summary>
        /// assigns value to an existing Bind
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetValueForBind(string name, object value)
        {
            _masterBinder[name] = value;
        }

        private object LambdaCompiler(string code)
        {
            var lambda = lizzie.LambdaCompiler.Compile(new EvaluatorScriptCore<T>(), _masterBinder, code);
            return lambda();
        }

        public void Execute(Dictionary<string, string> classes, Dictionary<string, string> subClasses, string script)
        {
            try
            {
                var dictionary = new Dictionary<string, List<T>>();
                // clearing data already binded in lizzie
                SetValueForBind("ratingCollection", dictionary);
                var ctxC = _masterBinder["$ctxC"] as List<T>;
                var evalKey = _masterBinder["evalKey"] as string[];


                if (classes != null)
                {
                    if (classes?.Keys == null)
                        throw new Exception("as classes de classificação não podem ser nulas ou vazias");

                    foreach (var key in classes?.Keys)
                    {
                        var symbolName = key + "s";
                        var expr = classes[key].SupressSpace();
                        var exec = LambdaCompiler(expr) as Function<EvaluatorScriptCore<T>>;


                        var data = new List<T>();
                        if (ctxC != null)
                            foreach (var x in ctxC)
                            {
                                _masterBinder["$ctxI"] = x;
                                if (!(exec?.Invoke(this, _masterBinder,
                                        new Arguments {GetValueFromKey(evalKey, x)}) is bool
                                    condition)) continue;
                                if (condition)
                                    data.Add(x);
                            }

                        _masterBinder[symbolName] = data;
                        dictionary.Add(symbolName, data);
                    }

                    SetValueForBind("ratingCollection", dictionary);
                }

                if (subClasses != null)
                {
                    var dictionary1 = new Dictionary<string, List<T>>();
                    // clearing data already binded in lizzie
                    SetValueForBind("subCollection", dictionary1);

                    if (subClasses?.Keys != null)
                        foreach (var key in subClasses?.Keys)
                        {
                            var symbolName = key + "s";
                            var expr = subClasses[key].SupressSpace();
                            var exec = LambdaCompiler(expr) as Function<EvaluatorScriptCore<T>>;
                            var data = new List<T>();
                            if (ctxC != null)
                                foreach (var x in ctxC)
                                {
                                    _masterBinder["$ctxI"] = x;
                                    if (!(exec?.Invoke(this, _masterBinder,
                                            new Arguments {GetValueFromKey(evalKey, x)}) is bool
                                        condition)) continue;
                                    if (condition)
                                        data.Add(x);
                                }

                            _masterBinder[symbolName] = data;
                            dictionary1.Add(symbolName, data);
                        }

                    SetValueForBind("subCollection", dictionary1);
                }

                LambdaCompiler(script);
            }
            catch (Exception e)
            {
                throw new LizzieException(e.ToString());
            }
        }

        #region extending Lizzie

        private Function<EvaluatorScriptCore<T>> Contains => (ctx, binder, args) =>
        {
            if (args.Count != 3)
                throw new LizzieException("o metodo não pode conter mais  nem menos do que 2 argumento");
            var list = args.Get(0);
            var equate = args.Get(1) as IEnumerable<object>;
            var field = args.Get(2) as string[];

            if (args.Get(1) is IEnumerable<T> compList)
            {
                if (!(list is IEnumerable<T> source)) return false;
                var enumerable = source as T[] ?? source.ToArray();

                return !enumerable.IsNullOrEmpty() &&
                       compList.Any(x => enumerable.Count(z => z.GetDynValue(field).Compare(x.GetDynValue(field))) > 0);
            }
            else
            {
                var objects = equate as object[] ?? (equate ?? new object[] { }).ToArray();
                for (var i = 0; i < objects.Count(); i++)
                    if (i < objects.Count() - 1)
                        if (objects[i].GetType() != objects[i + 1].GetType())
                            throw new LizzieException("os paramentros da lista de compracao devem ser do mesmo tipo");


                var teg = CreateInstance<T>();
                var t1 = objects?.FirstOrDefault()?.GetType();
                var t2 = GetFieldType<T>(field);


                if (t1 != t2)
                    if (!(t1 is null) && !(t1.ToString() == "System.Int64" && t2.ToString() == "System.Int32"))
                        if (!(t1 is null) && !(t1.ToString() == "System.Int64" &&
                                               (t2.ToString() == "System.Double" || t2.ToString() == "System.Float")))
                            throw new Exception($"o campo de referência da " +
                                                "função !=>(conjunto,comparacao,referencia) " +
                                                "deve ser do memo tipo que os itens do conjunto de comparação");


                if (!(list is IEnumerable<T> source)) return false;
                var enumerable = source as T[] ?? source.ToArray();
                if (enumerable.IsNullOrEmpty()) return false;
                return !(equate is null) && equate.Any(x => enumerable.Count(z => z.GetDynValue(field).Compare(x)) > 0);
            }
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="LizzieException"></exception>

        public Function<EvaluatorScriptCore<T>> NotContains => (ctx, binder, args) =>
        {
            if (args.IsNullOrEmpty()) return true;
            if (args.Count != 3)
                throw new LizzieException
                ("O método não pode "
                 + "conter mais nem menos"
                 + " do que 2 argumentos");

            var list = args.Get(0);
            if (((IEnumerable<T>) list).IsNullOrEmpty()) return true;


            if (args.Get(1) is IEnumerable<T> compList)
            {
                if (!(list is IEnumerable<T> source)) return true;
                var field = args.Get(2) as string[];
                if (field.IsNullOrEmpty()) return true;

                var listR = compList?.Select(x =>
                        source?.Count(z => z.GetDynValue(field).Compare(x.GetDynValue(field))) == 0)
                    ?.ToList();

                return (listR.Contains(true) && !listR.Contains(false)) ||
                       (!listR.Contains(true) && listR.Contains(false));
            }
            else
            {
                var equate = args.Get(1) as IEnumerable<object>;
                var objects = equate as object[] ?? (equate ?? new object[] { }).ToArray();
                if (objects.IsNullOrEmpty()) return true;

                var field = args.Get(2) as string[];

                if (field.IsNullOrEmpty()) return true;

                for (var i = 0; i < objects.Count(); i++)
                    if (i < objects.Count() - 1)
                        if (objects[i].GetType() != objects[i + 1].GetType())
                            throw new LizzieException("Os paramentros da lista " +
                                                      "de compração devem ser" +
                                                      " do mesmo tipo");

                if (!(list is IEnumerable<T> source)) return true;
                if (equate is null)
                    throw new LizzieException("A lista de " +
                                              "comparadores não pode ser nula");

                var enumerable = source as T[] ?? source.ToArray();
                if (enumerable.IsNullOrEmpty()) return true;

                var teg = CreateInstance<T>();
                var t1 = objects?.FirstOrDefault()?.GetType();
                var t2 = GetFieldTypeNew<T>(field);


                if (t1 != t2)
                    if (!(t1 is null) && !(t1.ToString() == "System.Int64" && t2.ToString() == "System.Int32"))
                        if (!(t1 is null) && !(t1.ToString() == "System.Int64" &&
                                               (t2.ToString() == "System.Double" || t2.ToString() == "System.Float")))
                            throw new Exception($"o campo de referência da " +
                                                "função !=>(conjunto,comparacao,referencia) " +
                                                "deve ser do memo tipo que os itens do conjunto de comparação");


                var listR = objects?.Select(x => enumerable?.Count(z => z.GetDynValue(field).Compare(x)) == 0)
                    ?.ToList();

                return (listR.Contains(true) && !listR.Contains(false)) ||
                       (!listR.Contains(true) && listR.Contains(false));
            }
        };


        private static Function<EvaluatorScriptCore<T>> ResultSetter => (ctx, binder, args) =>
        {
            try
            {
                // getting binded values
                var contextCollection = binder["$ctxC"] as List<T>;
                var storedData = binder["ratingCollection"] as Dictionary<string, List<T>>;
                var keyValue = binder["evalKey"] as string[];
                var basedKeyDisplayValue = binder["evalBasedKeyDisplayValue"] as string[];
                var exprObs = binder["$obs"] as string[];
                var exprResult = binder["$result"] as string[];
                var pkeyAll = binder["$pkAll"] as Func<T, T, bool>;
                var contextItem = binder["$ctxI"].Cast<T>();
                if (contextItem == null)
                    throw new Exception("o item de contexto nao pode ser nulo");
                //
                var obs = new StringBuilder();
                obs.Clear();
                obs.AppendLine();
                if (storedData != null)
                    foreach (var key in storedData.Keys)
                    {
                        var collection = storedData[key];
                        if (keyValue?.Length > 0 && basedKeyDisplayValue?.Length > 0)
                        {
                            var colet = StrBuilder(collection, keyValue, basedKeyDisplayValue);
                            obs.AppendLine($"    {key} -> {collection.Count}");
                            if (!colet.IsNullOrEmpty())
                                obs.AppendLine($"    {colet}");
                        }

                        obs.AppendLine("");
                    }

                obs.AppendLine();
                // getting arguments
                var arg1 = args[0];

                switch (args.Count)
                {
                    case 1:
                        //checking if the value assigned is a property field of type <T>, if yes,
                        //then get the value of the field, if not, keep the original value assigned
                        var value1 = arg1 is string[] strings
                            ? contextItem.GetDynValue(strings)
                            : arg1;
                        // checking if the value to be assigned is of the same type as the property field to be set
                        if (GetFieldTypeNew<T>(exprResult) != value1.GetType())
                            if (!(GetFieldTypeNew<T>(exprResult).IsNumber() && value1.GetType().IsNumber()))
                                throw new LizzieException(
                                    "O valor a ser atribuido tem de ser do mesmo tipo que o campo de propiedade escolhido como resultado");

                        if (pkeyAll != null)
                        {
                            contextCollection?.Update(p =>
                            {
                                if (exprObs?.Length > 0)
                                    p.SetDynValue(obs.ToString(), exprObs);

                                p.SetDynValue(value1, exprResult);
                            }, n => pkeyAll(n, contextItem));
                        }
                        else
                        {
                            contextItem.SetDynValue(value1, exprResult);
                        }


                        return typeof(void);
                    case 2:
                    {
                        // getting 2th argument 
                        var arg2 = args[1];
                        // getting context item

                        switch (arg1)
                        {
                            case string[] field1 when arg2 is string[] field2:
                            {
                                var fieldValue2 = contextItem.GetDynValue(field2);

                                if (GetFieldTypeNew<T>(field1) != GetFieldTypeNew<T>(field2))
                                    if (!(GetFieldTypeNew<T>(field1).IsNumber() && fieldValue2.GetType().IsNumber()))
                                        throw new LizzieException("O campo 1 tem de ser do mesmo tipo que o campo 2");
                                if (pkeyAll != null)
                                {
                                    contextCollection?.Update(p =>
                                    {
                                        if (exprObs?.Length > 1)
                                            p.SetDynValue(obs.ToString(), exprObs);
                                        p.SetDynValue(Convert.ChangeType(fieldValue2, GetFieldTypeNew<T>(field1)),
                                            field1);
                                    }, n => pkeyAll(n, contextItem));
                                }
                                else
                                {
                                    contextItem.SetDynValue(Convert.ChangeType(fieldValue2, GetFieldTypeNew<T>(field1)),
                                        field1);
                                }

                                break;
                            }
                            case string[] field when arg2.GetType() != typeof(string[]):
                            {
                                if (GetFieldTypeNew<T>(field) != arg2.GetType())
                                    if (!(GetFieldTypeNew<T>(field).IsNumber() && arg2.GetType().IsNumber()))
                                        throw new LizzieException("O campo 1 tem de ser do mesmo tipo que o campo 2");

                                if (pkeyAll != null)
                                {
                                    contextCollection?.Update(p =>
                                    {
                                        if (exprObs?.Length > 1)
                                            p.SetDynValue(obs.ToString(), exprObs);
                                        p.SetDynValue(Convert.ChangeType(arg2, GetFieldTypeNew<T>(field)), field);
                                    }, n => pkeyAll(n, contextItem));
                                }
                                else
                                {
                                    contextItem.SetDynValue(Convert.ChangeType(arg2, GetFieldTypeNew<T>(field)), field);
                                }

                                break;
                            }
                            default:
                                throw new LizzieException(
                                    "O primeiro argumento tem de ser um campo das propriedades do Item");
                        }

                        return typeof(void);
                    }
                    default:
                        throw new Exception("Esta funcao nao pode ter mais do que 2  argumentos");
                }
            }
            catch (Exception e)
            {
                throw new LizzieException(e.ToString());
            }
        };

        private static string StrBuilder(IEnumerable<T> collection, IEnumerable<string> key,
            IEnumerable<string> basedKeyDisplayValue)
        {
            var str =
                collection.Aggregate(string.Empty,
                    (current, x) => (current.IsNullOrEmpty() ? current : current + ", ") +
                                    x.GetDynRuntimeValue(basedKeyDisplayValue) + $"({x.GetDynRuntimeValue(key):#.0})");


            return $"{str} ";
        }


        #region Bolean

        private static Function<EvaluatorScriptCore<T>> Between => (ctx, binder, args) =>
        {
            if (args.Count < 3)
                throw new LizzieException("o metodo não pode conter menos do que 3 argumentos");
            if (args[0] is string[] field)
            {
                var baseValue = DeserializeValueDyn(field, binder);
                var compareValue1 = DeserializeValueDyn(args[1], binder);
                var compareValue2 = DeserializeValueDyn(args[1], binder);

                return baseValue > compareValue1 && baseValue < compareValue2;
            }
            throw new LizzieException(" o primeiro parametro do metodo tem de ser campo de propriedade");
        };


        private static Function<EvaluatorScriptCore<T>> And => (ctx, binder, args) =>
        {
            if (args.Count < 2)
                throw new LizzieException("o metodo não pode conter menos do que 2 argumentos");

            var equate = args as IEnumerable<object>;
            var objects = equate as object[] ?? (equate).ToArray();
            //
            for (var i = 0; i < objects.Length; i++)
                if (i < objects.Length - 1)
                    if (objects[i].GetType() != objects[i + 1].GetType())
                        throw new LizzieException("os paramentros da lista de compracao devem ser do mesmo tipo");

            return (args.Contains(true) && !args.Contains(false));
        };

        private Function<EvaluatorScriptCore<T>> Or => (ctx, binder, args) =>
        {
            if (args.Count < 2)
                throw new LizzieException("o metodo não pode conter menos do que 2 argumentos");
            return args.Aggregate(false, (current, x) => current || (bool) x);
        };
        private static Function<EvaluatorScriptCore<T>> DateIsMonthOver => (ctx, binder, args) =>
        {
            var arg1 = DeserializeValue(args[0], binder);

            switch (args.Count)
            {
                case 1:
                    if (arg1 is DateTime date1)
                    {
                        var day = date1.DaysInMonth();
                        var comp = new DateTime(date1.Year, date1.Month, day);
                        return DateTime.Now.Date > comp.Date;
                    }

                    throw new KikiException("Esta função so aceita paramentros do tipo DateTime");
                case 2:
                    var arg2 = DeserializeValue(args[1], binder);
                    if (arg1 is DateTime date && arg2 is DateTime date2)
                    {
                        var day = date.DaysInMonth();
                        var comp = new DateTime(date.Year, date.Month, day);
                        return date2.Date > comp.Date;
                    }

                    throw new KikiException("Esta função so aceita paramentros do tipo DateTime");
                default:
                    throw new LizzieException("o metodo não pode conter  nenos do que 1 nem mais do que 2  argumentos");
            }
        };
        #endregion

        private Function<EvaluatorScriptCore<T>> Summation => (ctx, binder, args) =>
        {
            var equate = args as IEnumerable<object>;
            var objects = equate as object[] ?? (equate ?? new object[] { }).ToArray();
            if (args.Count == 1)
            {
                var arg1 = objects[0];
                var contextCollection = binder["$ctxC"] as List<T>;
                double soma = 0;
                var contextItem = binder["$ctxI"].Cast<T>();

                if (!(arg1 is string[] field)) return soma;
                var value = GetFieldTypeNew<T>(field);
                if (value.IsNumber())
                    soma = contextCollection?.Sum(x => x.GetDynValue(field).ToString().ToDouble()) ?? 0;
                else
                    throw new LizzieException("Só podem ser somados campos do tipo numérico!");
                return soma;
            }

            if (args.Count != 2)
                throw new LizzieException(
                    "A função somaT so pode ser declarada das seguintes maneiras: [  somaT(list,campo)  ou somaT(campo)]");
            {
                var arg1 = objects[0];
                var arg2 = objects[1];
                double soma = 0;
                if (!(arg1 is List<T> collection))
                    throw new LizzieException(
                        "O primeiro argumento da função[  somaT(list,campo) ] tem de ser um conjunto de elementos!");
                if (!(arg2 is string[] field))
                    throw new LizzieException(
                        "O segundo argumento da função [  somaT(list,campo) ] tem de ser um campo da conjunto!");
                soma = collection?.Sum(x => x.GetDynValue(field).ToString().ToDouble()) ?? 0;
                return soma;
            }
        };


        private static Function<EvaluatorScriptCore<T>> Percent => (ctx, binder, args) =>
        {
            try
            {
                if (args.Count < 2 || args.Count > 2)
                    throw new LizzieException("o metodo não pode conter nem mais ou menos do que 2 argumentos");

                // getting arguments
                var arg1 = args[0];
                var arg2 = args[1];
                // getting context item
                var item = binder["$ctxI"].Cast<T>();

                // %(field,value) -> to get the percentage of certain value (value) base (field)
                if (arg1 is string[] field && arg2.GetType().IsNumber())
                {
                    var fieldValue = item.GetDynRuntimeValue(field);

                    if (!GetFieldTypeNew<T>(field).IsNumber())
                        throw new LizzieException("O valor do campo tem de ser de tipo numerico");

                    return double.Parse(fieldValue.ToString()) * double.Parse(arg2.ToString() ?? string.Empty) / 100;
                }

                // %(value,field) -> to get the percentage value (value) of the base total value (field)
                if (arg2 is string[] field2 && arg1.GetType().IsNumber())
                {
                    var fieldValue = item.GetDynRuntimeValue(field2);

                    if (!GetFieldTypeNew<T>(field2).IsNumber())
                        throw new LizzieException("O valor do campo tem de ser de tipo numerico");

                    if (fieldValue == 0)
                        return 0;

                    return 100 * double.Parse(arg1.ToString() ?? string.Empty) / fieldValue;
                }

                var msg = new StringBuilder();
                msg.AppendLine("A função em questao (percent) so aceita os segintes formatos de declaração: ");
                msg.AppendLine(" percent(field,value) para obter a percentagem de determinado valor (value) base (field)");
                msg.AppendLine(" percent(value,field) para obter  o valor (value)  percentual  do valor total base (field)");
                msg.AppendLine($"{DeserializeValue(args[1], binder)}");
                msg.AppendLine($"{args[1].GetType()}");
                throw new LizzieException(msg.ToString());
            }
            catch (Exception e)
            {
                throw new KikiException(e.ToString());
            }
        };

        #region DateAndTime

        private static Function<EvaluatorScriptCore<T>> GetDateTimeDaysInMoth => (ctx, binder, args) =>
        {
            if (args.Count > 1 | args.Count < 1)
                throw new LizzieException("o metodo não pode contais  mais nem menos do que 1  argumento");
            // getting arguments

            var value = args[0] is string[] strings
                ? ((object) (binder["$ctxI"] as dynamic)).GetDynRuntimeValue(strings)
                : args[0];
            if (value is DateTime date)
            {
                return date.DaysInMonth();
            }

            throw new KikiException("Esta função so aceita paramentros do tipo DateTime");
        };


        private static Function<EvaluatorScriptCore<T>> GetDateTimeDay => (ctx, binder, args) =>
        {
            if (args.Count > 1 | args.Count < 1)
                throw new LizzieException("o metodo não pode contais  mais nem menos do que 1  argumento");
            // getting arguments
            var value = args[0] is string[] strings
                ? ((object) (binder["$ctxI"] as dynamic)).GetDynRuntimeValue(strings)
                : args[0];
            if (value is DateTime date)
            {
                return date.Day;
            }

            throw new KikiException("Esta função so aceita paramentros do tipo DateTime");
        };

        private static Function<EvaluatorScriptCore<T>> GetDateTimeYear => (ctx, binder, args) =>
        {
            if (args.Count > 1 | args.Count < 1)
                throw new LizzieException("o metodo não pode contais  mais nem menos do que 1  argumento");
            // getting arguments
            var value = args[0] is string[] strings
                ? ((object) (binder["$ctxI"] as dynamic)).GetDynRuntimeValue(strings)
                : args[0];
            if (value is DateTime date)
            {
                return date.Year;
            }

            throw new KikiException("Esta função so aceita paramentros do tipo DateTime");
        };

        private static Function<EvaluatorScriptCore<T>> GetDateTimeMonth => (ctx, binder, args) =>
        {
            if (args.Count > 1 | args.Count < 1)
                throw new LizzieException("o metodo não pode contais  mais nem menos do que 1  argumento");
            // getting arguments
            var value = args[0] is string[] strings
                ? ((object) (binder["$ctxI"] as dynamic)).GetDynRuntimeValue(strings)
                : args[0];
            if (value is DateTime date)
            {
                return date.Month;
            }

            throw new KikiException("Esta função so aceita paramentros do tipo DateTime");
        };

        private static Function<EvaluatorScriptCore<T>> GetDateTimeHour => (ctx, binder, args) =>
        {
            if (args.Count > 1 | args.Count < 1)
                throw new LizzieException("o metodo não pode contais  mais nem menos do que 1  argumento");
            // getting arguments
            var value = args[0] is string[] strings
                ? ((object) (binder["$ctxI"] as dynamic)).GetDynRuntimeValue(strings)
                : args[0];
            if (value is DateTime date)
            {
                return date.Hour;
            }

            throw new KikiException("Esta função so aceita paramentros do tipo DateTime");
        };

        private static Function<EvaluatorScriptCore<T>> GetDateTimeMinute => (ctx, binder, args) =>
        {
            if (args.Count > 1 | args.Count < 1)
                throw new LizzieException("o metodo não pode contais  mais nem menos do que 1  argumento");
            // getting arguments
            var value = args[0] is string[] strings
                ? ((object) (binder["$ctxI"] as dynamic)).GetDynRuntimeValue(strings)
                : args[0];
            if (value is DateTime date)
            {
                return date.Minute;
            }

            throw new KikiException("Esta função so aceita paramentros do tipo DateTime");
        };

        private static Function<EvaluatorScriptCore<T>> GetDateTimeSecond => (ctx, binder, args) =>
        {
            if (args.Count > 1 | args.Count < 1)
                throw new LizzieException("o metodo não pode contais  mais nem menos do que 1  argumento");
            // getting arguments
            var value = args[0] is string[] strings
                ? ((object) (binder["$ctxI"] as dynamic)).GetDynRuntimeValue(strings)
                : args[0];
            if (value is DateTime date)
            {
                return date.Second;
            }

            throw new KikiException("Esta função so aceita paramentros do tipo DateTime");
        };

        private static Function<EvaluatorScriptCore<T>> GetDateTimeDayOfWeek => (ctx, binder, args) =>
        {
            if (args.Count > 1 | args.Count < 1)
                throw new LizzieException("o metodo não pode contais  mais nem menos do que 1  argumento");
            // getting arguments
            var value = args[0] is string[] strings
                ? ((object) (binder["$ctxI"] as dynamic)).GetDynRuntimeValue(strings)
                : args[0];
            if (value is DateTime date)
            {
                return date.DayOfWeek;
            }

            throw new KikiException("Esta função so aceita paramentros do tipo DateTime");
        };

        private static Function<EvaluatorScriptCore<T>> GetDateTimeCountDaysDiff => (ctx, binder, args) =>
        {
            if (args.Count < 2 | args.Count > 2)
                throw new LizzieException("o metodo não pode contais  mais nem menos do que 2  argumentos");
            // getting arguments
            var arg1 = DeserializeValue(args[0], binder);
            var arg2 = DeserializeValue(args[1], binder);

            if (arg1 is DateTime date1 && arg2 is DateTime date2)
            {
                return DateAndTime.DateDiff(DateInterval.Day, date1, date2);
            }

            throw new KikiException("Esta função so aceita paramentros do tipo DateTime");
        };

        private static Function<EvaluatorScriptCore<T>> GetDateTimeCountMonthDiff => (ctx, binder, args) =>
        {
            if (args.Count < 2 | args.Count > 2)
                throw new LizzieException("o metodo não pode contais  mais nem menos do que 2  argumentos");
            // getting arguments
            var arg1 = DeserializeValue(args[0], binder);
            var arg2 = DeserializeValue(args[1], binder);

            if (arg1 is DateTime date1 && arg2 is DateTime date2)
            {
                return Microsoft.VisualBasic.DateAndTime.DateDiff(DateInterval.Month, date1, date2);
            }

            throw new KikiException("Esta função so aceita paramentros do tipo DateTime");
        };

        private static Function<EvaluatorScriptCore<T>> GetDateTimeCountMinuteDiff => (ctx, binder, args) =>
        {
            if (args.Count < 2 | args.Count > 2)
                throw new LizzieException("o metodo não pode contais  mais nem menos do que 2  argumentos");
            // getting arguments
            var arg1 = DeserializeValue(args[0], binder);
            var arg2 = DeserializeValue(args[1], binder);

            if (arg1 is DateTime date1 && arg2 is DateTime date2)
            {
                return DateAndTime.DateDiff(DateInterval.Minute, date1, date2);
            }

            throw new KikiException("Esta função so aceita paramentros do tipo DateTime");
        };

        private static Function<EvaluatorScriptCore<T>> GetDateTimeCountSecondsDiff => (ctx, binder, args) =>
        {
            if (args.Count < 2 | args.Count > 2)
                throw new LizzieException("o metodo não pode contais  mais nem menos do que 2  argumentos");
            // getting arguments
            var arg1 = DeserializeValue(args[0], binder);
            var arg2 = DeserializeValue(args[1], binder);

            if (arg1 is DateTime date1 && arg2 is DateTime date2)
            {
                return DateAndTime.DateDiff(DateInterval.Second, date1, date2);
            }

            throw new KikiException("Esta função so aceita paramentros do tipo DateTime");
        };

        private static Function<EvaluatorScriptCore<T>> GetDateTimeCountYearDiff => (ctx, binder, args) =>
        {
            if (args.Count < 2 | args.Count > 2)
                throw new LizzieException("o metodo não pode contais  mais nem menos do que 2  argumentos");
            // getting arguments
            var arg1 = DeserializeValue(args[0], binder);
            var arg2 = DeserializeValue(args[1], binder);

            if (arg1 is DateTime date1 && arg2 is DateTime   date2)
            {
                return DateAndTime.DateDiff(DateInterval.Year, date1, date2);
            }

            throw new KikiException("Esta função so aceita paramentros do tipo DateTime");
        };

     

        #endregion

        #endregion
    }
}