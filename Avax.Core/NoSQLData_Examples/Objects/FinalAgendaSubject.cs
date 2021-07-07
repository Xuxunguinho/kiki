using System;
using System.ComponentModel;

namespace Avax.Core.NoSQLData_Examples.Objects
{
    [Serializable]
    public class FinalAgendaSubject
    {
        [ReadOnly(true)]
        public int Id { get; set; }
        [ReadOnly(true)]
        public string Name { get; set; }
        public double Grade { get; set; }
    }
}