using System.Linq;
using Avax.NoSQLData_Examples.Interfaces;
using Avax.NoSQLData_Examples.Objects;
using NoSqliteX;

namespace Avax.NoSQLData_Examples.XTablesApp
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