namespace SwanLLVerifier.TptpParser
{
    public class TokeniserException : System.Exception
    {
        private readonly int characterPos;

        public TokeniserException(int characterPos, string message) : base(message)
        {
            this.characterPos = characterPos;
        }

        public override string ToString()
        {
            return $"TokeniserException @ position {characterPos}: {base.Message}";
        }
    }
}