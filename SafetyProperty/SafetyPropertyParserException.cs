namespace SwanLLVerifier.SafetyProperty
{
    public class SafetyPropertyParserException : Exception
    {
        private readonly int characterNumber;

        public SafetyPropertyParserException(int characterNumber, string message)
            : base($"Safety Property Parse Error at character {characterNumber}: {message}")
        {
            this.characterNumber = characterNumber;
        }
    }
}
