using SwanLLVerifier.ETCSDC_Properties.OperatorTypes;

namespace SwanLLVerifier.ETCSDC_Properties.Operators
{
    public class Or : BinaryOperatorType
    {
        public Or() { }

        public Or(List<AbstractFirstOrderFormula> operandFormulae)
        {
            this.Operands = operandFormulae.ToArray();
        }
    }
}
