using SwanLLVerifier.ETCSDC_Properties.Operators;

namespace SwanLLVerifier.ETCSDC_Properties
{
    static class Constants
    {
        public const string NAMESPACE = "https://sigrad.siemens.cloud/SafetyPropSchema";
    }

    /// Class representing a property. Properties are logical statements about a model which can be checked and verified.
    [System.Serializable()]
    [System.Xml.Serialization.XmlRoot("Property", IsNullable = false, Namespace = Constants.NAMESPACE)]
    public class Property
    {
        public Property() { }

        /// Property to get and set the description of the property.
        [System.Xml.Serialization.XmlElement(Namespace = Constants.NAMESPACE)]
        public string Description { get; set; } = null!;

        /// Property to get and set the name of the property.
        [System.Xml.Serialization.XmlAttribute("name", Namespace = Constants.NAMESPACE)]
        public string Name { get; set; } = null!;

        /// Property to get and set the formula associated with the property.
        [System.Xml.Serialization.XmlElement("Negation", typeof(Negation), Namespace = Constants.NAMESPACE)]
        [System.Xml.Serialization.XmlElement("And", typeof(And), Namespace = Constants.NAMESPACE)]
        [System.Xml.Serialization.XmlElement("Or", typeof(Or), Namespace = Constants.NAMESPACE)]
        [System.Xml.Serialization.XmlElement("Implies", typeof(Implies), Namespace = Constants.NAMESPACE)]
        [System.Xml.Serialization.XmlElement("Predicate", typeof(Predicate), Namespace = Constants.NAMESPACE)]
        [System.Xml.Serialization.XmlElement("Equivalent", typeof(Equivalent), Namespace = Constants.NAMESPACE)]
        [System.Xml.Serialization.XmlElement("Brackets", typeof(Brackets), Namespace = Constants.NAMESPACE)]
        [System.Xml.Serialization.XmlChoiceIdentifier("FormulaType")]
        public SwanLLVerifier.ETCSDC_Properties.AbstractFirstOrderFormula FirstOrderFormula { get; set; }

        /// Property to get and set the type of the formula associated with the property.
        public required AbstractFirstOrderFormula.FOLFormulaType FormulaType
        {
            get { return FirstOrderFormula.FormulaType; }
            set { FirstOrderFormula.FormulaType = value; }
        }

    }
}
