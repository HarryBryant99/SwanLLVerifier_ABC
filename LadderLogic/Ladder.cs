using SwanLLVerifier.AIG;

namespace SwanLLVerifier.LadderLogic
{
    public class Ladder
    {
        public List<Rung> Rungs { get; set; }

        public Ladder()
        {
            Rungs = new List<Rung>();
        }

        public void AddRung(Rung rung)
        {
            Rungs.Add(rung);
        }

        public ISet<string> AllVariables()
        {
            HashSet<string> allVariables = new();

            foreach (Rung rung in Rungs)
            {
                allVariables.UnionWith(rung.AllVariables());

                _ = allVariables.Add(rung.output);
            }


            return allVariables;
        }

        // public ISet<string> AllInputs()
        // {
        //     HashSet<string> allVariables = new();

        //     // firstly get all variables from all rungs
        //     // then remove the outputs from the set of all variables
        //     // this will give us all the inputs

        //     Console.WriteLine("Calculating all input variables...");


        //     foreach (Rung rung in Rungs)
        //     {

        //         foreach (string variable in rung.AllVariables())
        //         {
        //             if (!rung.output.Equals(variable))
        //             {
        //                 _ = allVariables.Add(variable);
        //             }
        //         }


        //     }
        //     // remove all outputs from the set of all variables // There is possibility of some outputs still being in the set
        //     foreach (Rung rung in Rungs)
        //     {
        //         _ = allVariables.Remove(rung.output);
        //     }
        //     return allVariables;
        // }

        public ISet<string> AllOutputVariables()
        {
            HashSet<string> allOutputVariables = new();

            foreach (Rung rung in Rungs)
                _ = allOutputVariables.Add(rung.output);

            return allOutputVariables;
        }

        // all inputs should just be variables that are not outputs
        public ISet<string> AllInputs()
        {
            ISet<string> allVariables = AllVariables();
            ISet<string> allOutputVariables = AllOutputVariables();
            HashSet<string> inputVariables = new();

            HashSet<string> formattedOutputVariables = new();

            foreach (string var in allOutputVariables)
            {
                string formattedVar = AigConstructor.FormatVarName(var);

                _ = formattedOutputVariables.Add(formattedVar);
            }

            foreach (string var in allVariables)
            {
                if (!formattedOutputVariables.Contains(AigConstructor.FormatVarName(var)))
                {
                    _ = inputVariables.Add(var);
                }
            }

            return inputVariables;
        }

    }


}
