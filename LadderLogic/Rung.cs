using SwanLLVerifier.PropositionalLogic;
using SwanLLVerifier.ETCSDC_Properties;

namespace SwanLLVerifier.LadderLogic
{
    public class Rung
    {
        public string output { get; set; }

        public AbstractFirstOrderFormula formula { get; set; }

        public bool? Initialised { get; set; }

        public ISet<string> AllVariables()
        {
            ISet<string> allVariables = PropositionalFormulaUtils.AllVariablesFromFormula(formula);
            _ = allVariables.Add(output);

            return allVariables;
        }
    }
}
