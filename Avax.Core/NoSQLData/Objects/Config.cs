using System;
using System.Collections.Generic;

namespace Avax.Core.NoSQLData.Objects
{
    [Serializable]
    public class ConfigAvaliador
    {
        public string Nome { get; set; }
        [NoSqliteXKey]
        public int Curso { get;set;}
        [NoSqliteXKey]
        public int Classe { get; set; }
        public Dictionary<string, object> MapaClasseNotas { get; set; }
        public Dictionary<string, object> MapaResultados { get; set; }
     

       
    }
}