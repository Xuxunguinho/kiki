using System;

namespace Avax.Core.NoSQLData.Examples
{
    [Serializable]
    public class PautaFinal
    {
        [NoSqliteXKey]
        public int Id { get; set; }
        public string Nome { get; set; }
        [NoSqliteXKey]
        public Disciplina Disciplina { get; set; }

        public string Result { get; set; } = string.Empty;
        public string Obs { get; set; } = string.Empty;

    }
}