using SwanLLVerifier.PropositionalLogic;
using SwanLLVerifier.ETCSDC_Properties;

namespace SwanLLVerifier.LadderLogic
{
    public class Rung
    {
        public string Output { get; set; } = string.Empty;

        public AbstractFirstOrderFormula Formula { get; set; } = null!;

        public bool? Initialised { get; set; }

        public ISet<string> AllVariables()
        {
            ISet<string> allVariables = PropositionalFormulaUtils.AllVariablesFromFormula(Formula);
            _ = allVariables.Add(Output);

            return allVariables;
        }
    }
}
