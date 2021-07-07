using System;
using System.Collections.Generic;
using NoSqliteX;

namespace Avax.Core.NoSQLData_Examples.Objects
{
    [Serializable]
    public class Configuration : IAvaxConfiguration
    {
        public string Description { get; set; }
        [NoSqliteXKey]
        public int Course { get;set;}
        [NoSqliteXKey]
        public int Grade { get; set; }
        public Dictionary<string, object> CollectionsClass { get; set; }
        public Dictionary<string, object> Results { get; set; } = new Dictionary<string, object>();



    }
}