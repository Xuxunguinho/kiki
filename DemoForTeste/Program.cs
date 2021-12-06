using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Avax;
using Avax.NoSQLData_Examples;
using Avax.NoSQLData_Examples.Objects;
using Lex;

namespace DemoForTeste
{
    internal static class Program


    {
        private static Configuration _config;

        private static  void  Main(string[] args)
        {
             Starter.NoSqlStart(Directory.GetCurrentDirectory() + "\\Data\\");
            LoadConfigFile(0, 0);
            TestScript();
            Console.ReadLine();
        }

        private static void TestScript(bool supressSucessMessage = false)
        {
            if (_config is null)
            {
                Console.WriteLine();
                return;
            }

            var script = _config.Results["$R->"].ToString().SupressSpace();

            var atv = new Avaliator<FinalAgenda>();

            var verify = atv.Run(XTables.FinalAgenda.Items, x => x.StudentName,
                x => x.StudentId, 
                (t1, t2) => t1.StudentId == t2.StudentId, 
                x => x.FinalAgendaSubject.Grade,
                x => x.FinalAgendaSubject.Id, x => x.FinalAgendaSubject.Name,
                x => x.Result, x => x.Note,script, _config.CollectionsClass
            );


            Console.WriteLine(atv.GeneralDescription);
           
        }

        private static void LoadConfigFile(int curso, int classe)
        {
            var configs = XTables.Configs;
            _config = configs.Get(curso, classe);
            if (_config is null) return;
            Console.WriteLine(_config.Description);


            foreach (var x in _config.CollectionsClass.Keys)
            {
                Console.WriteLine(x);
            }

            foreach (var x in _config.Results.Keys)
                Console.WriteLine(x);


            // var code = list.Aggregate(string.Empty, (current, x) => current + (x.IsNullOrEmpty() ? $"{x}s" : $"{x}s" + "|"));

            // _lisRegClnObjects = @"\b(#)\b".Replace("#",code);

            // if (mainTree.Nodes?.Count > 0)
            //     tempNodeSelected = mainTree.Nodes[0];
        }
    }
}