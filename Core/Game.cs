using System;
using System.Collections.Generic;
using WordleGame.Database;
using WordleGame.Exceptions;
using WordleGame.Models;

namespace WordleGame.Core
{
    internal class Game
    {
        private readonly WordProvider wordProvider;
        private readonly GuessValidator validator;
        private readonly FeedbackGenerator feedbackGenerator;
        private readonly GameRepository repository;

        public Game()
        {
            repository = new GameRepository();
            wordProvider = new WordProvider(repository);
            validator = new GuessValidator();
            feedbackGenerator = new FeedbackGenerator();
        }

        public void Start(User currentUser)
        {
            bool playAgain = true;

            while (playAgain)
            {
                Console.Clear();

                string difficulty =
                    SelectDifficulty();

                int maxAttempts =
                    difficulty == "Hard" ? 4 : 6;

                string hiddenWord =
                    wordProvider.ChooseWord();

                List<string> previousGuesses =
                    new List<string>();

                bool guessedCorrectly = false;

                Console.ForegroundColor =
                    ConsoleColor.Cyan;

                Console.WriteLine("WORD GUESSING GAME");

                Console.ResetColor();

                Console.WriteLine(
                    $"Player: {currentUser.Username} | Difficulty: {difficulty}");

                for (int attempt = 1;
                     attempt <= maxAttempts;
                     attempt++)
                {
                    Console.WriteLine();
                    Console.Write(
                        $"Attempt {attempt}/{maxAttempts} : ");

                    string userInput =
                        Console.ReadLine()?.Trim().ToUpper() ?? "";

                    try
                    {
                        validator.CheckGuess(
                            userInput,
                            previousGuesses);

                        previousGuesses.Add(userInput);

                        string feedback =
                            feedbackGenerator.ValidateLetters(
                                hiddenWord,
                                userInput);

                        Console.WriteLine();

                        feedbackGenerator
                            .DisplayColoredFeedback(
                                userInput,
                                feedback);

                        if (feedback == "GGGGG")
                        {
                            guessedCorrectly = true;

                            Console.WriteLine();

                            DisplayWinningComment(attempt);

                            int score =
                                CalculateScore(attempt);

                            Console.ForegroundColor =
                                ConsoleColor.Green;

                            Console.WriteLine(
                                $"Your score : {score}");

                            Console.ResetColor();

                            repository.SaveScore(
                                new Score(currentUser.UserId, score, difficulty));

                            Console.ForegroundColor =
                                ConsoleColor.DarkGreen;

                            Console.WriteLine(
                                "Score saved to the database.");

                            Console.ResetColor();

                            break;
                        }
                    }
                    catch (InvalidGuessException ex)
                    {
                        Console.ForegroundColor =
                            ConsoleColor.Red;

                        Console.WriteLine(ex.Message);

                        Console.ResetColor();

                        attempt--;
                    }
                }

                if (!guessedCorrectly)
                {
                    Console.WriteLine();

                    Console.ForegroundColor =
                        ConsoleColor.Red;

                    Console.WriteLine(
                        "Better luck next time!");

                    Console.WriteLine(
                        $"Hidden word was : {hiddenWord}");

                    Console.ResetColor();
                }

                Console.WriteLine();

                Console.ForegroundColor =
                    ConsoleColor.Yellow;

                Console.Write(
                    "Would you like to play again? (Y/N): ");

                Console.ResetColor();

                string choice =
                    Console.ReadLine()?.Trim().ToUpper() ?? "";

                playAgain = choice == "Y";
            }

            Console.WriteLine();
            Console.ForegroundColor =
                ConsoleColor.Cyan;

            Console.WriteLine(
                "Thank you for playing. See you next time!");

            Console.ResetColor();
        }

        private string SelectDifficulty()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Choose difficulty mode:");

                Console.WriteLine(
                    "1. Easy (6 attempts)");

                Console.WriteLine(
                    "2. Hard (4 attempts)");

                Console.Write("Select mode: ");

                string choice =
                    Console.ReadLine() ?? "";

                if (choice == "1")
                {
                    Console.ForegroundColor =
                        ConsoleColor.Green;

                    Console.WriteLine(
                        "Easy mode selected. You will have 6 attempts.");

                    Console.ResetColor();

                    return "Easy";
                }

                if (choice == "2")
                {
                    Console.ForegroundColor =
                        ConsoleColor.Yellow;

                    Console.WriteLine(
                        "Hard mode selected. You will have 4 attempts.");

                    Console.ResetColor();

                    return "Hard";
                }

                Console.ForegroundColor =
                    ConsoleColor.Red;

                Console.WriteLine(
                    "Invalid option. Please select 1 or 2.");

                Console.ResetColor();
            }
        }

        private int CalculateScore(int attempt)
        {
            return attempt switch
            {
                1 => 100,
                2 => 80,
                3 => 60,
                4 => 40,
                5 => 20,
                _ => 0
            };
        }

        private void DisplayWinningComment(int attempt)
        {
            Console.ForegroundColor =
                ConsoleColor.Green;

            switch (attempt)
            {
                case 1:
                    Console.WriteLine("Genius!");
                    break;

                case 2:
                    Console.WriteLine("Excellent!");
                    break;

                case 3:
                    Console.WriteLine("Great job!");
                    break;

                case 4:
                    Console.WriteLine("Good work!");
                    break;

                case 5:
                    Console.WriteLine("Nice try!");
                    break;

                default:
                    Console.WriteLine("That was close!");
                    break;
            }

            Console.ResetColor();
        }
    }
}