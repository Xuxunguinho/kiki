using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using Avax.Core.NoSQLData_Examples;
using Avax.Core.NoSQLData_Examples.XTablesApp;
using NoSqliteX;
namespace Avax.Core
{
    public static class Starter
    {
        /// <summary>
        ///  set NoSQLiteX data path location
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool NoSqlStart(string path)
        {
            try
            {
                // Setting up the serializer
                
                NoSqLiteXStarter.Start(new NoSqLiteXStarterParams
                {
                    AssemblyFormat = FormatterAssemblyStyle.Full,
                    DataEncryptionPassword = string.Empty,
                    FilesEncryptionPassword = string.Empty,
                    StreamingContextState = StreamingContextStates.Persistence,
                    TypeFormat = FormatterTypeStyle.XsdString
                }, path);

                //  initializing the tables
                
                XTables.Configs = new Configs();
                XTables.FinalAgenda = new FinalAgendaTable();
                
                // taking advantage of the boot time to enter predefined settings for examples
                XTables.Configs.Insert(Repository.DefaultConfig);
                if (XTables.FinalAgenda.Items.Count <= 0)
                    XTables.FinalAgenda.Insert(Repository.DefaultFinalAgendaData);
               
                
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
      
    }
}