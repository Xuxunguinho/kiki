
# kiki  - A generic system for evaluating Data
 A generic system for evaluating and classifying data into enumerators (IEnummerable's) where their classification terms and conditions can be dynamically scripted.

I initially designed it for academic purposes, but it can be used for various purposes.
### Why 'kiki' ?
I called the project 'kiki' because it's the name of a good friend I have - practically the only human being with whom I can express what I really feel when it comes to emotions, and also because I developed this project while trying to distract myself so as not to feel missing her while she couldn't be around.
### The story behind Kiki
Working at Qbinary - as one of the developers of **SAPE (School Process Automation System)**, we needed something capable of assessing students based on Angola's complex **assessment system** (presenting the results like whether a student passes or fails, or approved leaving some subjects to be completed), like any good team, we developed an algorithm capable of doing what we needed, but it was not dynamic and efficient, if we had to make any changes in the Assessment system we would have to recompile the system; so I decided to challenge my brain and develop something that was more **dynamic, fast, effective**, solving the problems mentioned above.
### Important :warning:
Within this project is also [Lizzie](https://github.com/polterguy/lizzie "Lizzie") - is a dynamic scripting language for .Net based upon a design pattern called "Symbolic Delegates". This allows you to execute dynamically created scripts, that does neither compile nor are interpreted, but instead "compiles" directly down to managed CLR delegates. ...it was modified by me to achieve my purposes
### Technologies and environments

This project uses the .Net Standard 2.0 Framework

IDE: [Jetbrains Rider](https://www.jetbrains.com/rider/)

<img src="https://user-images.githubusercontent.com/74734491/148679922-a0d46288-8d51-4748-bb33-96ee759eb7ef.jpg" alt="" data-canonical-src="https://user-images.githubusercontent.com/74734491/148679862-8607cc11-3fb7-46eb-8ae0-8be1729406a3.jpg" width="100" height="50" />  <img src="https://user-images.githubusercontent.com/74734491/148679100-3059af09-27af-464e-ac47-f10d91279f57.png" alt="" data-canonical-src="https://user-images.githubusercontent.com/74734491/148679100-3059af09-27af-464e-ac47-f10d91279f57.png" width="50" height="50" /> 

# Small documentation
basics of how kiki works
### kiki's main method
it is possible that new overloads of this method appear while we are updating and improving the project, after all, the objective is to create a library that can be able to perform data evaluations in different ways.

```csharp
public KikiEvaluatorResultMessage Run(IEnumerable<T> source,
                     Expression<Func<T, object>> itemDisplayValue, 
                     Expression<Func<T, object>> itemKey, 
                     Func<T, T,bool> itemKeyDistinct,
                     Expression<Func<T, object>> evalKey, 
                     Expression<Func<T, object>> evalBasedKey,
                     Expression<Func<T, object>> evalBasedKeyDisplayValue,
                     Expression<Func<T, object>> resultKey,
                     Expression<Func<T, object>> obsKey, 
                     string script, 
                     Dictionary<string, string> collectionClass = null,
                     Dictionary<string, string> collectionSubclasses = null)
```


the above method execute assessment based on subsets extracted from the same dataset 'source'

### Explaining the parameters of method

1. **source** -> *dataset*
1. **itemDisplayValue** -> *the value to show for each item evaluated, for example,
 the Name of a student when evaluating it*
1. **itemKey** -> *key to identify each entity in the dataset(source) - (**contextItem**)*
1. **itemKeyDistinct** -> *for the where condition, to create the entity's 
 data subset - (**Context Collection**)*
1. **evalKey** -> *the field to be evaluated*
1. **evalBasedKey** -> *the field on which the rating is based*
1. **evalBasedKeyDisplayValue** -> *value to show in results or statistics for 'evalBasedKey' field*         
1. **resultKey** -> *the collection field where the result will be assigned*
1. **obsKey** -> *the collection field where the evaluator will assign notes based on the result*
1. **script** -> *the main evaluation script*
1. **collectionClass** -> *sorted subsets extracted from the context collection (these will appear described in the 'obsKey' note)*
1. **collectionSubclasses** -> *sorted subsets extracted from the context collection 
         (these will not appear described in the 'obsKey' 
         observation as they are only auxiliaries)*
         
## kiki Script
As I explained before kiki uses the scripting language [Lizzie](https://github.com/polterguy/lizzie "Lizzie") as, so its syntax is based on Lizzie. so far we have also added the systaxis language for portuguese.

As we will show below, the main method uses script coming from the sorted data subsets (they are used to create the subsets) and also the main script (runs the evaluation as a whole)
### Sorted data subsets -> collectionClass,collectionSubclasses (main method parameters)
``` csharp
           CollectionsClass = new Dictionary<string, string>
            {
                {"negativa", "{menorQ(MFD, 8)}"},
                {"positiva", "{maiorOig(MFD, 10)}"},
                {"deficiencia", "{&(maiorOig(MFD,8),menorQ(MFD,10))}"}
            }
```
 The dictionary **key** represents the subset name and its **value** must be a Lizzie script as shown above.
 The expression ```{menorQ(MFD, 8)}``` represents the condition similar to the **Where clause in Linq or SQL**. all subsets of data must be represented by a similar script as in the code example above.


### Main Script
``` csharp
se(maiorQ(somaT(FaltasNaoJustificadas),5), {$R->('Reprovado F')},
{ 
    se(&(igual(conta(negativas),0),igual(conta(deficiencias),0)),
    {
            se(Ajudado,{$R->('Aprovado A')},{$R->('Aprovado')})
    },
    {
       /* verificando*/
       se(&(igual(conta(negativas),0), menorOig(conta(deficiencias),2),
       !=> (deficiencias,disciplinas_xaves,IdDisciplina)),
                {
                  $R->('Recurso')
                }
               ,{
                   $R->('Reprovado')
                })
    })

})
```

# Sample Usage
```csharp
var  evaluator = new kiki.Evaluator<uspCarregarPautaFinalFGParaProcessamentoResult>();
var message = evaluator.Run(Data, x => x.Nome,
                     x => x.NumSequencia,
                    (t1, t2) => t1.NumSequencia == t2.NumSequencia,
                    x => x.MFD, x => x.IdDisciplina,
                    x => x.Disciplina,
                    x => x.Resultado, x => x.Observacao,
                    mainScript,
                    classes, subClass);
```
### 
