using SwanLLVerifier.ETCSDC_Properties.Operators;

namespace SwanLLVerifier.ETCSDC_Properties.OperatorTypes
{
    public class UnaryOperatorType : AbstractFirstOrderFormula
    {
        public UnaryOperatorType() { }

        [System.Xml.Serialization.XmlElement("Negation", typeof(Negation), Namespace = Constants.NAMESPACE)]
        [System.Xml.Serialization.XmlElement("And", typeof(And), Namespace = Constants.NAMESPACE)]
        [System.Xml.Serialization.XmlElement("Or", typeof(Or), Namespace = Constants.NAMESPACE)]
        [System.Xml.Serialization.XmlElement("Implies", typeof(Implies), Namespace = Constants.NAMESPACE)]
        [System.Xml.Serialization.XmlElement("Predicate", typeof(Predicate), Namespace = Constants.NAMESPACE)]
        [System.Xml.Serialization.XmlElement("Equivalent", typeof(Equivalent), Namespace = Constants.NAMESPACE)]
        [System.Xml.Serialization.XmlElement("Brackets", typeof(Brackets), Namespace = Constants.NAMESPACE)]
        [System.Xml.Serialization.XmlChoiceIdentifier("OperandType")]

        /// Property to get and set the operand on the formula.
        public AbstractFirstOrderFormula Operand { get; set; }

        /// Property to get the operand type for the formula
        public AbstractFirstOrderFormula.FOLFormulaType OperandType
        {
            get { return Operand.FormulaType; }
            set { Operand.FormulaType = value; }
        }
    }
}
