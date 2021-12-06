using System;
using System.Collections.Generic;

namespace Avax
{
    public class AvaxTrigger<T> : EventArgs
    {
        public T ContextItem { get; }

        public IEnumerable<T> ContextCollection { get; }

        public AvaxTrigger(T ctxItem,IEnumerable<T> ctxCollection)
        {
            this.ContextItem = ctxItem;
            this.ContextCollection = ctxCollection;
        }
    }
}