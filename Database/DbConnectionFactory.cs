using Npgsql;

namespace WordleGame.Database
{
    internal static class DbConnectionFactory
    {
        private static readonly string connectionString = Environment.GetEnvironmentVariable("WORDLE_GAME_CONNECTION")
            ?? "Host=localhost;Port=5432;Username=postgres;Password=postgre;Database=wordle_game";

        public static NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}
