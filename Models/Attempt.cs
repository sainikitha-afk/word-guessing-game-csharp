using System;

namespace WordleGame.Models
{
    /// <summary>
    /// Represents a single attempt made by the player in the Wordle game.
    /// This class stores the word guessed, the feedback result, and the attempt number.
    /// </summary>
    internal class Attempt
    {
        /// <summary>
        /// The word that the player guessed in this attempt.
        /// For example, "apple" or "grape".
        /// </summary>
        public string Word { get; set; }

        /// <summary>
        /// The feedback result for this guess, using G/Y/X notation:
        /// - G: Correct letter in the correct position
        /// - Y: Correct letter in the wrong position
        /// - X: Letter not present in the hidden word
        /// Example: "GYXXG" for a 5-letter word.
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// The number of this attempt, starting from 1.
        /// For example, 1 for the first guess, 2 for the second, etc.
        /// </summary>
        public int AttemptNo { get; set; }

        /// <summary>
        /// Creates a new Attempt with the specified word, result, and attempt number.
        /// </summary>
        /// <param name="word">The guessed word.</param>
        /// <param name="result">The feedback result string.</param>
        /// <param name="no">The attempt number.</param>
        public Attempt(string word, string result, int no)
        {
            Word = word;
            Result = result;
            AttemptNo = no;
        }
    }
}
