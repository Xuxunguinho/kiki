/*
 * Copyright (c) 2019 Xuxunguinho - https://github.com/Xuxunguinho/Lex
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace kiki
{
    public static partial class Lex
    {
        // Type is...
        public static bool Is<T>(this object obj, Action<T> action)
        {
            var ret = obj is T;

            if (ret)
            {
                action((T) obj);
            }

            return ret;
        }

        public static int ParseInt(this string obj)
        {
            int.TryParse(obj, out var number);

            return number;
        }

        public static bool CompareAsLogin(this string a, string b)
        {
            return a.Equals(b, StringComparison.CurrentCulture);
        }

        [Obsolete("ToDouble")]
        public static double AsDouble(this object obj)
        {
            double.TryParse(obj?.ToString(), out var number);

            return number;
        }

        public static double ToDouble(this object obj)
        {
            double.TryParse(obj?.ToString(), out var number);
            return number;
        }

        public static decimal ToDecimal(this object obj)
        {
            decimal.TryParse(obj?.ToString(), out var number);

            return number;
        }

        public static bool IsNumber(this Type t)
        {
            return t == typeof(short) || t == typeof(short?) || t == typeof(int?) || t == typeof(int) ||
                   t == typeof(long) || t == typeof(long?) ||
                   t == typeof(double) || t == typeof(double?) || t == typeof(decimal) || t == typeof(decimal?) ||
                   t == typeof(float) || t == typeof(float?);
        }
        public static bool IsBoolean(this Type t)
        {
            return t == typeof(bool) || t == typeof(bool?);

        }
        public static bool IsDateTime(this Type t)
        {
            return t == typeof(DateTime) || t == typeof(DateTime?);

        }
        public static bool IsTimeSpan(this Type t)
        {
            return t == typeof(TimeSpan) || t == typeof(TimeSpan);

        }
        public static float ToFloat(this object obj)
        {
            float.TryParse(obj?.ToString(), out var number);
            return number;
        }

        public static DateTime ToDateTime(this object obj)
        {
            DateTime.TryParse(obj?.ToString(), out var number);
            return number;
        }

        /// <summary>
        /// To Convert  an TimeSpan to DateTime
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime UpToDateTime(this TimeSpan obj)
        {
            var dt = new DateTime(2000, 01, 01, obj.Hours, obj.Minutes, obj.Seconds);
            return dt;
        }

        public static int AsIntType(this string obj)
        {
            int.TryParse(obj, out var number);

            return number;
        }

        public static DateTime ToDateTime(this string obj)
        {
            DateTime.TryParse(obj, out var number);

            return number;
        }

        [Obsolete("use ToInt()")]
        public static int AsIntType(this object obj)
        {
            if (obj == null) return 0;
            int.TryParse(obj?.ToString(), out var number);

            return number;
        }

        public static int ToInt(this object obj)
        {
            if (obj == null) return 0;
            int.TryParse(obj?.ToString(), out var number);

            return number;
        }

        public static long ToLong(this object obj)
        {
            if (obj == null) return 0;
            long.TryParse(obj?.ToString(), out var number);

            return number;
        }

        /// <summary>
        /// returns the number of days from especific month form Context DateTime
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int DaysInMonth(this DateTime date) => DateTime.DaysInMonth(date.Year, date.Month);


        public static int StrLength(this object obj)
        {
            return obj?.ToString()?.Length ?? 0;
        }

        public static string ToShortTimeString(this TimeSpan time)
        {
            return $"{time.Hours}:{time.Minutes}";
        }

        /// <summary>
        /// Compare values of 2 items of the object class.  why not <x1.Equals(x2)> ?
        /// i noticed that c# has a little problem when the case is to make this kind of comparison
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <returns></returns>
        public static bool Compare(this object x1, object x2)
        {
            switch (x1)
            {
                case double _:
                    return x1.ToString() == x2.ToString();
                case int _:
                    return x1.ToString() == x2.ToString();
                case float _:
                    return x1.ToString() == x2.ToString();
                default:
                    return (x1.Equals(x2));
            }
        }

        /// <summary>
        /// Converte o Objecto para o Determinado tipo T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Cast<T>(this object obj)
        {
            try
            {
                if (obj is null) return CreateInstance<T>();
                return ((T) obj);
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e);
                return CreateInstance<T>();
            }
        }

        public static bool CanCast<T>(this object obj)
        {
            try
            {
                if (obj is null) return false;
                var it = CreateInstance<T>();
                return (it != null);
            }
            catch (InvalidCastException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static TResult CastAsDefault<TResult, TToConvert>(this TToConvert obj)
        {
            var props = TypeDescriptor.GetProperties(typeof(TResult));
            var propsConvert = TypeDescriptor.GetProperties(typeof(TToConvert));
            var s = obj;
            //if (props.Count != propsConvert.Count) return default (TResult);

            var result = CreateInstance<TResult>();

            var values = new object[props.Count];


            for (var i = 0; i < values.Length; i++)
            {
                var name = props[i].Name;
                if (props[i].PropertyType != propsConvert[name].PropertyType) continue;

                props[i].SetValue(result, propsConvert[name].GetValue(obj));
            }

            return result;
        }

        [Obsolete("Utilze simplesmente o CastAs")]
        public static TResult CastAsDefault<TResult>(this object obj)
        {
            var props = TypeDescriptor.GetProperties(typeof(TResult));
            var propsConvert = TypeDescriptor.GetProperties(obj);
            var s = obj;
            //if (props.Count != propsConvert.Count) return default (TResult);

            var result = CreateInstance<TResult>();
            var values = new object[props.Count];
            for (var i = 0; i < values.Length; i++)
            {
                try
                {
                    var name = props[i].Name;
                    if (props[i].PropertyType != propsConvert[name].PropertyType) continue;

                    props[i].SetValue(result, propsConvert[name].GetValue(obj));
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return result;
        }

        [Obsolete("Use TO<>()")]
        public static TResult CastAs<TResult>(this object obj)
        {
            var props = TypeDescriptor.GetProperties(typeof(TResult));
            var propsConvert = TypeDescriptor.GetProperties(obj);
            var s = obj;
            //if (props.Count != propsConvert.Count) return default (TResult);

            var result = CreateInstance<TResult>();
            var values = new object[props.Count];
            for (var i = 0; i < values.Length; i++)
            {
                try
                {
                    var name = props[i].Name;
                    if (props[i].PropertyType != propsConvert[name].PropertyType) continue;

                    props[i].SetValue(result, propsConvert[name].GetValue(obj));
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return result;
        }

        public static TResult To<TResult>(this object obj)
        {
            if (obj is null) return default(TResult);
            if (obj.GetType() == typeof(TResult))
            {
                CreateInstance<TResult>();
                return ((TResult) obj);
            }

            var props = TypeDescriptor.GetProperties(typeof(TResult));
            var propsConvert = TypeDescriptor.GetProperties(obj);
            var s = obj;


            var result = CreateInstance<TResult>();
            var values = new object[props.Count];
            for (var i = 0; i < values.Length; i++)
            {
                try
                {
                    var name = props[i].Name;
                    if (props[i].PropertyType != propsConvert[name].PropertyType) continue;

                    props[i].SetValue(result, propsConvert[name].GetValue(obj));
                }
                catch
                {
                    continue;
                }
            }

            return result;
        }

        /// <summary>
        ///  Retorna a a base principal da contagem numerica 
        ///  Ex: 2019 => Retorna 2000
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int GetBase(this int number)
        {
            var n = number.ToString();
            if (n.Length < 4) return number;
            var length = n.Length;
            var idx = 0;
            switch (length)
            {
                case 4:
                    idx = 1;
                    break;
                case 5:
                    idx = 3;
                    break;
                case 6:
                    idx = 3;
                    break;
                default:
                {
                    if (length >= 7)
                    {
                        idx = 6;
                    }

                    break;
                }
            }

            var numreturn = n.Substring(0, Math.Abs(idx));
            for (var i = 1; i < length - idx; i++)
            {
                numreturn += ("" + 0);
            }

            return numreturn.ToInt();
        }

        public static double GetBase(this double number)
        {
            var n = number.ToString(CultureInfo.InvariantCulture);
            if (n.Length < 4) return number;

            var length = n.Length;
            var idx = 0;
            switch (length)
            {
                case 4:
                    idx = 1;
                    break;
                case 5:
                    idx = 2;
                    break;
                case 6:
                    idx = 3;
                    break;
                default:
                {
                    if (length == 7)
                    {
                        idx = 1;
                    }
                    else
                    {
                        idx = (int) Math.Abs(Math.Sqrt(length));
                    }

                    break;
                }
            }

            var numreturn = n.Substring(0, Math.Abs(idx));
            for (var i = 1; i <= length - idx; i++)
            {
                numreturn += ("" + 0);
            }

            return numreturn.ToDouble();
        }

        public static long GetBase(this long number)
        {
            var n = number.ToString();
            if (n.Length < 4) return number;

            var length = n.Length;
            var idx = 0;
            switch (length)
            {
                case 4:
                    idx = 1;
                    break;
                case 5:
                    idx = 2;
                    break;
                case 6:
                    idx = 3;
                    break;
                default:
                {
                    if (length == 7)
                    {
                        idx = 1;
                    }
                    else
                    {
                        idx = (int) Math.Abs(Math.Sqrt(length));
                    }

                    break;
                }
            }

            var numreturn = n.Substring(0, Math.Abs(idx));
            for (var i = 1; i <= length - idx; i++)
            {
                numreturn += ("" + 0);
            }

            return numreturn.ToLong();
        }

        /// <summary>
        ///  Retorna o numero fora da base principal da contagem numerica 
        ///  Ex: 2019 => Retorna 19
        /// </summary>
        /// <param name="number"/>
        public static int GetOutBase(this int number)
        {
            var n = number.ToString();
            var length = n.Length;
            var sc = length - 3;
            if (n.Length < 2) return number;
            var numreturn = n.Substring(0, sc);
            for (var i = 1; i < length; i++)
            {
                numreturn += ("" + 0);
            }

            return number - numreturn.ToInt();
        }

        public static double GetOutBase(this double number)
        {
            var n = number.ToString();
            if (n.Length < 4) return number;

            var length = n.Length;
            var idx = 0;
            switch (length)
            {
                case 4:
                    idx = 1;
                    break;
                case 5:
                    idx = 2;
                    break;
                case 6:
                    idx = 3;
                    break;
                default:
                {
                    if (length == 7)
                    {
                        idx = 1;
                    }
                    else
                    {
                        idx = (int) Math.Abs(Math.Sqrt(length));
                    }

                    break;
                }
            }

            if (n.Length < 2) return number;
            var numreturn = n.Substring(idx, length - idx);
            //for (var i = 1; i <= length-idx; i++)
            //{
            //    numreturn += ("" + 0);
            //}

            return numreturn.ToDouble();
        }

        public static long GetOutBase(this long number)
        {
            var n = number.ToString();
            if (n.Length < 4) return number;

            var length = n.Length;
            var idx = 0;
            switch (length)
            {
                case 4:
                    idx = 1;
                    break;
                case 5:
                    idx = 2;
                    break;
                case 6:
                    idx = 3;
                    break;
                default:
                {
                    if (length == 7)
                    {
                        idx = 1;
                    }
                    else
                    {
                        idx = (int) Math.Abs(Math.Sqrt(length));
                    }

                    break;
                }
            }

            if (n.Length < 2) return number;
            var numreturn = n.Substring(idx, length - idx);
            //for (var i = 1; i <= length-idx; i++)
            //{
            //    numreturn += ("" + 0);
            //}

            return numreturn.ToLong();
        }

        public static bool AsBool(this object obj)
        {
            bool.TryParse(obj?.ToString(), out var number);

            return number;
        }

        public static double ParseDouble<T>(this string obj)
        {
            double.TryParse(obj, out var number);

            return number;
        }

        public static float ParseFloat(this double obj)
        {
            return (float) obj;
        }
        // ---------- if-then-else as lambda expressions --------------

        /// <summary>
        /// Returns true if the object is null.
        /// </summary>
        public static bool IfNull<T>(this T obj)
        {
            return obj == null;
        }

        /// <summary>
        /// If the object is null, performs the action and returns true.
        /// </summary>
        public static bool IfNull<T>(this T obj, Action action)
        {
            var ret = obj == null;

            if (ret)
            {
                action();
            }

            return ret;
        }

        /// <summary>
        /// Returns true if the object is not null.
        /// </summary>
        public static bool IfNotNull<T>(this T obj)
        {
            return obj != null;
        }

        public static void IfNotNull<T>(this T obj, Action act)
        {
            if (obj != null)
            {
                act();
            }
        }

        public static TR IfNotNullReturn<T, TR>(this T obj, Func<T, TR> func)
        {
            if (obj != null)
            {
                return func(obj);
            }
            else
            {
                return default(TR);
            }
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            try
            {
                if (source == null)
                    return true;
                return source?.Count() < 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return true;
            }
        }

        public static TR ElseIfNullReturn<T, TR>(this T obj, Func<TR> func)
        {
            if (obj == null)
            {
                return func();
            }
            else
            {
                return default(TR);
            }
        }

        /// <summary>
        /// If the object is not null, performs the action and returns true.
        /// </summary>
        public static bool IfNotNull<T>(this T obj, Action<T> action)
        {
            var ret = obj != null;

            if (ret)
            {
                action(obj);
            }

            return ret;
        }

        /// <summary>
        /// If the boolean is true, performs the specified action.
        /// </summary>
        public static bool Then(this bool b, Action f)
        {
            if (b)
            {
                f();
            }

            return b;
        }

        /// <summary>
        /// If the boolean is false, performs the specified action and returns the complement of the original state.
        /// </summary>
        public static void Else(this bool b, Action f)
        {
            if (!b)
            {
                f();
            }
        }


        /// <summary>
        /// Return the key for the dictionary value or throws an exception if more than one value matches.
        /// </summary>
        public static TKey KeyFromValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TValue val)
        {
            // from: http://stackoverflow.com/questions/390900/cant-operator-be-applied-to-generic-types-in-c
            // "Instead of calling Equals, it's better to use an IComparer<T> - and if you have no more information, EqualityComparer<T>.Default is a good choice: Aside from anything else, this avoids boxing/casting."
            return dict.Single(t => EqualityComparer<TValue>.Default.Equals(t.Value, val)).Key;
        }

        public static TValue ValueFromKey<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            // from: http://stackoverflow.com/questions/390900/cant-operator-be-applied-to-generic-types-in-c
            // "Instead of calling Equals, it's better to use an IComparer<T> - and if you have no more information, EqualityComparer<T>.Default is a good choice: Aside from anything else, this avoids boxing/casting."
            return dict.Single(t => EqualityComparer<TKey>.Default.Equals(t.Key, key)).Value;
        }
        // ---------- DBNull value --------------

        // Note the "where" constraint, only value types can be used as Nullable<T> types.
        // Otherwise, we get a bizzare error that doesn't really make it clear that T needs to be restricted as a value class.
        public static object AsDbNull<T>(this T? item) where T : struct
        {
            // If the class is null, return DBNull.Value, otherwise return the class.
            return item as object ?? DBNull.Value;
        }

        // ---------- ForEach iterators --------------

        /// <summary>
        /// Implements a ForEach for generic enumerators.
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }

        /// <summary>
        /// Implements ForEach for non-generic enumerators.
        /// </summary>
        // Usage: Controls.ForEach<Control>(t=>t.DoSomething());
        public static void ForEach<T>(this IEnumerable collection, Action<T> action)
        {
            foreach (T item in collection)
            {
                action(item);
            }
        }

        public static void ForEach(this DataView dv, Action<DataRowView> action)
        {
            foreach (DataRowView drv in dv)
            {
                action(drv);
            }
        }

        // ---------- collection management --------------

        // From the comments of the blog entry http://blog.jordanterrell.com/post/LINQ-Distinct()-does-not-work-as-expected.aspx regarding why Distinct doesn't work right.
        public static IEnumerable<T> RemoveDuplicates<T>(this IEnumerable<T> source)
        {
            return RemoveDuplicates(source, (t1, t2) => t1.Equals(t2));
        }

        public static IEnumerable<T> RemoveDuplicates<T>(this IEnumerable<T> source, Func<T, T, bool> equater)
        {
            // copy the source array 
            var result = new List<T>();

            foreach (var item in source)
            {
                if (result.All(t => !equater(item, t)))
                {
                    // Doesn't exist already: Add it 
                    result.Add(item);
                }
            }

            return result;
        }

        public static T IIF<T>(this bool condition, T truereturn, T falsereturn)
        {
            return condition ? truereturn : falsereturn;
        }

        /// <summary/>
        /// remove duplicacao de items na lista atraves de uma especie de chave primaria escolhida 
        // </summary>
        /// <param name="source"></param>
        /// <param name="indexOf"> index da propriedade do Objecto da lista 'List<object>'</param>
        /// <returns></returns>
        public static IEnumerable<T> RemoveEspecificDuplicates<T>(this IEnumerable<T> source, int indexOf = 0)
        {
            return RemoveDuplicates(source, (t1, t2) => t1.Equals(t2), indexOf);
        }

        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Expression<Func<T, object>> field)
        {
            var props = TypeDescriptor.GetProperties(field.Body.GetType());
            var obj = props["Operand"]?.GetValue(field.Body)?.ToString() ?? field.Body.ToString();
            var strfinal = obj?.Split('.').RightItems(0);
            return Distinct(source, (t1, t2) => t1.Equals(t2), strfinal);
        }

        public static string[] DeserializeExpression<T>(this Expression<Func<T, object>> field)
        {
            try
            {
                var props = TypeDescriptor.GetProperties(field.Body.GetType());
                var obj = props["Operand"]?.GetValue(field.Body)?.ToString() ?? field.Body.ToString();
                var str = obj?.Split('.').RightItems(0);
                return str?.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static PropertyDescriptorCollection DeserializeProperties(this Type item)
        {
            try
            {
                var props = TypeDescriptor.GetProperties(item);
                return props;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Obsolete()]
        public static void Iif<T>(this T targetControl, Expression<Func<bool, bool, bool>> actionExpression)
        {
        }

        /// <summary>
        ///  retorna um conjunto de class sem singulares dependendo do criterio do desenvolvedor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="equater"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        private static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Func<object, object, bool> equater,
            IEnumerable<string> op)
        {
            try
            {
                var enumerable = source as T[] ?? source.ToArray();
                var result = new List<T>();
                if (!(enumerable?.Count() > 0)) return result;
                var enumerable1 = op as string[] ?? op.ToArray();

                if (enumerable1.Length <= 1)
                    return Distinct(enumerable, (t1, t2) => t1.Equals(t2), op.FirstOrDefault());
                foreach (var item in enumerable)
                {
                    var v1 = enumerable1.GetValue(item);
                    if (result.All(t => !equater(enumerable1.GetValue(t), v1)))
                    {
                        result.Add(item);
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        ///  Get value Dynamicaly from an object 
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static object GetValue(this IEnumerable<string> fields, object item)
        {
            try
            {
                object finalvalue = null;
                var enumerable = fields as string[] ?? fields.ToArray();
                var props = TypeDescriptor.GetProperties(item);
                for (var i = 0; i <= enumerable.Count() - 1; i++)
                {
                    var value = props[enumerable[i]]?.GetValue(item);

                    if (i >= enumerable.Length - 1)
                    {
                        finalvalue = value;
                        continue;
                    }

                    props = TypeDescriptor.GetProperties(value);
                    var finalProp = enumerable[i + 1];
                    if (value is null) return new object();
                    finalvalue = props[finalProp]?.GetValue(value);
                    break;
                }

                return finalvalue;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        ///  Get value Dynamicaly from an object 
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static object GetDynValue<T>(this T item, IEnumerable<string> fields)
        {
            object finalvalue = null;
            var enumerable = fields as string[] ?? fields.ToArray();
            var props = TypeDescriptor.GetProperties(item);
            for (var i = 0; i <= enumerable.Count() - 1; i++)
            {
                var value = props[enumerable[i]]?.GetValue(item);
                if (i >= enumerable.Length - 1)
                {
                    finalvalue = value;
                    continue;
                }

                props = TypeDescriptor.GetProperties(value);
                var finalProp = enumerable[i + 1];
                if (value is null) return new object();
                finalvalue = props[finalProp]?.GetValue(value);
                break;
            }

            return finalvalue;
        }

        public static dynamic GetDynRuntimeValue<T>(this T item, IEnumerable<string> fields)
        {
            dynamic finalvalue = null;
            var enumerable = fields as string[] ?? fields.ToArray();
            var props = TypeDescriptor.GetProperties(item);
            for (var i = 0; i <= enumerable.Count() - 1; i++)
            {
                var value = props[enumerable[i]]?.GetValue(item);
                if (i >= enumerable.Length - 1)
                {
                    finalvalue = value;
                    continue;
                }

                props = TypeDescriptor.GetProperties(value);
                var finalProp = enumerable[i + 1];
                if (value is null) return new object();
                finalvalue = props[finalProp]?.GetValue(value);
                break;
            }

            return finalvalue;
        }

        /// <summary>
        ///  returns the field type
        /// </summary>
        /// <param name="desserializedExpessionfields"></param>
        /// <param name="class"></param>
        /// <returns></returns>
        public static Type GetFieldType<T>(IEnumerable<string> desserializedExpessionfields)
        {
            var @class = CreateInstance<T>();
            Type fieldType = null;
            var enumerable = desserializedExpessionfields as string[] ?? desserializedExpessionfields.ToArray();
            var props = TypeDescriptor.GetProperties(typeof(T));
            object type;
            for (var i = 0; i < enumerable.Length; i++)
            {
                type = props[enumerable[i]]?.GetValue(@class) ?? props[enumerable[i]]?.PropertyType;
                fieldType = props[enumerable[i]]?.GetValue(@class)?.GetType() ?? props[enumerable[i]]?.PropertyType;
                if (i <= enumerable.Length - 1) continue;
                props = TypeDescriptor.GetProperties(type);
                fieldType = props[enumerable[i]]?.GetValue(@class)?.GetType() ?? props[enumerable[i]]?.PropertyType;
            }

            return fieldType;
        }

        public static Type GetFieldTypeNew<T>(IEnumerable<string> desserializedExpessionfields)
        {
            Type fieldType = null;
            var enumerable = desserializedExpessionfields.ToList();
            var props = TypeDescriptor.GetProperties(typeof(T));
            var temp = new List<string>();
            var count = enumerable.Count;
            while (fieldType is null)
            {
                for (var i = 0; i < count; i++)
                {
                    var x = enumerable[i];

                    var ss = props.Find(x, true);

                    if (ss is null) return null;

                    if (ss.GetChildProperties().Count > 0 && i < enumerable.Count - 1)
                        props = ss.GetChildProperties();
                    else
                    {
                        fieldType = ss.PropertyType;
                    }
                }
            }

            return fieldType;
        }

        public static void SetDynValue<T>(this T item, object value, IEnumerable<string> fields)
        {
            try
            {
                var enumerable = fields as string[] ?? fields.ToArray();
                object temp;
                var props = TypeDescriptor.GetProperties(item);
                for (var i = 0; i <= enumerable.Count() - 1; i++)

                {
                    temp = props[enumerable[i]]?.GetValue(item);

                    string finalProp;
                    if (i >= enumerable.Length - 1)
                    {
                        finalProp = enumerable[i];
                        props[finalProp].SetValue(i == 0 ? item : temp, value);
                        continue;
                    }

                    props = TypeDescriptor.GetProperties(temp);
                    finalProp = enumerable[i + 1];
                    props[finalProp].SetValue(temp, value);
                    break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static bool CompareFromDynFieldsValue<T>(this T item, T item2, IEnumerable<string> fieldsMatch)
        {
            try
            {
                object finalvalue1 = null, finalvalue2 = null;
                var enumerable = fieldsMatch as string[] ?? fieldsMatch.ToArray();
                var props = TypeDescriptor.GetProperties(item);
                for (var i = 0; i <= enumerable.Count() - 1; i++)
                {
                    var value1 = props[enumerable[i]]?.GetValue(item);
                    var value2 = props[enumerable[i]]?.GetValue(item2);

                    if (i >= enumerable.Length - 1)
                    {
                        finalvalue1 = value1;
                        finalvalue2 = value2;
                        continue;
                    }

                    props = TypeDescriptor.GetProperties(value1);
                    var finalProp = enumerable[i + 1];
                    if (value1 is null) return false;
                    finalvalue1 = props[finalProp]?.GetValue(value1);
                    finalvalue2 = props[finalProp]?.GetValue(value2);
                    break;
                }

                return finalvalue1 != null && finalvalue1.Equals(finalvalue2);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Func<object, object, bool> equater,
            string op)
        {
            try
            {
                // copy the source array 
                var result = new List<T>();
                var props = TypeDescriptor.GetProperties(typeof(T));
                var enumerable = source as T[] ?? source.ToArray();
                if (!(enumerable?.Count() > 0)) return result;
                foreach (var item in enumerable)
                {
                    var s = props[op]?.GetValue(item);

                    // ReSharper disable once PossibleNullReferenceException
                    if (result.All(t => !equater(props[op].GetValue(t).ToString(), s?.ToString())))
                    {
                        result.Add(item);
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Remove a duplicacao comparando um class dos elementos de propriedade
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="equater"></param>
        /// <param name="indexOf"></param>
        /// <returns></returns>
        public static IEnumerable<T> RemoveDuplicates<T>(this IEnumerable<T> source, Func<object, object, bool> equater,
            int indexOf)
        {
            // copy the source array 
            var result = new List<T>();
            var props = TypeDescriptor.GetProperties(typeof(T));
            var enumerable = source as T[] ?? source.ToArray();
            if (!(enumerable?.Count() > 0)) return result;
            foreach (var item in enumerable)
            {
                if (result.All(t => !equater(props[indexOf].GetValue(t), props[indexOf].GetValue(item))))
                {
                    // Doesn't exist already: Add it 
                    result.Add(item);
                }
            }

            return result;
        }

        public static bool Equal<T>(this IEnumerable<T> source, IEnumerable<T> equater, string prop)
        {
            // copy the source array 
            if (equater is null | source is null) return false;
            var props = TypeDescriptor.GetProperties(typeof(T));
            var enumerable = source as T[] ?? source.ToArray();
            var enumerable1 = equater as T[] ?? equater.ToArray();
            if (enumerable?.Count() != enumerable1?.Count()) return false;
            var list = new List<bool>();
            for (var i = 0; i <= enumerable?.Count(); i++)
            {
                var item = props[prop].GetValue(enumerable[0]);
                var item2 = props[prop].GetValue(enumerable1[0]);
                var result = item2 != null && (item != null && item.ToString().Equals(item2.ToString()));
                list.Add(result);
            }

            return !list.Contains(false);
        }


        public static ICollection<T> RemoveDuplicates<T>(this ICollection<T> source)
        {
            return source?.Count > 0 ? RemoveDuplicates(source, (t1, t2) => t1.Equals(t2)) : null;
        }

        public static ICollection<T> RemoveDuplicates<T>(this ICollection<T> source, Func<object, object, bool> equater)
        {
            // copy the source array 
            var result = new List<T>();

            if (!(source?.Count() > 0)) return result;
            foreach (var item in source)
            {
                if (result.All(t => !equater(t, item)))
                {
                    // Doesn't exist already: Add it 
                    result.Add(item);
                }
            }

            return result;
        }

        public static IEnumerable<T> Replace<T>(this IEnumerable<T> source, T newItem, Func<T, T, bool> equater)
        {
            var result = source.Where(item => !equater(item, newItem)).ToList();
            result.Add(newItem);

            return result;
        }

        public static void AddIfUnique<T>(this IList<T> list, T item)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }

        /// <summary>
        /// Remove os itens da lista que contem na toremove
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="toremove"></param>
        /// <returns></returns>
        public static IList<T> Remove<T>(this IList<T> source, IList<T> toremove)
        {
            if (source.IsNullOrEmpty() || toremove.IsNullOrEmpty()) return null;
            foreach (var x in toremove)
            {
                if (!source.Contains(x)) continue;
                var idx = source.IndexOf(x);
                source.RemoveAt(idx);
            }

            return source;
        }

        public static IEnumerable<T> Remove<T>(this IEnumerable<T> source, IEnumerable<T> toremove,
            Func<T, T, bool> equater)
        {
            var enumerable = toremove as T[] ?? toremove.ToArray();
            var remove = source.ToList();
            if (remove.IsNullOrEmpty() || enumerable.IsNullOrEmpty()) return remove;
            foreach (var x in enumerable)
            {
                var it = remove.FirstOrDefault(a => equater(x, a));
                if (remove.Contains(it))
                    remove.Remove(it);
            }

            return remove;
        }

        public static int GetKey(this Dictionary<string[], int[]> list, string key)
        {
            var c = 0;
            ForEach(list, n =>
            {
                for (var i = 0; i < n.Key.Length; i++)
                {
                    if (n.Key[i] == key)
                        c = n.Value[i];
                }

                //c = t.Value.();
            });
            return c;
        }


        // ---------- List to DataTable --------------
        // From http://stackoverflow.com/questions/564366/generic-list-to-datatable

        // which also suggests, for better performance, HyperDescriptor: http://www.codeproject.com/Articles/18450/HyperDescriptor-Accelerated-dynamic-property-acces
        [Obsolete("Use ToDataTable")]
        public static DataTable AsDataTable<T>(this IEnumerable<T> data)
        {
            var enumerable = data as T[] ?? data.ToArray();
            if (enumerable.IsNullOrEmpty()) return new DataTable();
            var props = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();

            for (var i = 0; i < props.Count; i++)
            {
                try
                {
                    var prop = props[i];
                    table.Columns.Add(prop.Name, prop?.PropertyType);
                }
                catch (Exception)
                {
                    continue;
                }
            }

            var values = new object[props.Count];

            foreach (var item in enumerable)
            {
                try
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }

                    table.Rows.Add(values);
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return table;
        }

        /// <summary>
        /// Conver An  IEnumerable<T> To DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> data)
        {
            try
            {
                var enumerable = data as T[] ?? data.ToArray();
                if (enumerable.IsNullOrEmpty()) return new DataTable();
                var props = TypeDescriptor.GetProperties(typeof(T));
                var table = new DataTable();

                for (var i = 0; i < props.Count; i++)
                {
                    try
                    {
                        var prop = props[i];

                        table.Columns.Add(prop.Name, prop?.PropertyType);
                    }
                    catch (Exception)
                    {
                        var prop = props[i];
                        table.Columns.Add(prop.Name,
                            prop?.PropertyType?.BaseType ?? throw new InvalidOperationException());
                    }
                }

                var values = new object[props.Count];
                foreach (var item in enumerable.ToList())
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        try
                        {
                            values[i] = props[i].GetValue(item);
                        }
                        catch (Exception)
                        {
                            //values[i] = default(props);
                        }
                    }

                    table.Rows.Add(values);
                }

                return table;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // http://www.codeproject.com/Questions/455350/how-to-convert-numbers-into-texual-format-using-cs
        public static string NumWordsWrapper(this double n)
        {
            var words = "";
            double decPart = 0;

            if (Math.Abs(n) < 1 && Math.Abs(n) > -1)
            {
                return "zero";
            }

            var splitter = n.ToString(CultureInfo.InvariantCulture).Split('.');
            var intPart = double.Parse(splitter[0]);

            if (splitter.Length == 2)
            {
                // We have a fractional component.
                decPart = double.Parse(splitter[1]);
            }

            words = NumWords(intPart);

            if (!(decPart > 0)) return words;
            if (words != "")
                words += " and ";
            var counter = decPart.ToString(CultureInfo.InvariantCulture).Length;
            switch (counter)
            {
                case 1:
                    words += NumWords(decPart) + " decimo";
                    break;
                case 2:
                    words += NumWords(decPart) + " centezimo";
                    break;
                case 3:
                    words += NumWords(decPart) + " milezimo";
                    break;
                case 4:
                    words += NumWords(decPart) + " decimo milezimo";
                    break;
                case 5:
                    words += NumWords(decPart) + " centezimo milezimo";
                    break;
                case 6:
                    words += NumWords(decPart) + " milionezimo";
                    break;
                case 7:
                    words += NumWords(decPart) + " decimo milionezimo";
                    break;
            }

            return words;
        }

        private static string NumWords(double n) //converts double to words
        {
            var numbersArr = new[]
            {
                "um", "dois", "tres", "quatro", "cinco", "seis", "sete", "oito", "nove", "dez", "onze", "doze", "treze",
                "quatorze", "quinze", "dezaseis", "dezasete", "dezoito", "dezanove"
            };
            var tensArr = new[]
                {"vinte", "trinta", "quarenta", "Sinquenta", "Secenta", "Setenta", "Oitenta", "noventa"};
            var suffixesArr = new[]
            {
                "mil", "milhão", "bilhão", "trilhão", "quadrilhão", "quintilhão", "sextilhão", "septilhão", "octilão",
                "nonilhão", "decilhão", "undecillion", "duodecillion", "tredecillion", "Quattuordecillion",
                "Quindecillion", "Sexdecillion", "Septdecillion", "Octodecillion", "Novemdecillion", "Vigintillion"
            };
            var words = "";

            var tens = false;

            if (n < 0)
            {
                words += "negative ";
                n *= -1;
            }

            var power = (suffixesArr.Length + 1) * 3;

            // TODO: Clean this up - it should be resolvable without resorting to a while loop!
            while (power > 3)
            {
                var pow = Math.Pow(10, power);
                if (n > pow)
                {
                    if (n % Math.Pow(10, power) > 0)
                    {
                        words += NumWords(Math.Floor(n / pow)) + " " + suffixesArr[(power / 3) - 1] + ", ";
                    }
                    else if (n % pow > 0)
                    {
                        words += NumWords(Math.Floor(n / pow)) + " " + suffixesArr[(power / 3) - 1];
                    }

                    n %= pow;
                }

                power -= 3;
            }

            if (n >= 1000)
            {
                // TODO: Gross.  this should be cleaner.
                if (n % 1000 > 0) words += NumWords(Math.Floor(n / 1000)) + " mil, ";
                else words += NumWords(Math.Floor(n / 1000)) + " mil";
                n %= 1000;
            }

            if (0 <= n && n <= 999)
            {
                if ((int) n / 100 > 0)
                {
                    words += NumWords(Math.Floor(n / 100)) + " cento";
                    n %= 100;
                }

                // TODO: Why no "and" here if we say "five hundred [and] ..."

                if ((int) n / 10 > 1)
                {
                    if (words != "")
                        words += " ";
                    words += tensArr[(int) n / 10 - 2];
                    tens = true;
                    n %= 10;
                }

                if ((n < 20) && (n > 0)) // 20, 30, 40, 50, etc... do not have further components.
                {
                    if (words != "" && tens == false)
                        words += " ";
                    words += (tens ? "-" + numbersArr[(int) n - 1] : numbersArr[(int) n - 1]);
                    n -= Math.Floor(n);
                }
            }

            return words;
        }


        /// <summary>
        /// Created by julio reis
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateInstance<T>()
        {
            var obj = TypeDescriptor.CreateInstance(null, typeof(T), null, null);
            return (T) obj;
        }

        /// <summary>
        /// Conversao apartir de um tipo derivado 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<TResult> From<T, TResult>(this IEnumerable<T> data) where TResult : T
        {
            var props = TypeDescriptor.GetProperties(typeof(T));
            var list = new List<TResult>();
            var values = new object[props.Count];
            foreach (var item in data)
            {
                var tipo = CreateInstance<TResult>();
                for (var i = 0; i < values.Length; i++)
                {
                    var value = props[i].GetValue(item);
                    props[i].SetValue(tipo, value);
                }

                list.Add(tipo);
            }

            return list;
        }

        public static List<T> AsTypedList<T>(this DataTable data)
        {
            var props = TypeDescriptor.GetProperties(typeof(T));
            var list = new List<T>();
            props.Add(props[0]);


            var values = new object[props.Count];

            foreach (DataRow item in data.Rows)
            {
                var tipo = CreateInstance<T>();
                for (var i = 0; i < values.Length; i++)
                {
                    props[i].SetValue(tipo, item.ItemArray[i]);
                }

                list.Add(tipo);
            }

            return list;
        }
    }

    //Copyright (c) https://github.com/Xuxunguinho All rights reserved.
    public static partial class Lex
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static int RightCount<T>(this IEnumerable<T> source, T item)
        {
            var list = source?.ToList();
            if (list != null && !list.Any()) return 0;

            if (list != null)
            {
                var idx = list.IndexOf(item);
                var i = list.Count - (idx + 1);

                return i;
            }
            else return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static int LeftCount<T>(this IEnumerable<T> source, T item)
        {
            var enumerable = source as IList<T> ?? source.ToList();
            var list = enumerable?.ToList();
            if (list.Any())
            {
                var countr = enumerable.RightCount(item);
                var count = list.Count - countr;

                return count;
            }
            else return 0;
        }

        /// <summary>
        /// Obtem o class a esquerda do class especificado
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static T LeftItem<T>(this IEnumerable<T> source, T item)
        {
            var list = source?.ToList();
            if (list != null)
            {
                var idx = list.IndexOf(item);
                return idx > 1 ? list[idx - 1] : list.FirstOrDefault();
            }
            else
            {
                return default(T);
            }
        }

        public static IEnumerable<T> LeftItems<T>(this IEnumerable<T> source, T item)
        {
            var enumerable = source as T[] ?? source.ToArray();
            if (!enumerable.Contains(item)) return new List<T>();
            var listleft = CreateInstance<List<T>>();
            var idx = enumerable.ToList().IndexOf(item);
            if (idx > 0)
            {
                for (var i = 0; i < idx; i++)
                {
                    listleft.Add(enumerable[i]);
                }

                return listleft;
            }
            else return new List<T>();
        }

        public static IEnumerable<T> LeftItems<T>(this IEnumerable<T> source, int index)
        {
            var enumerable = source as T[] ?? source.ToArray();

            var item = enumerable[index];

            if (!enumerable.Contains(item)) return new List<T>();
            var listleft = CreateInstance<List<T>>();
            var idx = enumerable.ToList().IndexOf(item);
            if (idx > 0)
            {
                for (var i = 0; i < idx; i++)
                {
                    listleft.Add(enumerable[i]);
                }

                return listleft;
            }
            else return new List<T>();
        }

        public static IEnumerable<T> RightItems<T>(this IEnumerable<T> source, T item)
        {
            var enumerable = source as T[] ?? source.ToArray();
            if (!enumerable.Contains(item)) return new List<T>();
            var listleft = CreateInstance<List<T>>();
            var idx = enumerable.ToList().IndexOf(item);
            var list = enumerable.ToList();
            if (list.Count > idx)
            {
                for (var i = idx + 1; i < list.Count; i++)
                {
                    listleft.Add(enumerable[i]);
                }

                return listleft;
            }
            else return new List<T>();
        }

        public static IEnumerable<T> RightItems<T>(this IEnumerable<T> source, int index)
        {
            var enumerable = source as T[] ?? source.ToArray();

            var item = enumerable[index];

            if (!enumerable.Contains(item)) return new List<T>();
            var listleft = CreateInstance<List<T>>();
            var idx = enumerable.ToList().IndexOf(item);
            var list = enumerable.ToList();
            if (list.Count > idx)
            {
                for (var i = idx + 1; i < list.Count; i++)
                {
                    listleft.Add(enumerable[i]);
                }

                return listleft;
            }
            else return new List<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static T RightItem<T>(this IEnumerable<T> source, T item)
        {
            var list = source?.ToList();
            if (list != null)
            {
                var idx = list.IndexOf(item);

                return idx < list.Count - 1 ? list[idx + 1] : default(T);
            }
            else
            {
                return default(T);
            }
        }

        public static T LastOrdefaultItem<T>(this IEnumerable<T> source)
        {
            if (source == null) return default(T);
            var list = source?.ToList();
            if (list.IsNullOrEmpty()) return default(T);
            var idx = list.Count - 1;
            return list[idx];
        }


        // actions


        /// <summary>
        ///  Atualiza um ou mais  items especificos no FileTable
        /// </summary>
        /// <param name="items"></param>
        /// <param name="setter"> metodo para setar os dados pretendidos</param>
        /// <param name="equater">Função condicional => representa a opcao Where</param>
        public static bool Update<T>(this IEnumerable<T> items, Action<T> setter, Func<T, bool> equater)
        {
            try
            {
                var enumerable = items as T[] ?? items.ToArray();
                var data = enumerable?.Where(equater)?.ToList();
                //disparando a Trigger Before Update
                //captura os items que serão atualizados para retornar estes na trigger Before & After Update
                var deleted = data;
                /* quado não encontra registos a ser atualizado então retorna true
                   decidi fazer isso pela experiencia que adiquiri ao longo dos anos enquanto trabalher com o MS SQLServer
                 */
                if (data.IsNullOrEmpty()) return true;
                //Setando os dados nos registos 
                Parallel.ForEach(enumerable.Where(equater), setter);
                //Salvando alterações nos registos

                // disparando a Trigger AfterUpdate

                return !deleted.IsNullOrEmpty();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        ///  Atualiza um ou mais  items especificos no FileTable apartir de uma lista referente
        /// </summary>
        /// <param name="items"></param>
        /// <param name="source"></param>
        /// <param name="setter"> metodo para setar os dados pretendidos</param>
        /// <param name="equater">Função condicional => representa a opcao Where</param>
        public static bool Update<T>(this IEnumerable<T> items, List<T> source, Action<T, T> setter,
            Func<T, T, bool> equater)
        {
            try
            {
                if (source.IsNullOrEmpty()) return false;
                if (items is null) return false;

                var deleteds = new List<T>();
                foreach (var s in source)
                {
                    var enumerable1 = items as T[] ?? items.ToArray();
                    var data = enumerable1?.Where(x => equater(x, s));
                    {
                        var enumerable = data as T[] ?? data.ToArray();
                        if (enumerable.IsNullOrEmpty()) continue;
                        //captura os items que serão atualizados para retornar estes na trigger Before & After Update
                        deleteds.AddRange(enumerable);
                        /* quado não encontra registos a ser atualizado então retorna true
                           decidi fazer isso pela experiencia que adiquiri ao longo dos anos enquanto trabalher com o MS SQLServer
                         */
                        //Setando os dados nos registos 
                        Parallel.ForEach(enumerable1.Where(x => equater(x, s)), x => setter(x, s));
                    }
                }

                return !deleteds.IsNullOrEmpty();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Elimina do FileTable todos items representados pela função condicional
        /// </summary>
        /// <param name="items"></param>
        /// <param name="equater">Função condicional</param>
        public static bool Delete<T>(this IEnumerable<T> items, Func<T, bool> equater)
        {
            try
            {
                //captura os items que serão atualizados para retornar estes na trigger Before & After delete 
                var enumerable = items as T[] ?? items.ToArray();
                var data = enumerable.Where(equater)?.ToList();
                if (data.IsNullOrEmpty()) return true;
                enumerable.Remove(data);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private static bool Compare<T>(this T t1, T t2, string[] keys)
        {
            foreach (var ke in keys)
            {
                if (!t1.CompareFromDynFieldsValue(t2, new[] {ke})) return false;
            }

            return true;
        }

        public static bool ContainsKey<T>(this List<T> source, T compare, List<string> keys)
        {
            try
            {
                if (source.IsNullOrEmpty()) return false;
                var ks = keys.ToArray();
                foreach (var ix in source)
                {
                    if (ix.Compare(compare, ks)) return true;
                }

                return false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }

    /// <summary>
    /// https://github.com/cliftonm 
    /// Helpers for string manipulation.
    /// </summary>
    public static partial class Lex
    {
        /// <summary>
        /// Left of the first occurance of c
        /// </summary>
        /// <param name="src">The source string.</param>
        /// <param name="c">Return everything to the left of this character.</param>
        /// <returns>String to the left of c, or the entire string.</returns>
        public static string LeftOf(this string src, char c)
        {
            var ret = src;

            var idx = src.IndexOf(c);

            if (idx != -1)
            {
                ret = src.Substring(0, idx);
            }

            return ret;
        }

        /// <summary>
        /// Retorna o primeiro e ultimo Nome
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        [Obsolete("Use FirstAndLastWords")]
        public static string FirstAndLastName(this string nome)
        {
            var wds = nome?.Split(' ');
            if (wds != null && wds.Length <= 1) return nome;
            if (wds == null) return null;
            var str = $"{wds[0]} {wds[wds.Length - 1]}";
            return str;
        }

        public static string FirstAndLastWords(this string nome, bool forNames = false)
        {
            if (string.IsNullOrEmpty(nome)) return string.Empty;
            var wds = nome?.Split(' ');
            if (wds.Length <= 1) return nome;
            string str;
            if (forNames)
            {
                if (wds.Length > 2 && wds[wds.Length - 2].Equals("dos", StringComparison.InvariantCultureIgnoreCase))
                    str = $"{wds[0]} {wds[wds.Length - 2]} {wds[wds.Length - 1]}";
                else
                {
                    str = $"{wds[0]} {wds[wds.Length - 1]}";
                }
            }
            else str = $"{wds[0]} {wds[wds.Length - 1]}";


            return str;
        }

        public static string SupressSpace(this string src)
        {
            var source = src.Split(' ');
            return source.Aggregate(string.Empty, (current, t) => current + t);
        }

        /// <summary>
        /// Left of the n'th occurance of c.
        /// </summary>
        /// <param name="src">The source string.</param>
        /// <param name="c">Return everything to the left n'th occurance of this character.</param>
        /// <param name="n">The occurance.</param>
        /// <returns>String to the left of c, or the entire string if not found or n is 0.</returns>
        public static string LeftOf(this string src, char c, int n)
        {
            var ret = src;
            var idx = -1;

            while (n > 0)
            {
                idx = src.IndexOf(c, idx + 1);

                if (idx == -1)
                {
                    break;
                }

                --n;
            }

            if (idx != -1)
            {
                ret = src.Substring(0, idx);
            }

            return ret;
        }

        /// <summary>
        /// Right of the first occurance of c
        /// </summary>
        /// <param name="src">The source string.</param>
        /// <param name="c">The search char.</param>
        /// <returns>Returns everything to the right of c, or an empty string if c is not found.</returns>
        public static string RightOf(this string src, char c)
        {
            var ret = string.Empty;
            var idx = src.IndexOf(c);

            if (idx != -1)
            {
                ret = src.Substring(idx + 1);
            }

            return ret;
        }

        /// <summary>
        /// Returns all the text to the right of the specified string.
        /// Returns an empty string if the substring is not found.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="substr"></param>
        /// <returns></returns>
        public static string RightOf(this string src, string substr)
        {
            var ret = string.Empty;
            var idx = src.IndexOf(substr, StringComparison.Ordinal);

            if (idx != -1)
            {
                ret = src.Substring(idx + substr.Length);
            }

            return ret;
        }

        /// <summary>
        /// Returns the last character in the string.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static char LastChar(this string src)
        {
            return src[src.Length - 1];
        }

        /// <summary>
        /// Returns all but the last character of the source.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string RemoveLastChar(this string src)
        {
            return src.Substring(0, src.Length - 1);
        }

        public static string RemoveFirstChar(this string src)
        {
            return src.Substring(0, 1);
        }

        /// <summary>
        /// Right of the n'th occurance of c
        /// </summary>
        /// <param name="src">The source string.</param>
        /// <param name="c">The search char.</param>
        /// <param name="n">The occurance.</param>
        /// <returns>Returns everything to the right of c, or an empty string if c is not found.</returns>
        public static string RightOf(this string src, char c, int n)
        {
            var ret = string.Empty;
            var idx = -1;

            while (n > 0)
            {
                idx = src.IndexOf(c, idx + 1);

                if (idx == -1)
                {
                    break;
                }

                --n;
            }

            if (idx != -1)
            {
                ret = src.Substring(idx + 1);
            }

            return ret;
        }

        /// <summary>
        /// Returns everything to the left of the righmost char c.
        /// </summary>
        /// <param name="src">The source string.</param>
        /// <param name="c">The search char.</param>
        /// <returns>Everything to the left of the rightmost char c, or the entire string.</returns>
        public static string LeftOfRightmostOf(this string src, char c)
        {
            var ret = src;
            var idx = src.LastIndexOf(c);

            if (idx != -1)
            {
                ret = src.Substring(0, idx);
            }

            return ret;
        }

        /// <summary>
        /// Returns everything to the right of the rightmost char c.
        /// </summary>
        /// <param name="src">The source string.</param>
        /// <param name="c">The seach char.</param>
        /// <returns>Returns everything to the right of the rightmost search char, or an empty string.</returns>
        public static string RightOfRightmostOf(this string src, char c)
        {
            var ret = string.Empty;
            var idx = src.LastIndexOf(c);

            if (idx != -1)
            {
                ret = src.Substring(idx + 1);
            }

            return ret;
        }

        /// <summary>
        /// Returns everything between the start and end chars, exclusive.
        /// </summary>
        /// <param name="src">The source string.</param>
        /// <param name="start">The first char to find.</param>
        /// <param name="end">The end char to find.</param>
        /// <returns>The string between the start and stop chars, or an empty string if not found.</returns>
        public static string Between(this string src, char start, char end)
        {
            var ret = string.Empty;
            var idxStart = src.IndexOf(start);

            if (idxStart == -1) return ret;
            ++idxStart;
            var idxEnd = src.IndexOf(end, idxStart);

            if (idxEnd != -1)
            {
                ret = src.Substring(idxStart, idxEnd - idxStart);
            }

            return ret;
        }

        public static string Between(this string src, string start, string end)
        {
            var count = src.Length - 2;

            var ret = string.Empty;
            var idxStart = src.IndexOf(start, StringComparison.Ordinal);

            if (idxStart == -1) return ret;
            idxStart += start.Length;
            var idxEnd = src.IndexOf(end, idxStart, StringComparison.Ordinal);

            if (idxEnd != -1)
            {
                ret = src.Substring(idxStart, idxEnd - idxStart);
            }

            return ret;
        }

        public static string BetweenEnds(this string src, char start, char end)
        {
            var ret = string.Empty;
            var idxStart = src.IndexOf(start);

            if (idxStart == -1) return ret;
            ++idxStart;
            var idxEnd = src.LastIndexOf(end);

            if (idxEnd != -1)
            {
                ret = src.Substring(idxStart, idxEnd - idxStart);
            }

            return ret;
        }

        public static string BetweenEnds(this string src, string start, string end)
        {
            try
            {
                var ret = string.Empty;
                var idxStart = src.IndexOf(start, StringComparison.Ordinal);

                if (idxStart == -1) return ret;
                idxStart += start.Length;
                var idxEnd = src.LastIndexOf(end, StringComparison.Ordinal);

                if (idxEnd != -1)
                {
                    ret = src.Substring(idxStart, idxEnd - idxStart);
                }

                return ret;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return "0";
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns the number of occurances of "find".
        /// </summary>
        /// <param name="src">The source string.</param>
        /// <param name="find">The search char.</param>
        /// <returns>The # of times the char occurs in the search string.</returns>
        public static int Count(this string src, char find)
        {
            return src.Count(s => s == find);
        }

        /// <summary>
        /// Returns the rightmost char in src.
        /// </summary>
        /// <param name="src">The source string.</param>
        /// <returns>The rightmost char, or '\0' if the source has zero length.</returns>
        public static char Rightmost(this string src)
        {
            var c = '\0';

            if (src.Length > 0)
            {
                c = src[src.Length - 1];
            }

            return c;
        }

        public static bool BeginsWith(this string src, char c)
        {
            var ret = false;

            if (src.Length > 0)
            {
                ret = src[0] == c;
            }

            return ret;
        }

        public static bool EndsWith(this string src, char c)
        {
            var ret = false;

            if (src.Length > 0)
            {
                ret = src[src.Length - 1] == c;
            }

            return ret;
        }

        public static string EmptyStringAsNull(this string src)
        {
            var ret = src;

            if (ret == string.Empty)
            {
                ret = null;
            }

            return ret;
        }
        public static string Normalized(this string src)
        {
            if (src.IsNullOrEmpty()) return string.Empty;
            return string.Concat(src.Normalize(NormalizationForm.FormD)
                .Where(ch=> CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark));
        }
        public static string NullAsEmptyString(this string src)
        {
            var ret = src ?? string.Empty;

            return ret;
        }

        public static string RemoveChar(this string src, char chr)
        {
            var ret = src ?? string.Empty;
            if (string.IsNullOrEmpty(ret)) return string.Empty;
            if (!ret.Contains(chr)) return string.Empty;
            var str = string.Empty;
            return src == null ? str : src.Where(x => x != chr).Aggregate(str, (current, x) => current + x);
        }

        public static string RemoveCharAndRight(this string src, char chr)
        {
            var ret = src ?? string.Empty;
            if (string.IsNullOrEmpty(ret)) return string.Empty;
            if (!ret.Contains(chr)) return ret;

            var idx = ret.IndexOf(chr);
            var countright = ret.RightCount(chr);
            var str = ret.Remove(idx, countright + 1);

            return str;
        }

        public static bool IsNullOrEmpty(this string src)
        {
            return ((src == null) || (src.Trim() == string.Empty));
        }

        // Read about MD5 here: http://en.wikipedia.org/wiki/MD5
        public static string Hash(string src)
        {
            HashAlgorithm hashProvider = new MD5CryptoServiceProvider();
            var bytes = Encoding.UTF8.GetBytes(src);
            var encoded = hashProvider.ComputeHash(bytes);
            return Convert.ToBase64String(encoded);
        }

        /// <summary>
        /// Returns a camelcase string, where the first character is lowercase.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string CamelCase(this string src)
        {
            if (src != null)
            {
                return src[0].ToString().ToLower() + src?.Substring(1).ToLower();
            }

            return string.Empty;
        }

        /// <returns></returns>
        public static string CamelCaseTrimestre(this string src)
        {
            if (src != null)
            {
                var words = src.Split('º');
                return words[0].ToString().ToUpper() + "ºTrimestre";
            }

            return string.Empty;
        }

        /// <summary>
        /// Returns a Pascalcase string, where the first character is uppercase.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string PascalCase(this string src)
        {
            var ret = string.Empty;

            if (!string.IsNullOrEmpty(src))
            {
                ret = src[0].ToString().ToUpper() + src.Substring(1).ToLower();
            }

            return ret;
        }

        /// <summary>
        /// Returns a Pascal-cased string, given a string with words separated by spaces.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string PascalCaseWords(this string src)
        {
            var sb = new StringBuilder();
            if (string.IsNullOrEmpty(src)) return string.Empty;
            ;
            var s = src.Split(' ');
            var more = string.Empty;

            foreach (var s1 in s)
            {
                sb.Append(more);
                sb.Append(PascalCase(s1));
                more = " ";
            }

            return sb.ToString();
        }

        public static string SeparateCamelCase(this string src)
        {
            var sb = new StringBuilder();
            sb.Append(char.ToUpper(src[0]));

            for (var i = 1; i < src.Length; i++)
            {
                var c = src[i];

                if (char.IsUpper(c))
                {
                    sb.Append(' ');
                }

                sb.Append(c);
            }

            return sb.ToString();
        }

        public static string[] Split(this string source, char delimeter, char quoteChar)
        {
            var retArray = new List<string>();
            int start = 0, end = 0;
            var insideField = false;

            for (end = 0; end < source.Length; end++)
            {
                if (source[end] == quoteChar)
                {
                    insideField = !insideField;
                }
                else if (!insideField && source[end] == delimeter)
                {
                    retArray.Add(source.Substring(start, end - start));
                    start = end + 1;
                }
            }

            retArray.Add(source.Substring(start));

            return retArray.ToArray();
        }
    }

    // Code here is from the excellent article: http://www.codeproject.com/Articles/488643/LinQ-Extended-Joins
    /*
    Example of a join using Linq's Join method:  This is an inner join, where only the intersection of A & B is returned.  
    Linq:

    var result = from p in Person.BuiltPersons()
                 join a in Address.BuiltAddresses()
                 on p.IdAddress equals a.IdAddress
                 select new 
           { 
                     Name             = a.MyPerson.Name,
                     Age              = a.MyPerson.Age,
                     PersonIdAddress  = a.MyPerson.IdAddress,
                     AddressIdAddress = a.MyAddress.IdAddress,
                     Street           = a.MyAddress.Street
           };  

    Lambda:

    var resultJoint = Person.BuiltPersons().Join(                      /// Source Collection
                      Address.BuiltAddresses(),                        /// Inner Collection
                      p => p.IdAddress,                                /// PK
                      a => a.IdAddress,                                /// FK
                      (p, a) => new { MyPerson = p, MyAddress = a })   /// Result Collection
                      .Select(a => new
                        {
                            Name             = a.MyPerson.Name,
                            Age              = a.MyPerson.Age,
                            PersonIdAddress  = a.MyPerson.IdAddress,
                            AddressIdAddress = a.MyAddress.IdAddress,
                            Street           = a.MyAddress.Street
                        });  */

    public static partial class Lex
    {
        /*
		Lambda:

		var resultJoint = Person.BuiltPersons().LeftJoin(                    /// Source Collection
							Address.BuiltAddresses(),                        /// Inner Collection
							p => p.IdAddress,                                /// PK
							a => a.IdAddress,                                /// FK
							(p, a) => new { MyPerson = p, MyAddress = a })   /// Result Collection
							.Select(a => new
							{
								Name             = a.MyPerson.Name,
								Age              = a.MyPerson.Age,
								PersonIdAddress  = a.MyPerson.IdAddress,
								AddressIdAddress = (a.MyAddress != null ? a.MyAddress.IdAddress : -1),
					Street           = (a.MyAddress != null ? a.MyAddress.Street    : "Null-Value")
							}); 
		*/
        /// <summary>
        /// Returns all of A with null values for B if B doesn't exist.
        /// </summary>
        public static IEnumerable<TResult> LeftJoin<TSource, TInner, TKey, TResult>(this IEnumerable<TSource> source,
            IEnumerable<TInner> inner,
            Func<TSource, TKey> pk,
            Func<TInner, TKey> fk,
            Func<TSource, TInner, TResult> result)
        {
            var _result = Enumerable.Empty<TResult>();

            _result = from s in source
                join i in inner
                    on pk(s) equals fk(i) into joinData
                from left in joinData.DefaultIfEmpty()
                select result(s, left);

            return _result;
        }

        /*
		Lambda: 

		var resultJoint = Person.BuiltPersons().RightJoin(                   /// Source Collection
							Address.BuiltAddresses(),                        /// Inner Collection
							p => p.IdAddress,                                /// PK
							a => a.IdAddress,                                /// FK
							(p, a) => new { MyPerson = p, MyAddress = a })   /// Result Collection
							.Select(a => new
							{
								Name           = (a.MyPerson != null ? a.MyPerson.Name : "Null-Value"),
								Age              = (a.MyPerson != null ? a.MyPerson.Age : -1),
								PersonIdAddress  = (a.MyPerson != null ? a.MyPerson.IdAddress : -1),
								AddressIdAddress = a.MyAddress.IdAddress,
								Street           = a.MyAddress.Street
							}); 
		 */
        /// <summary>
        /// Returns all of B with nulls for A if A doesn't exist.
        /// </summary>
        public static IEnumerable<TResult> RightJoin<TSource, TInner, TKey, TResult>(this IEnumerable<TSource> source,
            IEnumerable<TInner> inner,
            Func<TSource, TKey> pk,
            Func<TInner, TKey> fk,
            Func<TSource, TInner, TResult> result)
        {
            var _result = Enumerable.Empty<TResult>();

            _result = from i in inner
                join s in source
                    on fk(i) equals pk(s) into joinData
                from right in joinData.DefaultIfEmpty()
                select result(right, i);

            return _result;
        }

        /*
		Lambda: 

		var resultJoint = Person.BuiltPersons().FullOuterJoinJoin(           /// Source Collection
							Address.BuiltAddresses(),                        /// Inner Collection
							p => p.IdAddress,                                /// PK
							a => a.IdAddress,                                /// FK
							(p, a) => new { MyPerson = p, MyAddress = a })   /// Result Collection
							.Select(a => new
							{
								Name             = (a.MyPerson  != null ? a.MyPerson.Name       : "Null-Value"),
								Age              = (a.MyPerson  != null ? a.MyPerson.Age        : -1),
								PersonIdAddress  = (a.MyPerson  != null ? a.MyPerson.IdAddress  : -1),
								AddressIdAddress = (a.MyAddress != null ? a.MyAddress.IdAddress : -1),
								Street           = (a.MyAddress != null ? a.MyAddress.Street    : "Null-Value")
							});  
		*/
        /// <summary>
        /// Returns both A and B, with nulls where A or B doesn't have a correlation to B or A.
        /// </summary>
        public static IEnumerable<TResult> FullOuterJoinJoin<TSource, TInner, TKey, TResult>(
            this IEnumerable<TSource> source,
            IEnumerable<TInner> inner,
            Func<TSource, TKey> pk,
            Func<TInner, TKey> fk,
            Func<TSource, TInner, TResult> result)
        {
            var enumerable = source as TSource[] ?? source.ToArray();
            var inners = inner as TInner[] ?? inner.ToArray();
            var left = enumerable.LeftJoin(inners, pk, fk, result).ToList();
            var right = enumerable.RightJoin(inners, pk, fk, result).ToList();

            return left.Union(right);
        }

        /*
		Lambda:  

		var resultJoint = Person.BuiltPersons().LeftExcludingJoin(           /// Source Collection
							Address.BuiltAddresses(),                        /// Inner Collection
							p => p.IdAddress,                                /// PK
							a => a.IdAddress,                                /// FK
							(p, a) => new { MyPerson = p, MyAddress = a })   /// Result Collection
							.Select(a => new
							{
								Name             = a.MyPerson.Name,
								Age              = a.MyPerson.Age,
								PersonIdAddress  = a.MyPerson.IdAddress,
								AddressIdAddress = (a.MyAddress != null ? a.MyAddress.IdAddress : -1),
								Street           = (a.MyAddress != null ? a.MyAddress.Street    : "Null-Value")
							}); 
		*/

        /// <summary>
        /// Returns only A where B does NOT exists for A.
        /// </summary>
        public static IEnumerable<TResult> LeftExcludingJoin<TSource, TInner, TKey, TResult>(
            this IEnumerable<TSource> source,
            IEnumerable<TInner> inner,
            Func<TSource, TKey> pk,
            Func<TInner, TKey> fk,
            Func<TSource, TInner, TResult> result)
        {
            var _result = Enumerable.Empty<TResult>();

            _result = from s in source
                join i in inner
                    on pk(s) equals fk(i) into joinData
                from left in joinData.DefaultIfEmpty()
                where left == null
                select result(s, left);

            return _result;
        }


        /*
		 Lambda:   

		var resultJoint = Person.BuiltPersons().RightExcludingJoin(          /// Source Collection
							Address.BuiltAddresses(),                        /// Inner Collection
							p => p.IdAddress,                                /// PK
							a => a.IdAddress,                                /// FK
							(p, a) => new { MyPerson = p, MyAddress = a })   /// Result Collection
							.Select(a => new
							{
								Name             = (a.MyPerson != null ? a.MyPerson.Name      : "Null-Value"),
								Age              = (a.MyPerson != null ? a.MyPerson.Age       : -1),
								PersonIdAddress  = (a.MyPerson != null ? a.MyPerson.IdAddress : -1),
								AddressIdAddress = a.MyAddress.IdAddress,
								Street           = a.MyAddress.Street
							}); 
		*/
        /// <summary>
        /// Returns only B where A does not exist for B.
        /// </summary>
        public static IEnumerable<TResult> RightExcludingJoin<TSource, TInner, TKey, TResult>(
            this IEnumerable<TSource> source,
            IEnumerable<TInner> inner,
            Func<TSource, TKey> pk,
            Func<TInner, TKey> fk,
            Func<TSource, TInner, TResult> result)
        {
            var _result = Enumerable.Empty<TResult>();

            _result = from i in inner
                join s in source
                    on fk(i) equals pk(s) into joinData
                from right in joinData.DefaultIfEmpty()
                where right == null
                select result(right, i);

            return _result;
        }

        /*
		Lambda Expression:   
		Collapse | Copy Code

		var resultJoint = Person.BuiltPersons().FullExcludingJoin(          /// Source Collection
							Address.BuiltAddresses(),                        /// Inner Collection
							p => p.IdAddress,                                /// PK
							a => a.IdAddress,                                /// FK
							(p, a) => new { MyPerson = p, MyAddress = a })   /// Result Collection
							.Select(a => new
							{
								Name             = (a.MyPerson  != null ? a.MyPerson.Name       : "Null-Value"),
								Age              = (a.MyPerson  != null ? a.MyPerson.Age        : -1),
								PersonIdAddress  = (a.MyPerson  != null ? a.MyPerson.IdAddress  : -1),
								AddressIdAddress = (a.MyAddress != null ? a.MyAddress.IdAddress : -1),
								Street           = (a.MyAddress != null ? a.MyAddress.Street    : "Null-Value")
							}); 
		 */
        /// <summary>
        /// Returns A and B where A and B do not reference each other.
        /// </summary>
        public static IEnumerable<TResult> FullExcludingJoin<TSource, TInner, TKey, TResult>(
            this IEnumerable<TSource> source,
            IEnumerable<TInner> inner,
            Func<TSource, TKey> pk,
            Func<TInner, TKey> fk,
            Func<TSource, TInner, TResult> result)
        {
            var enumerable = source as TSource[] ?? source.ToArray();
            var inners = inner as TInner[] ?? inner.ToArray();
            var left = enumerable.LeftExcludingJoin(inners, pk, fk, result).ToList();
            var right = enumerable.RightExcludingJoin(inners, pk, fk, result).ToList();

            return left.Union(right);
        }

        public class TypeInconsistenceException : Exception
        {
            private string _message;

            public TypeInconsistenceException(string message)
            {
                _message = message;
            }
        }

        public static IEnumerable<TResult> Merge<TResult>(this IEnumerable<TResult> source,
            IEnumerable<TResult> sourceToMerge)
        {
            if (source.GetType() != sourceToMerge.GetType())
                throw new TypeInconsistenceException("the 'source Type is Diferent then 'SourceToMerce'");
            var enumerable = source as TResult[] ?? source.ToArray();
            var toMerge = sourceToMerge as TResult[] ?? sourceToMerge.ToArray();
            if (enumerable.IsNullOrEmpty() || toMerge.IsNullOrEmpty()) return default(IEnumerable<TResult>);
            var xs = enumerable.ToList();
            xs.AddRange(toMerge.ToArray());
            return xs;
        }
    }
}