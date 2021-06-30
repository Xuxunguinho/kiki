using System;
using System.ComponentModel;

namespace Avax.Core.NoSQLData.Examples
{
    [Serializable]
    public class Disciplina
    {
        [ReadOnly(true)]
        public int Id { get; set; }
        [ReadOnly(true)]
        public string Nome { get; set; }
        public double Nota { get; set; }
    }
}