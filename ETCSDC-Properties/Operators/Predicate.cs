namespace SwanLLVerifier.ETCSDC_Properties.Operators
{
    [System.Serializable()]
    [System.Xml.Serialization.XmlRootAttribute("Predicate", IsNullable = false, Namespace = Constants.NAMESPACE)]
    public class Predicate : AbstractFirstOrderFormula
    {
        public Predicate() { }
        
        /// Property to get and set the name of the Predicate.
        [System.Xml.Serialization.XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        /// Property to get and set the terms passed as arguements to the predicate.
        [System.Xml.Serialization.XmlElement("Term", typeof(SwanLLVerifier.ETCSDC_Properties.Term), Namespace = Constants.NAMESPACE)]
        public SwanLLVerifier.ETCSDC_Properties.Term[] Term { get; set; } = Array.Empty<Term>();        
    }
}
