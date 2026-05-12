using WordleGame.Core;
using WordleGame.Database;
using WordleGame.Models;

namespace WordleGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Word Guessing Game";

            var repository = new GameRepository();
            User currentUser = AuthenticatePlayer(repository);

            bool playAgain = true;
            while (playAgain)
            {
                var game = new Game();
                game.Start(currentUser);

                DisplayUserScores(currentUser, repository);

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Would you like to play again? (Y/N): ");
                Console.ResetColor();
                var input = Console.ReadLine()?.Trim().ToUpper() ?? string.Empty;
                playAgain = input == "Y";
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Thank you for playing. See you next time!");
            Console.ResetColor();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static User AuthenticatePlayer(GameRepository repository)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("WORD GUESSING GAME");
                Console.WriteLine();
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option: ");

                var choice = Console.ReadLine()?.Trim();
                Console.WriteLine();

                if (choice == "1")
                {
                    try
                    {
                        return RegisterPlayer(repository);
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex.Message);
                    }
                }
                else if (choice == "2")
                {
                    var user = LoginPlayer(repository);
                    if (user != null)
                    {
                        return user;
                    }
                }
                else if (choice == "3")
                {
                    Environment.Exit(0);
                }
                else
                {
                    ShowError("Invalid option. Please select 1, 2 or 3.");
                }

                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }
        }

        private static User RegisterPlayer(GameRepository repository)
        {
            Console.Write("Enter a username: ");
            var username = Console.ReadLine()?.Trim() ?? string.Empty;
            Console.Write("Enter a password: ");
            var password = Console.ReadLine()?.Trim() ?? string.Empty;

            ValidateCredentials(username, password);
            var user = repository.RegisterUser(username, password);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Registration successful. Welcome, {user.Username}!");
            Console.ResetColor();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            return user;
        }

        private static User? LoginPlayer(GameRepository repository)
        {
            Console.Write("Username: ");
            var username = Console.ReadLine()?.Trim() ?? string.Empty;
            Console.Write("Password: ");
            var password = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ShowError("Both username and password are required.");
                return null;
            }

            var user = repository.AuthenticateUser(username, password);
            if (user == null)
            {
                ShowError("Login failed. Please check your username and password.");
                return null;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Login successful. Hello, {user.Username}!\n");
            Console.ResetColor();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            return user;
        }

        private static void ValidateCredentials(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException("Username and password cannot be empty.");
            }

            if (username.Length < 3)
            {
                throw new InvalidOperationException("Username must be at least 3 characters.");
            }

            if (password.Length < 3)
            {
                throw new InvalidOperationException("Password must be at least 3 characters.");
            }
        }

        private static string GetDifficultyMode()
        {
            while (true)
            {
                Console.WriteLine("Choose difficulty mode:");
                Console.WriteLine("1. Easy (6 attempts)");
                Console.WriteLine("2. Hard (4 attempts)");
                Console.Write("Select mode: ");

                var input = Console.ReadLine()?.Trim();
                if (input == "1")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Easy mode selected. You will have 6 attempts.");
                    Console.ResetColor();
                    return "Easy";
                }
                else if (input == "2")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Hard mode selected. You will have 4 attempts.");
                    Console.ResetColor();
                    return "Hard";
                }

                ShowError("Invalid choice. Please enter 1 or 2.");
            }
        }

        private static bool AskForReplay()
        {
            Console.Write("Would you like to play again? (Y/N): ");
            var input = Console.ReadLine()?.Trim().ToUpper() ?? string.Empty;
            return input == "Y";
        }

        private static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static void DisplayUserScores(User user, GameRepository repository)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"=== Score History for {user.Username} ===");
            Console.ResetColor();

            var scores = repository.GetUserScores(user.UserId);

            if (scores.Count == 0)
            {
                Console.WriteLine("No scores yet. Start playing!");
                return;
            }

            Console.WriteLine($"\n{"Score",8} {"Difficulty",12} {"Date & Time",25}");
            Console.WriteLine(new string('-', 50));

            foreach (var score in scores)
            {
                var dateStr = score.PlayedOn.ToString("yyyy-MM-dd HH:mm:ss");
                Console.ForegroundColor = score.ScoreValue > 0 ? ConsoleColor.Green : ConsoleColor.Yellow;
                Console.WriteLine($"{score.ScoreValue,8} {score.Difficulty,12} {dateStr,25}");
                Console.ResetColor();
            }

            Console.WriteLine(new string('-', 50));
            int totalScore = scores.Sum(s => s.ScoreValue);
            int gamesPlayed = scores.Count;
            int averageScore = gamesPlayed > 0 ? totalScore / gamesPlayed : 0;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Total Games: {gamesPlayed} | Total Score: {totalScore} | Average: {averageScore}");
            Console.ResetColor();
        }
    }
}
