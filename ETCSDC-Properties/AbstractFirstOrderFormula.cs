using System.Collections.Generic;
using System.Linq;
using SwanLLVerifier.ETCSDC_Properties.Operators;
using SwanLLVerifier.ETCSDC_Properties.OperatorTypes;

namespace SwanLLVerifier.ETCSDC_Properties
{
    public abstract class AbstractFirstOrderFormula
    {
        public AbstractFirstOrderFormula() { }

        /// Enum capturing the type of the first order formula
        public enum FOLFormulaType
        {
            And,
            Or,
            Implies,
            Negation,
            Equivalent,
            Brackets,
            Predicate,
        }

        /// Property to access the first order formula type.
        public FOLFormulaType FormulaType { get; set; }

        /// <summary>
        /// Retrieves all variables present in the formula via recursive traversal.
        /// For Predicate types, extracts predicate names as variables.
        /// For operators, recursively collects variables from operands.
        /// </summary>
        /// <returns>A HashSet of unique variable names (predicate names)</returns>
        public HashSet<string> GetAllVariables()
        {
            var variables = new HashSet<string>();

            switch (this.FormulaType)
            {
                case FOLFormulaType.Predicate:
                    // For Predicate nodes, the Name property represents the variable
                    if (this is Predicate predicate && !string.IsNullOrEmpty(predicate.Name))
                    {
                        variables.Add(predicate.Name);
                    }
                    break;

                case FOLFormulaType.Negation:
                case FOLFormulaType.Brackets:
                    // Unary operators: recursively collect from their single operand
                    if (this is UnaryOperatorType unaryOp && unaryOp.Operand != null)
                    {
                        var operandVariables = unaryOp.Operand.GetAllVariables();
                        foreach (var variable in operandVariables)
                        {
                            variables.Add(variable);
                        }
                    }
                    break;

                case FOLFormulaType.And:
                case FOLFormulaType.Or:
                case FOLFormulaType.Implies:
                case FOLFormulaType.Equivalent:
                    // Binary operators: recursively collect from both operands
                    if (this is BinaryOperatorType binaryOp && binaryOp.Operands != null)
                    {
                        foreach (var operand in binaryOp.Operands)
                        {
                            if (operand != null)
                            {
                                var operandVariables = operand.GetAllVariables();
                                foreach (var variable in operandVariables)
                                {
                                    variables.Add(variable);
                                }
                            }
                        }
                    }
                    break;
            }

            return variables;
        }

        /// <summary>
        /// Retrieves all variables as a sorted list for consistent ordering.
        /// </summary>
        /// <returns>A sorted list of unique variable names</returns>
        public List<string> GetAllVariablesSorted()
        {
            return this.GetAllVariables().OrderBy(v => v).ToList();
        }
    }
}
