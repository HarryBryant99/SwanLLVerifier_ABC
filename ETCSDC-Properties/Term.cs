namespace SwanLLVerifier.ETCSDC_Properties
{
    /// Enumeration representing the type of the term.
    [System.Xml.Serialization.XmlType(Namespace = Constants.NAMESPACE, IncludeInSchema = false)]
    public enum TermType
    {
        Constant,
        Var,
        Function,
    }

    /// Class representing a term.
    [System.Serializable()]
    [System.Xml.Serialization.XmlRoot("Term", IsNullable = false, Namespace = Constants.NAMESPACE)]
    public class Term
    {
        public Term() { }

        /// Property to get and set the value of the term.
        [System.Xml.Serialization.XmlElement("Var", typeof(string), Namespace = Constants.NAMESPACE)]
        [System.Xml.Serialization.XmlElement("Constant", typeof(string), Namespace = Constants.NAMESPACE)]
        [System.Xml.Serialization.XmlChoiceIdentifier("TypeValue")]
        public object Value
        {
            get;
            set;
        } = null!;

        /// Property to get and set the type of the term
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public TermType TypeValue
        {
            get;
            set;
        }
    }
}
