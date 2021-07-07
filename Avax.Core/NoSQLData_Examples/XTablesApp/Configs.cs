using System.Linq;
using Avax.Core.NoSQLData_Examples.Interfaces;
using Avax.Core.NoSQLData_Examples.Objects;
using NoSqliteX;

namespace Avax.Core.NoSQLData_Examples.XTablesApp
{
    [NoSqLiteXFileTable("CONFIGS")]
    internal class Configs : NoSqliteXFileTable<Configuration>, IConfigs
    {
        public Configuration Get(int curso , int classe)
        {
            return Items.FirstOrDefault(x => x.Course == curso && x.Grade == classe);
        }
    }
}