using System;
using NoSqliteX;

namespace Avax.Core.NoSQLData_Examples.Objects
{
    [Serializable]
    public class FinalAgenda
    {
        [NoSqliteXKey]
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        [NoSqliteXKey]
        public FinalAgendaSubject FinalAgendaSubject { get; set; }

        public string Result { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;

    }
}