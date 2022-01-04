
/*
 * Copyright (c) 2020 Xuxunguinho - https://github.com/Xuxunguinho
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using System;
using System.Collections.Generic;

namespace kiki
{
    public class EvaluatorXTrigger<T> : EventArgs
    {
        public T ContextItem { get; }

        public IEnumerable<T> ContextCollection { get; }

        public EvaluatorXTrigger(T ctxItem,IEnumerable<T> ctxCollection)
        {
            this.ContextItem = ctxItem;
            this.ContextCollection = ctxCollection;
        }
    }
}