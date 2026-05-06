namespace WordleGame.Exceptions
{
    internal class InvalidGuessException : Exception
    {
        public InvalidGuessException(string msg) : base(msg)
        {

        }
    }
}