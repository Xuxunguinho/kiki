/*
 * Copyright (c) 2018 Thomas Hansen - thomas@gaiasoul.com
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using System;

namespace kiki.lizzie
{
    /// <summary>
    /// Convenience class to more easily compile a lambda function from Lizzie code.
    /// </summary>
    public static class LambdaCompiler
    {
        /// <summary>
        /// Compiling the specified code to a lambda function, without requiring
        /// the caller to bind the evaluation towards a particular type.
        /// 
        /// Will bind to all the default 'keywords' in Lizzie found in the
        /// Functions class.
        /// </summary>
        /// <returns>The compiled lambda function.</returns>
        /// <param name="code">Lizzie code to compile.</param>
        public static Func<object> Compile(string code)
        {
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<Nothing>(tokenizer, code);
            var binder = new Binder<Nothing>();
            BindFunctions(binder);
            var nothing = new Nothing();
            return new Func<object>(() => function(nothing, binder));
        }

        /// <summary>
        /// Compiles the specified code, binding to the specified context, and
        /// returns a function allowing you to evaluate the specified code.
        /// 
        /// Will bind to all the default 'keywords' in Lizzie found in the
        /// Functions class.
        /// </summary>
        /// <returns>The compiled lambda function.</returns>
        /// <param name="context">Context to bind the evaluation towards.</param>
        /// <param name="code">Lizzie code to compile.</param>
        /// <param name="bindDeep">If true will perform binding on type of instance, and not on type TContext.</param>
        /// <typeparam name="TContext">The type of context you want to bind towards.</typeparam>
        public static Func<object> Compile<TContext>(TContext context, string code, bool bindDeep = false)
        {
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<TContext>(tokenizer, code);
            var binder = new Binder<TContext>(bindDeep ? context : default(TContext));
            BindFunctions(binder);
            return new Func<object>(() => function(context, binder));
        }

        /// <summary>
        /// Compiles the specified code, binding to the specified context, and
        /// returns a function allowing you to evaluate the specified code.
        /// 
        /// Will not bind the binder to any functions. If you wish to bind the
        /// binder to the default functions, you can use 'LambdaCompiler.BindFunctions'.
        /// 
        /// If you use this overload, and you cache your Binder, you will
        /// experience significant performance improvements, since the process of creating
        /// a Binder has some overhead, due to the compilation of lambda expressions,
        /// and dependencies upon reflection. If you do, you must never use your
        /// "master" Binder instance, but Clone it every time you want to use it.
        /// </summary>
        /// <returns>The compiled lambda function.</returns>
        /// <param name="context">Context to bind the lambda towards.</param>
        /// <param name="binder">Binder to use for your lambda.</param>
        /// <param name="code">Lizzie code to compile.</param>
        /// <typeparam name="TContext">The type of context you want to bind towards.</typeparam>
        public static Func<object> Compile<TContext>(TContext context, Binder<TContext> binder, string code)
        {
            var tokenizer = new Tokenizer(new LizzieTokenizer());
            var function = Compiler.Compile<TContext>(tokenizer, code);
            return new Func<object>(() => function(context, binder));
        }

        /// <summary>
        /// Binds the specified binder to all default functions in Lizzie from the Functions class.
        /// </summary>
        /// <param name="binder">Binder to bind.</param>
        /// <typeparam name="TContext">The type of context you want to use.</typeparam>
        public static void BindFunctions<TContext>(Binder<TContext> binder)
        {
            // Variables.
            binder["var"] = Functions<TContext>.Var;
            binder["set"] = Functions<TContext>.Set;

            // Comparison functions. english
            binder["if"] = Functions<TContext>.If;
            binder["eq"] = Functions<TContext>.Eq;
            binder["mt"] = Functions<TContext>.Mt;
            binder["lt"] = Functions<TContext>.Lt;
            binder["mte"] = Functions<TContext>.Mte;
            binder["lte"] = Functions<TContext>.Lte;
            binder["not"] = Functions<TContext>.Not;
            // Comparison functions. Portuguese
            binder["menorOig"] = Functions<TContext>.Lte;
            binder["nao"] = Functions<TContext>.Not;
            binder["maiorOig"] = Functions<TContext>.Mte;
            binder["maiorQ"] = Functions<TContext>.Mt;
            binder["menorQ"] = Functions<TContext>.Lt;
            binder["igual"] = Functions<TContext>.Eq;
            binder["se"] = Functions<TContext>.If;

            // Boolean algebraic functions. English
            binder["any"] = Functions<TContext>.Any;
            binder["all"] = Functions<TContext>.All;
         
            // Boolean algebraic functions. Portuguese
            binder["qualquer"] = Functions<TContext>.Any;
            binder["todos"] = Functions<TContext>.All;
            
            // Function functions. English
            binder["function"] = Functions<TContext>.Function;
            binder["apply"] = Functions<TContext>.Apply;
            // Function functions. English
            binder["funcao"] = Functions<TContext>.Function;
            
            // List functions,
            binder["list"] = Functions<TContext>.List;
            binder["slice"] = Functions<TContext>.Slice;
            // List functions. Portuguese
            binder["lista"] = Functions<TContext>.List;
            
            // Map functions,
            binder["map"] = Functions<TContext>.Map;
            // Map functions. Portuguese
            binder["mapa"] = Functions<TContext>.Map;

            // Common functions for map and list.
            binder["get"] = Functions<TContext>.Get;
            binder["count"] = Functions<TContext>.Count;
            binder["add"] = Functions<TContext>.AddValue;
            binder["each"] = Functions<TContext>.Each;
            // Common functions for map and list. Portuguese
            binder["pega"] = Functions<TContext>.Get;
            binder["conta"] = Functions<TContext>.Count;
            binder["cada"] = Functions<TContext>.Each;
            
            // Conversion functions.
            binder["string"] = Functions<TContext>.String;
            binder["number"] = Functions<TContext>.Number;
          
            // Conversion functions. Portuguese
            binder["numero"] = Functions<TContext>.Number;
            binder["literal"] = Functions<TContext>.String;
            // binder["json"] = Functions<TContext>.Json;

            // Math functions.
            binder["+"] = Functions<TContext>.Add;
            binder["sum"] = Functions<TContext>.Add;
            binder["-"] = Functions<TContext>.Subtract;
            binder["*"] = Functions<TContext>.Multiply;
            binder["/"] = Functions<TContext>.Divide;
            binder["%"] = Functions<TContext>.Modulo;
            // Math functions. Literal in Portuguese
            binder["modulo"] = Functions<TContext>.Modulo;
            binder["divide"] = Functions<TContext>.Divide;
            binder["soma"] = Functions<TContext>.Add;
            binder["subtrai"] = Functions<TContext>.Subtract;
            binder["multiplica"] = Functions<TContext>.Multiply;
           
            // other 

            binder["write"] = Functions<TContext>.Write;
            //Portuguese
            binder["escreva"] = Functions<TContext>.Write;
            // String functions.
            binder["substr"] = Functions<TContext>.Substr;
            binder["length"] = Functions<TContext>.Length;
            binder["replace"] = Functions<TContext>.Replace;
            // String functions. Portuguese
            binder["tamanho"] = Functions<TContext>.Length;
           
            // The eval function.
            binder["eval"] = Functions<TContext>.Eval;

            // Null is simply a constant yielding null.
            binder["null"] = null;
            // Null is simply a constant yielding null. Portuguese
            binder["nulo"] = null;
            binder["true"] = true;
            binder["false"] = false;
            
            binder["verdadeira"] = true;
            binder["falsa"] = false;
        }

        /// <summary>
        /// Empty class to help create a Lambda function without needing to
        /// bind to some specific type, but rather simply evaluate Lizzie code,
        /// without having to bind it to anything in particular.
        /// </summary>
        public class Nothing { };
    }
}