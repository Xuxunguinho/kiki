
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
         
## kiki Script and Sample Usage
As I explained before kiki uses the scripting language [Lizzie](https://github.com/polterguy/lizzie "Lizzie") as, so its syntax is based on Lizzie. so far we have also added the systaxis language for portuguese.

As we will show below, the main method uses script coming from the sorted data subsets (they are used to create the subsets) and also the main script (runs the evaluation as a whole)

For better understanding, below I will present practical examples and I will also leave the Link to the Demo repository in question.

***Note***: *for the example, let's consider **a software development company that is hiring Developers**,
these developers are evaluated based on some areas of knowledge such as: programming language skills, platform skills, and framework skills.*

### The generic object that we are going to use for the example
***Note***: *All **property fields** of our generic **object** can be used within our script to return their values... you can see this in the examples below*
``` csharp
public class  SoftwareDeveloperJobApplication
        {
            public int  CandidateId { get; set; }           
            public string CandidateName { get; set; }
           
            public int SubjectId { get; set; }            
            public string SubjectName { get; set; }          
            
            public int SubjectCategoryId{ get; set; }
            public string SubjectCategoryName{ get; set; }
            
            public int ContributionsOnGithub { get; set; } = 0;
            public double  TestGrade { get; set; }
            
            public bool Helped { get; set; } = false;
            public string Obs { get; set; } = string.Empty;
            public string Result { get; set; }
        }
}
```

### Sorted data subsets 
- ***collectionClass*** (main method parameters)
``` csharp
        private static readonly Dictionary<string, string> SortedSubsets = new()
        {
            //considering negative grades all those 'lower than' 8 values
            {"negativeGrade", "{lt(TestGrade, 8)}"},
            //considering positive grades all those 'greater than'10 values
            {"positiveGrade", "{mte(TestGrade, 10)}"},
            //considering deficient grades all those 'greater than or equal' to 8 and 'less than' 10 values
            {"deficiencyGrade", "{&(mte(TestGrade,8),lt(TestGrade,10))}"}
        };
```
The dictionary **key** represents the subset name and its **value** must be a Lizzie script as shown above.
The expression ```{lt(TestGrade, 8)}``` represents the condition similar to the **Where clause in Linq or SQL**. all subsets of data must be represented by a similar script as in the code example above.
 
- ***collectionSubclasses*** (main method parameters)
``` csharp
        private static readonly Dictionary<string, string> AuxilarSortedSubsets = new()
        {
            //considering 'Csharp' and 'Javascript' as the most important subjects
            //(I will use it in the script to help me find candidates who have
            //no negative or deficiency in this subject)
            {"mostImportant", "{or(eq(SubjectName,'Csharp'),eq(SubjectName,'Javascript'))}"},

        };
```
### Main Script
It is in the main script where we write all the evaluation rule

``` csharp
if(lt(sumT(ContributionsOnGithub),1000), {$R->('Failed GC')},
            { 
                if(&(eq(count(negativeGrades),0),eq(count(deficiencyGrades),0)),
                {
                        if(Helped,{$R->('Passed H')},{$R->('Passed')})
              },
             {
                   if(&(eq(count(negativeGrades),0), lte(count(deficiencyGrades),2),
                   !=> (deficiencyGrades,mostImportants,SubjectId)),
                            {
                              $R->('Recovery')
                            }
                           ,{
                               $R->('Failed')
                            })
                })
            })
```
###### Understanding Main Script

```if(lt(sumT(ContributionsOnGithub),1000)``` -> it is saying that if the **sum of all contributions** (```sumT(ContributionsOnGithub)```) of the candidate on github is less than (```lt```) 1000,
then( ```{$R->('Failed GC')},```) this candidate is not approved.

{$R->( )} -> assigns value to result field - can be of any type

```if(&(eq(count(negativeGrades),0),eq(count(deficiencyGrades),0))``` -> is saying that if the number of negative grades (```count(negativeGrades)```) is equal (```eq```) to 0 and the number of deficient grades (```count(deficiencyGrades)```) is equal (```eq```) to 0.

```!=> (deficiencyGrades,mostImportants,SubjectId))``` -> checking that the **deficiencyGrades** subset does not contain any of the items found in the **mostImportants** subset

###### Understanding main script in a simple way

In nested conditions the first condition is imperative so it appears first and the others inside it (The logic will depend on what you need). This was written to serve my purpose.

what the script wants to tell us is that: if the candidate does not have contributions on github greater than 1000, then he is soon rejected by the company, as he does not meet this main requirement. if he has contributions greater than 1000 , then checks if the candidate has conditions to be admitted to the company imposing that he has no negative grades or grades considered as a deficiency, if he meets the condition, checks if the candidate was helped and assigns the result based on in the help he received, and if he does not meet this condition, then he checks if the candidate has the conditions to receive a new chance to retake the tests in which the grades are considered as deficiencies, imposing that the candidate does not have negative grades and that he has at most two deficiencies whose subjects are not among the most important ones. otherwise, the candidate is rejected.



