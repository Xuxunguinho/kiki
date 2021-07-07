using System.Collections.Generic;
using Avax.Core.NoSQLData_Examples.Objects;

namespace Avax.Core.NoSQLData_Examples
{
    public static class Repository
    {
        public static List<FinalAgenda> DefaultFinalAgendaData => new List<FinalAgenda>
        {
            new FinalAgenda
            {
                StudentId = 1,
                StudentName = "Júlio Jose de Andrade Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 1, Grade = 10, Name = "L.PORT"},
            },
            new FinalAgenda
            {
                StudentId = 1,
                StudentName = "Júlio Jose de Andrade Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 2, Grade = 15, Name = "MATEMATICA"},
            },
            new FinalAgenda
            {
                StudentId = 1,
                StudentName = "Júlio Jose de Andrade Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 3, Grade = 14, Name = "INGLES"},
            },
            new FinalAgenda
            {
                StudentId = 1,
                StudentName = "Júlio Jose de Andrade Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 4, Grade = 14, Name = "FISICA"},
            },
            new FinalAgenda
            {
                StudentId = 1,
                StudentName = "Júlio Jose de Andrade Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 5, Grade = 18, Name = "BIOLOGIA"},
            },


            new FinalAgenda
            {
                StudentId = 2,
                StudentName = "Clotilde Isabel de Andrade Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 1, Grade = 10, Name = "L.PORT"},
            },
            new FinalAgenda
            {
                StudentId = 2,
                StudentName = "Clotilde Isabel de Andrade Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 2, Grade = 10, Name = "MATEMATICA"},
            },
            new FinalAgenda
            {
                StudentId = 2,
                StudentName = "Clotilde Isabel de Andrade Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 3, Grade = 18, Name = "INGLES"},
            },
            new FinalAgenda
            {
                StudentId = 2,
                StudentName = "Clotilde Isabel de Andrade Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 4, Grade = 18, Name = "FISICA"},
            },
            new FinalAgenda
            {
                StudentId = 2,
                StudentName = "Clotilde Isabel de Andrade Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 5, Grade = 18, Name = "BIOLOGIA"},
            },

            new FinalAgenda
            {
                StudentId = 3,
                StudentName = "João Manzodila de Andrade Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 1, Grade = 18, Name = "L.PORT"},
            },
            new FinalAgenda
            {
                StudentId = 3,
                StudentName = "João Manzodila de Andrade Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 2, Grade = 10, Name = "MATEMATICA"},
            },
            new FinalAgenda
            {
                StudentId = 3,
                StudentName = "João Manzodila de Andrade Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 3, Grade = 8, Name = "INGLES"},
            },
            new FinalAgenda
            {
                StudentId = 3,
                StudentName = "João Manzodila de Andrade Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 4, Grade = 12.5, Name = "FISICA"},
            },
            new FinalAgenda
            {
                StudentId = 3,
                StudentName = "João Manzodila de Andrade Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 5, Grade = 10, Name = "BIOLOGIA"},
            },

            new FinalAgenda
            {
                StudentId = 4,
                StudentName = "Josefa de Andrade Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 1, Grade = 18, Name = "L.PORT"},
            },
            new FinalAgenda
            {
                StudentId = 4,
                StudentName = "Josefa de Andrade Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 2, Grade = 10, Name = "MATEMATICA"},
            },
            new FinalAgenda
            {
                StudentId = 4,
                StudentName = "Josefa de Andrade Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 3, Grade = 7, Name = "INGLES"},
            },
            new FinalAgenda
            {
                StudentId = 4,
                StudentName = "Josefa de Andrade Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 4, Grade = 12.5, Name = "FISICA"},
            },
            new FinalAgenda
            {
                StudentId = 4,
                StudentName = "Josefa de Andrade Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 5, Grade = 10, Name = "BIOLOGIA"},
            },


            new FinalAgenda
            {
                StudentId = 5,
                StudentName = "Ana Maria  de Andrade Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 1, Grade = 18, Name = "L.PORT"},
            },
            new FinalAgenda
            {
                StudentId = 5,
                StudentName = "Ana Maria  de Andrade Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 2, Grade = 8, Name = "MATEMATICA"},
            },
            new FinalAgenda
            {
                StudentId = 5,
                StudentName = "Ana Maria  de Andrade Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 3, Grade = 18, Name = "INGLES"},
            },
            new FinalAgenda
            {
                StudentId = 5,
                StudentName = "Ana Maria  de Andrade Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 4, Grade = 12.5, Name = "FISICA"},
            },
            new FinalAgenda
            {
                StudentId = 5,
                StudentName = "Ana Maria  de Andrade Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 5, Grade = 10, Name = "BIOLOGIA"},
            },

            new FinalAgenda
            {
                StudentId = 6,
                StudentName = "Rosa Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 1, Grade = 18, Name = "L.PORT"},
            },
            new FinalAgenda
            {
                StudentId = 6,
                StudentName = "Rosa Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 2, Grade = 13.8, Name = "MATEMATICA"},
            },
            new FinalAgenda
            {
                StudentId = 6,
                StudentName = "Rosa Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 3, Grade = 18, Name = "INGLES"},
            },
            new FinalAgenda
            {
                StudentId = 6,
                StudentName = "Rosa Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 4, Grade = 12.5, Name = "FISICA"},
            },
            new FinalAgenda
            {
                StudentId = 6,
                StudentName = "Rosa Reis",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 5, Grade = 10, Name = "BIOLOGIA"},
            },

            new FinalAgenda
            {
                StudentId = 7,
                StudentName = "Serafim Guialo",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 1, Grade = 18, Name = "L.PORT"},
            },
            new FinalAgenda
            {
                StudentId = 7,
                StudentName = "Serafim Guialo",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 2, Grade = 13.8, Name = "MATEMATICA"},
            },
            new FinalAgenda
            {
                StudentId = 7,
                StudentName = "Serafim Guialo",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 3, Grade = 18, Name = "INGLES"},
            },
            new FinalAgenda
            {
                StudentId = 7,
                StudentName = "Serafim Guialo",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 4, Grade = 12.5, Name = "FISICA"},
            },
            new FinalAgenda
            {
                StudentId = 7,
                StudentName = "Serafim Guialo",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 5, Grade = 10, Name = "BIOLOGIA"},
            },

            new FinalAgenda
            {
                StudentId = 8,
                StudentName = "Nkawa Mayombo",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 1, Grade = 18, Name = "L.PORT"},
            },
            new FinalAgenda
            {
                StudentId = 8,
                StudentName = "Nkawa Mayombo",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 2, Grade = 13.8, Name = "MATEMATICA"},
            },
            new FinalAgenda
            {
                StudentId = 8,
                StudentName = "Nkawa Mayombo",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 3, Grade = 18, Name = "INGLES"},
            },
            new FinalAgenda
            {
                StudentId = 8,
                StudentName = "Nkawa Mayombo",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 4, Grade = 12.5, Name = "FISICA"},
            },
            new FinalAgenda
            {
                StudentId = 8,
                StudentName = "Nkawa Mayombo",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 5, Grade = 10, Name = "BIOLOGIA"},
            },

            new FinalAgenda
            {
                StudentId = 9,
                StudentName = "Manilson de Melo",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 1, Grade = 18, Name = "L.PORT"},
            },
            new FinalAgenda
            {
                StudentId = 9,
                StudentName = "Manilson de Melo",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 2, Grade = 13.8, Name = "MATEMATICA"},
            },
            new FinalAgenda
            {
                StudentId = 9,
                StudentName = "Manilson de Melo",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 3, Grade = 18, Name = "INGLES"},
            },
            new FinalAgenda
            {
                StudentId = 9,
                StudentName = "Manilson de Melo",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 4, Grade = 12.5, Name = "FISICA"},
            },
            new FinalAgenda
            {
                StudentId = 9,
                StudentName = "Manilson de Melo",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 5, Grade = 10, Name = "BIOLOGIA"},
            },

            new FinalAgenda
            {
                StudentId = 10,
                StudentName = "Alfa Guialo",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 1, Grade = 18, Name = "L.PORT"},
            },
            new FinalAgenda
            {
                StudentId = 10,
                StudentName = "Alfa Guialo",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 2, Grade = 13.8, Name = "MATEMATICA"},
            },
            new FinalAgenda
            {
                StudentId = 10,
                StudentName = "Alfa Guialo",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 3, Grade = 18, Name = "INGLES"},
            },
            new FinalAgenda
            {
                StudentId = 10,
                StudentName = "Alfa Guialo",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 4, Grade = 12.5, Name = "FISICA"},
            },
            new FinalAgenda
            {
                StudentId = 10,
                StudentName = "Alfa Guialo",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 5, Grade = 10, Name = "BIOLOGIA"},
            },


            new FinalAgenda
            {
                StudentId = 11,
                StudentName = "Benção Timoteo",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 1, Grade = 18, Name = "L.PORT"},
            },
            new FinalAgenda
            {
                StudentId = 11,
                StudentName = "Benção Timoteo",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 2, Grade = 13.8, Name = "MATEMATICA"},
            },
            new FinalAgenda
            {
                StudentId = 11,
                StudentName = "Benção Timoteo",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 3, Grade = 18, Name = "INGLES"},
            },
            new FinalAgenda
            {
                StudentId = 11,
                StudentName = "Benção Timoteo",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 4, Grade = 12.5, Name = "FISICA"},
            },
            new FinalAgenda
            {
                StudentId = 11,
                StudentName = "Benção Timoteo",
                FinalAgendaSubject = new FinalAgendaSubject {Id = 5, Grade = 10, Name = "BIOLOGIA"},
            },
        };

        public static readonly Configuration DefaultConfig = new Configuration
        {
            Description = "Example Course -> Grade 1",
            Course = 0,
            Grade = 0,

            CollectionsClass = new Dictionary<string, object>
            {
                {"negativa", "funcao({menorQ(nota,8)},@nota)"},
                {"positiva", "funcao({ maiorOig(nota, 10) }, @nota)"},
                {"deficiencia", "funcao({ &(maiorOig(nota,8),menorQ(nota,10))},@nota)"}
            },
            Results = new Dictionary<string, object>
            {
                {
                    "$R->", "if(&(menorQ(conta(negativas),1),\r\n  " +
                    "  !=>(negativas,lista(1,5),discId),\r\n   " +
                    "  menorOig(conta(deficiencias),0), \r\n     " +
                    "  !=> (deficiencias,lista(1),discId)),\r\n     " +
                    "       {$R->(\'aprovado\')},\r\n    " +
                    "     { if(&(menorQ(conta(negativas),1),\r\n    " +
                    "        !=>(negativas,lista(1,5,2),discId),\r\n            " +
                    "    menorOig(conta(deficiencias),2), \r\n     " +
                    "           !=> (deficiencias,lista(1,2,5),discId)),\r\n      " +
                    "          {$R->(\'recurso\')}\r\n                ,{$R->(\'reprovado\')})\r\n        " +
                    "       })\r\n      "
                }
            }
        };
    }
}