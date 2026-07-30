using SwanLLVerifier.ETCSDC_Properties.Operators;

namespace SwanLLVerifier.ETCSDC_Properties.OperatorTypes
{
    public abstract class BinaryOperatorType : AbstractFirstOrderFormula
    {
        public BinaryOperatorType() { }

        [System.Xml.Serialization.XmlElement("Negation", typeof(Negation), Namespace = Constants.NAMESPACE)]
        [System.Xml.Serialization.XmlElement("And", typeof(And), Namespace = Constants.NAMESPACE)]
        [System.Xml.Serialization.XmlElement("Or", typeof(Or), Namespace = Constants.NAMESPACE)]
        [System.Xml.Serialization.XmlElement("Implies", typeof(Implies), Namespace = Constants.NAMESPACE)]
        [System.Xml.Serialization.XmlElement("Predicate", typeof(Predicate), Namespace = Constants.NAMESPACE)]
        [System.Xml.Serialization.XmlElement("Equivalent", typeof(Equivalent), Namespace = Constants.NAMESPACE)]
        [System.Xml.Serialization.XmlElement("Brackets", typeof(Brackets), Namespace = Constants.NAMESPACE)]
        [System.Xml.Serialization.XmlChoiceIdentifier("OperandTypes")]
		/// Property to get and set the operands of the expression as an array.
		public AbstractFirstOrderFormula[] Operands { get; set; } = null!;

        /// Property to get and set the left operand of the expression.
        public AbstractFirstOrderFormula LeftOperand
        {
            get { return Operands[0]; }
            set { Operands[0] = value; }
        }

        /// Property to get and set the right operand of the expression.
        public AbstractFirstOrderFormula RightOperand
        {
            get { return Operands[1]; }
            set { Operands[1] = value; }
        }

        /// Property to get and set the types of the operands
        public AbstractFirstOrderFormula.FOLFormulaType[] OperandTypes
        {
            get { return new FOLFormulaType[] { Operands[0].FormulaType, Operands[1].FormulaType }; }
            set
            {
                Operands[0].FormulaType = value[0];
                Operands[1].FormulaType = value[1];
            }
        }

    }
}
