using System.Linq;
using Avax.Core.NoSQLData.Interfaces;
using Avax.Core.NoSQLData.Objects;
using NoSqliteX;

namespace Avax.Core.NoSQLData.BussinesLayer
{
    [NoSqLiteXFileTable("CONFIGS")]
    internal class Configs : NoSqliteXFileTable<ConfigAvaliador>, IConfigs
    {
        public ConfigAvaliador Get(int curso , int classe)
        {
            return Items.FirstOrDefault(x => x.Curso == curso && x.Classe == classe);
        }
    }
}