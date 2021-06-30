using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using Avax.Core.NoSQLData;
using Avax.Core.NoSQLData.BussinesLayer;

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
                NoSqLiteXStarter.Start(new NoSqLiteXStarterParams
                {
                    AssemblyFormat = FormatterAssemblyStyle.Full,
                    DataEncryptionPassword = string.Empty,
                    FilesEncryptionPassword = string.Empty,
                    StreamingContextState = StreamingContextStates.Persistence,
                    TypeFormat = FormatterTypeStyle.XsdString
                }, path);

                InstanceBuilder.AvConfigs = new Configs();
                InstanceBuilder.PautaExamples = new PautaExamples();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        /// <summary>
        /// try conect with de SQLServer
        /// </summary>
        /// <param name="conectionString"></param>
        /// <returns></returns>
        public static bool SqlStart(string conectionString)
        {
            try
            {
                var cod = new Sape.Data.Core.ApiSettings().Init("##443$KJRJKRKSDSLKDL",
                    conectionString);
                return cod;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}