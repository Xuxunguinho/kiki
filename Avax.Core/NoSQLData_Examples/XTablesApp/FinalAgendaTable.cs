using System.Collections.Generic;
using System.Linq;
using Avax.NoSQLData_Examples.Interfaces;
using Avax.NoSQLData_Examples.Objects;
using Lex;
using NoSqliteX;

namespace Avax.NoSQLData_Examples.XTablesApp
{
    [NoSqLiteXFileTable("FinalAgendaTable")]
    internal class FinalAgendaTable : NoSqliteXFileTable<FinalAgenda>, IFinalAgenda
    {
        public List<FinalAgendaSubject> GetSubjects(int id)
        {
            return Items?.Where(x => x.StudentId == id)?.Select(x => x.FinalAgendaSubject)?.ToList();
        }

        public List<FinalAgenda> GetStudents()
        {
            return Items?.Distinct(x=> x.StudentId)?.OrderBy(x=> x.StudentName)?.ToList();
        }
    }
}