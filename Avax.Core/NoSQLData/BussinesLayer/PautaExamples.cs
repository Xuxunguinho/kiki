using System.Collections.Generic;
using System.Linq;
using Avax.Core.NoSQLData.Examples;
using Avax.Core.NoSQLData.Interfaces;
using Lex;
using NoSqliteX;

namespace Avax.Core.NoSQLData.BussinesLayer
{
    [NoSqLiteXFileTable("PautaExamples")]
    internal class PautaExamples : NoSqliteXFileTable<PautaFinal>, IPautaExamples
    {
        public List<Disciplina> GetDisciplinas(int id)
        {
            return Items?.Where(x => x.Id == id)?.Select(x => x.Disciplina)?.ToList();
        }

        public List<PautaFinal> GetListaAlunos()
        {
            return Items?.Distinct(x=> x.Id)?.OrderBy(x=> x.Nome)?.ToList();
        }
    }
}