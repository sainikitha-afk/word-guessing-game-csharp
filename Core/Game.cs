using WordleGame.Exceptions;

namespace WordleGame.Core
{
    internal class Game
    {
        private WordProvider words;
        private GuessValidator guess;
        private FeedbackGenerator feed;

        private string hiddenWord;

        private int attemptCount;

        private List<string> prevGuesses;

        private int max; // maximum number of attempts allowed
        private int sco = 0; // score of the player

        public Game(int tries)
        {
            max = tries;
            words = new WordProvider();
            guess = new GuessValidator();
            feed = new FeedbackGenerator();

            prevGuesses = new List<string>();

            hiddenWord = words.ChooseWord();

            attemptCount = 0;
        }

        public void Start()
        {
            Console.WriteLine("WORD GUESSING GAME");
            Console.WriteLine();

            while (attemptCount < max)
            {
                Console.Write($"Attempt {attemptCount + 1}/{max} : ");  

                string userInput = Console.ReadLine()!;

                try
                {
                    guess.CheckGuess(userInput, prevGuesses);

                    userInput = userInput.ToUpper();

                    prevGuesses.Add(userInput);

                    attemptCount++;

                    string res = feed.ValidateLetters(hiddenWord, userInput);

                    Feedback(userInput, res);

                    // player guessed correctly
                    if (res == "GGGGG")
                    {
                        Win();
                        return;
                    }
                }
                catch (InvalidGuessException exc)
                {
                    Console.WriteLine(exc.Message);
                    Console.WriteLine();
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Hidden word was : {hiddenWord}");
            Console.WriteLine("Better luck next time :(");
        }

        private void Feedback(string word, string res)
        {
            Console.WriteLine();

            for (int i = 0; i < word.Length; i++)
            {
                Console.Write(word[i] + " ");
            }

            Console.WriteLine();

            for (int i = 0; i < res.Length; i++)
            {
                if (res[i] == 'G')
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else if (res[i] == 'Y')
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }

                Console.Write(res[i] + " ");

                Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        private void Win()
        {
            Console.WriteLine();

            if (attemptCount == 1)
            {
                sco = 100;
                Console.WriteLine("Genius!");
            }
            else if (attemptCount == 2)
            {
                sco = 80;
                Console.WriteLine("Excellent!");
            }
            else if (attemptCount == 3)
            {
                sco = 60;
                Console.WriteLine("Great job!");
            }
            else if (attemptCount == 4)
            {
                sco = 40;
                Console.WriteLine("Good work!");
            }
            else if (attemptCount == 5)
            {
                sco = 20;
                Console.WriteLine("Nice try!");
            }
            else
            {
                sco = 0;
                Console.WriteLine("That was close!");
            }
            Console.WriteLine($"Your score : {sco}");
        }
    }
}