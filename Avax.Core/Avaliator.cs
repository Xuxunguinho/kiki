using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Avax.Core.NoSQLData.Examples;
using Lex;

namespace Avax.Core
{
    public class Avaliator<T>
    {
        public static List<PautaFinal> PautaFinalExampleRepository => new List<PautaFinal>
        {
            new PautaFinal
            {
                Id = 1,
                Nome = "Júlio Jose de Andrade Reis",
                Disciplina = new Disciplina {Id = 1, Nota = 10, Nome = "L.PORT"},
            },
            new PautaFinal
            {
                Id = 1,
                Nome = "Júlio Jose de Andrade Reis",
                Disciplina = new Disciplina {Id = 2, Nota = 15, Nome = "MATEMATICA"},
            },
            new PautaFinal
            {
                Id = 1,
                Nome = "Júlio Jose de Andrade Reis",
                Disciplina = new Disciplina {Id = 3, Nota = 14, Nome = "INGLES"},
            },
            new PautaFinal
            {
                Id = 1,
                Nome = "Júlio Jose de Andrade Reis",
                Disciplina = new Disciplina {Id = 4, Nota = 14, Nome = "FISICA"},
            },
            new PautaFinal
            {
                Id = 1,
                Nome = "Júlio Jose de Andrade Reis",
                Disciplina = new Disciplina {Id = 5, Nota = 18, Nome = "BIOLOGIA"},
            },


            new PautaFinal
            {
                Id = 2,
                Nome = "Clotilde Isabel de Andrade Reis",
                Disciplina = new Disciplina {Id = 1, Nota = 10, Nome = "L.PORT"},
            },
            new PautaFinal
            {
                Id = 2,
                Nome = "Clotilde Isabel de Andrade Reis",
                Disciplina = new Disciplina {Id = 2, Nota = 10, Nome = "MATEMATICA"},
            },
            new PautaFinal
            {
                Id = 2,
                Nome = "Clotilde Isabel de Andrade Reis",
                Disciplina = new Disciplina {Id = 3, Nota = 18, Nome = "INGLES"},
            },
            new PautaFinal
            {
                Id = 2,
                Nome = "Clotilde Isabel de Andrade Reis",
                Disciplina = new Disciplina {Id = 4, Nota = 18, Nome = "FISICA"},
            },
            new PautaFinal
            {
                Id = 2,
                Nome = "Clotilde Isabel de Andrade Reis",
                Disciplina = new Disciplina {Id = 5, Nota = 18, Nome = "BIOLOGIA"},
            },

            new PautaFinal
            {
                Id = 3,
                Nome = "João Manzodila de Andrade Reis",
                Disciplina = new Disciplina {Id = 1, Nota = 18, Nome = "L.PORT"},
            },
            new PautaFinal
            {
                Id = 3,
                Nome = "João Manzodila de Andrade Reis",
                Disciplina = new Disciplina {Id = 2, Nota = 10, Nome = "MATEMATICA"},
            },
            new PautaFinal
            {
                Id = 3,
                Nome = "João Manzodila de Andrade Reis",
                Disciplina = new Disciplina {Id = 3, Nota = 8, Nome = "INGLES"},
            },
            new PautaFinal
            {
                Id = 3,
                Nome = "João Manzodila de Andrade Reis",
                Disciplina = new Disciplina {Id = 4, Nota = 12.5, Nome = "FISICA"},
            },
            new PautaFinal
            {
                Id = 3,
                Nome = "João Manzodila de Andrade Reis",
                Disciplina = new Disciplina {Id = 5, Nota = 10, Nome = "BIOLOGIA"},
            },

            new PautaFinal
            {
                Id = 4,
                Nome = "Josefa de Andrade Reis",
                Disciplina = new Disciplina {Id = 1, Nota = 18, Nome = "L.PORT"},
            },
            new PautaFinal
            {
                Id = 4,
                Nome = "Josefa de Andrade Reis",
                Disciplina = new Disciplina {Id = 2, Nota = 10, Nome = "MATEMATICA"},
            },
            new PautaFinal
            {
                Id = 4,
                Nome = "Josefa de Andrade Reis",
                Disciplina = new Disciplina {Id = 3, Nota = 7, Nome = "INGLES"},
            },
            new PautaFinal
            {
                Id = 4,
                Nome = "Josefa de Andrade Reis",
                Disciplina = new Disciplina {Id = 4, Nota = 12.5, Nome = "FISICA"},
            },
            new PautaFinal
            {
                Id = 4,
                Nome = "Josefa de Andrade Reis",
                Disciplina = new Disciplina {Id = 5, Nota = 10, Nome = "BIOLOGIA"},
            },


            new PautaFinal
            {
                Id = 5,
                Nome = "Ana Maria  de Andrade Reis",
                Disciplina = new Disciplina {Id = 1, Nota = 18, Nome = "L.PORT"},
            },
            new PautaFinal
            {
                Id = 5,
                Nome = "Ana Maria  de Andrade Reis",
                Disciplina = new Disciplina {Id = 2, Nota = 8, Nome = "MATEMATICA"},
            },
            new PautaFinal
            {
                Id = 5,
                Nome = "Ana Maria  de Andrade Reis",
                Disciplina = new Disciplina {Id = 3, Nota = 18, Nome = "INGLES"},
            },
            new PautaFinal
            {
                Id = 5,
                Nome = "Ana Maria  de Andrade Reis",
                Disciplina = new Disciplina {Id = 4, Nota = 12.5, Nome = "FISICA"},
            },
            new PautaFinal
            {
                Id = 5,
                Nome = "Ana Maria  de Andrade Reis",
                Disciplina = new Disciplina {Id = 5, Nota = 10, Nome = "BIOLOGIA"},
            },

            new PautaFinal
            {
                Id = 6,
                Nome = "Rosa Reis",
                Disciplina = new Disciplina {Id = 1, Nota = 18, Nome = "L.PORT"},
            },
            new PautaFinal
            {
                Id = 6,
                Nome = "Rosa Reis",
                Disciplina = new Disciplina {Id = 2, Nota = 13.8, Nome = "MATEMATICA"},
            },
            new PautaFinal
            {
                Id = 6,
                Nome = "Rosa Reis",
                Disciplina = new Disciplina {Id = 3, Nota = 18, Nome = "INGLES"},
            },
            new PautaFinal
            {
                Id = 6,
                Nome = "Rosa Reis",
                Disciplina = new Disciplina {Id = 4, Nota = 12.5, Nome = "FISICA"},
            },
            new PautaFinal
            {
                Id = 6,
                Nome = "Rosa Reis",
                Disciplina = new Disciplina {Id = 5, Nota = 10, Nome = "BIOLOGIA"},
            },

            new PautaFinal
            {
                Id = 7,
                Nome = "Serafim Guialo",
                Disciplina = new Disciplina {Id = 1, Nota = 18, Nome = "L.PORT"},
            },
            new PautaFinal
            {
                Id = 7,
                Nome = "Serafim Guialo",
                Disciplina = new Disciplina {Id = 2, Nota = 13.8, Nome = "MATEMATICA"},
            },
            new PautaFinal
            {
                Id = 7,
                Nome = "Serafim Guialo",
                Disciplina = new Disciplina {Id = 3, Nota = 18, Nome = "INGLES"},
            },
            new PautaFinal
            {
                Id = 7,
                Nome = "Serafim Guialo",
                Disciplina = new Disciplina {Id = 4, Nota = 12.5, Nome = "FISICA"},
            },
            new PautaFinal
            {
                Id = 7,
                Nome = "Serafim Guialo",
                Disciplina = new Disciplina {Id = 5, Nota = 10, Nome = "BIOLOGIA"},
            },

            new PautaFinal
            {
                Id = 8,
                Nome = "Nkawa Mayombo",
                Disciplina = new Disciplina {Id = 1, Nota = 18, Nome = "L.PORT"},
            },
            new PautaFinal
            {
                Id = 8,
                Nome = "Nkawa Mayombo",
                Disciplina = new Disciplina {Id = 2, Nota = 13.8, Nome = "MATEMATICA"},
            },
            new PautaFinal
            {
                Id = 8,
                Nome = "Nkawa Mayombo",
                Disciplina = new Disciplina {Id = 3, Nota = 18, Nome = "INGLES"},
            },
            new PautaFinal
            {
                Id = 8,
                Nome = "Nkawa Mayombo",
                Disciplina = new Disciplina {Id = 4, Nota = 12.5, Nome = "FISICA"},
            },
            new PautaFinal
            {
                Id = 8,
                Nome = "Nkawa Mayombo",
                Disciplina = new Disciplina {Id = 5, Nota = 10, Nome = "BIOLOGIA"},
            },

            new PautaFinal
            {
                Id = 9,
                Nome = "Manilson de Melo",
                Disciplina = new Disciplina {Id = 1, Nota = 18, Nome = "L.PORT"},
            },
            new PautaFinal
            {
                Id = 9,
                Nome = "Manilson de Melo",
                Disciplina = new Disciplina {Id = 2, Nota = 13.8, Nome = "MATEMATICA"},
            },
            new PautaFinal
            {
                Id = 9,
                Nome = "Manilson de Melo",
                Disciplina = new Disciplina {Id = 3, Nota = 18, Nome = "INGLES"},
            },
            new PautaFinal
            {
                Id = 9,
                Nome = "Manilson de Melo",
                Disciplina = new Disciplina {Id = 4, Nota = 12.5, Nome = "FISICA"},
            },
            new PautaFinal
            {
                Id = 9,
                Nome = "Manilson de Melo",
                Disciplina = new Disciplina {Id = 5, Nota = 10, Nome = "BIOLOGIA"},
            },

            new PautaFinal
            {
                Id = 10,
                Nome = "Alfa Guialo",
                Disciplina = new Disciplina {Id = 1, Nota = 18, Nome = "L.PORT"},
            },
            new PautaFinal
            {
                Id = 10,
                Nome = "Alfa Guialo",
                Disciplina = new Disciplina {Id = 2, Nota = 13.8, Nome = "MATEMATICA"},
            },
            new PautaFinal
            {
                Id = 10,
                Nome = "Alfa Guialo",
                Disciplina = new Disciplina {Id = 3, Nota = 18, Nome = "INGLES"},
            },
            new PautaFinal
            {
                Id = 10,
                Nome = "Alfa Guialo",
                Disciplina = new Disciplina {Id = 4, Nota = 12.5, Nome = "FISICA"},
            },
            new PautaFinal
            {
                Id = 10,
                Nome = "Alfa Guialo",
                Disciplina = new Disciplina {Id = 5, Nota = 10, Nome = "BIOLOGIA"},
            },


            new PautaFinal
            {
                Id = 11,
                Nome = "Benção Timoteo",
                Disciplina = new Disciplina {Id = 1, Nota = 18, Nome = "L.PORT"},
            },
            new PautaFinal
            {
                Id = 11,
                Nome = "Benção Timoteo",
                Disciplina = new Disciplina {Id = 2, Nota = 13.8, Nome = "MATEMATICA"},
            },
            new PautaFinal
            {
                Id = 11,
                Nome = "Benção Timoteo",
                Disciplina = new Disciplina {Id = 3, Nota = 18, Nome = "INGLES"},
            },
            new PautaFinal
            {
                Id = 11,
                Nome = "Benção Timoteo",
                Disciplina = new Disciplina {Id = 4, Nota = 12.5, Nome = "FISICA"},
            },
            new PautaFinal
            {
                Id = 11,
                Nome = "Benção Timoteo",
                Disciplina = new Disciplina {Id = 5, Nota = 10, Nome = "BIOLOGIA"},
            },
        };

        private readonly Dictionary<string, object> _mapaClassesNota;
        private readonly string _script;

        public Avaliator(string script, Dictionary<string, object> mapaClassesNota)
        {
            _mapaClassesNota = mapaClassesNota;
            _script = script;
        }

        private static Func<IEnumerable<string>, object, double> Nota => (nota, x) => nota.GetValue(x).ToDouble();
        private static Func<IEnumerable<string>, object, string> Nome => (nota, x) => nota.GetValue(x).ToString();

        public bool Avaliar(IEnumerable<T> source, Expression<Func<T, object>> pKeyDistinct, Func<T, T,
                bool> pKeyAll, Expression<Func<T, object>> notaKey,
            Expression<Func<T, object>> nomeKey, Expression<Func<T, object>> resultKey,
            Expression<Func<T, object>> obsKey, Expression<Func<T, object>> discPkKey,
            Expression<Func<T, object>> discNameKey, out string message)
        {
            try
            {
#if DEBUG

                var stow = new Stopwatch();
                stow.Start();
#endif
                // expressions

                var exprNota = notaKey.DeserializeExpression();
                var exprNome = nomeKey.DeserializeExpression();
                var exprResult = resultKey.DeserializeExpression();
                var exprObs = obsKey.DeserializeExpression();
                var exprDiscName = discNameKey.DeserializeExpression();
                var exprDiscPk = discPkKey.DeserializeExpression();
                
                // binding
                var helper = new ScriptHelper<T>();
                helper.AddBind("idDisciplina", exprDiscPk);
                helper.AddBind("discId", exprDiscPk);
                helper.AddBind("nomeDisciplina", exprDiscName);
                helper.AddBind("discName", exprDiscName);

                helper.SetValueForBind("$nota", exprNota);
                helper.SetValueForBind("$result", exprResult);
                helper.SetValueForBind("$obs", exprObs);
                helper.SetValueForBind("$pkAll", pKeyAll);

                var enumerable = source as T[] ?? source.ToArray();

                foreach (var s in enumerable.Distinct(pKeyDistinct))
                {
                    var av = enumerable.Where(x => pKeyAll(x, s))?.ToArray();

                    helper.SetValueForBind("$aluno", s);
                    helper.SetValueForBind("notasAluno", av);

                    helper.ClasseNotasBinder(_mapaClassesNota, av, exprNota);

                    helper.Run(_script);
                }
#if DEBUG


                stow.Stop();
                message =
                    $"The results were presented in -> {new TimeSpan(stow.ElapsedMilliseconds).Milliseconds}";
#endif
                return true;
            }
            catch (Exception e)
            {
                message = (e.Message);
                return false;
            }
        }
    }
}