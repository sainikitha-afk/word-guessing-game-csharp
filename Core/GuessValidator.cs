using WordleGame.Exceptions;

namespace WordleGame.Core
{
    internal class GuessValidator
    {
        public void CheckGuess(string userInput, List<string> prevGuesses)
        {
            // checking empty values first
            if (string.IsNullOrWhiteSpace(userInput))
            {
                throw new InvalidGuessException("Invalid guess! Your input cannot be empty.");
            }

            userInput = userInput.Trim().ToUpper();

            if (userInput.Length < 5)
            {
                throw new InvalidGuessException("Invalid guess! Only 5 letter words are accepted.");
            }

            if (userInput.Length > 5)
            {
                throw new InvalidGuessException("Invalid guess! Only 5 letter words are accepted.");
            }

            // numbers are not allowed
            if (userInput.Any(char.IsDigit))
            {
                throw new InvalidGuessException("Invalid guess! Numbers are not accepted.");
            }

            // symbols should not be accepted
            if (userInput.Any(ch => !char.IsLetter(ch)))
            {
                throw new InvalidGuessException("Invalid guess! It's a word game, hence no symbols :)");
            }

            // same guesses again are not accepted 
            if (prevGuesses.Contains(userInput))
            {
                throw new InvalidGuessException("Invalid guess! You have already guessed that word.");
            }
        }
    }
}