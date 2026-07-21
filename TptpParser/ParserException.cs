namespace SwanLLVerifier.TptpParser
{
    public class ParserException : System.Exception
    {
        public int CharacterPosition { get; }

        public ParserException(int characterPosition, string message) : base(message)
        {
            CharacterPosition = characterPosition;
        }

        public override string ToString()
        {
            return $"ParserException @ position {CharacterPosition}: {Message}";
        }
    }
}

