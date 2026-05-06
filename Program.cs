using WordleGame.Core;

namespace WordleGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Word Guessing Game";

            bool replay = true;

            while (replay)
            {
                Console.Clear();

                Console.WriteLine("WORD GUESSING GAME");
                Console.WriteLine();

                Console.WriteLine("1. Easy");
                Console.WriteLine("2. Hard");
                Console.Write("Choose difficulty : ");

                string mode = Console.ReadLine()!;

                int tries = 6;

                if (mode == "1")
                {
                    Console.WriteLine("You have chosen EASY mode. You get 6 attempts to guess the word.");
                    tries = 6;
                }

                else if (mode == "2")
                {
                    Console.WriteLine("You have chosen HARD mode. You get 4 attempts to guess the word.");
                    tries = 4;
                }

                else  {
                    Console.WriteLine("Invalid choice! Defaulting to EASY mode.");
                    tries = 6;
                }

                // creating object g of class game
                Game g = new Game(tries);
                g.Start();

                Console.WriteLine();
                Console.Write("Wanna play again? (Y/N) : ");

                // to store users choice of y/n
                string ch = Console.ReadLine()!;

                // unified input of user - edge case
                if (ch == "y")
                {
                    ch = "Y";
                }
                else if (ch == "n")
                {
                    ch = "N";
                }

                if (ch != "Y")
                {
                    replay = false;
                }
            }
            
            Console.WriteLine();
            Console.WriteLine("Thank you for playing :)");
            Console.WriteLine("To exit press any key.");

            Console.ReadKey();                
        }
    }
}