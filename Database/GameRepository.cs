using Npgsql;
using WordleGame.Models;

namespace WordleGame.Database
{
    internal class GameRepository
    {
        public GameRepository()
        {
            InitializeDatabase();
        }

        public void InitializeDatabase()
        {
            using var connection = DbConnectionFactory.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS users
                (
                    user_id SERIAL PRIMARY KEY,
                    username VARCHAR(50) UNIQUE NOT NULL,
                    password VARCHAR(50) NOT NULL
                );

                CREATE TABLE IF NOT EXISTS words
                (
                    word_id SERIAL PRIMARY KEY,
                    word_text VARCHAR(5) NOT NULL
                );

                CREATE TABLE IF NOT EXISTS scores
                (
                    score_id SERIAL PRIMARY KEY,
                    user_id INT REFERENCES users(user_id),
                    score_value INT,
                    difficulty VARCHAR(20),
                    played_on TIMESTAMP
                );";
            command.ExecuteNonQuery();

            SeedWords(connection);
        }

        private void SeedWords(NpgsqlConnection connection)
        {
            using var countCommand = connection.CreateCommand();
            countCommand.CommandText = "SELECT COUNT(*) FROM words;";
            var count = (long)(countCommand.ExecuteScalar() ?? 0L);

            if (count > 0)
            {
                return;
            }

            using var insertCommand = connection.CreateCommand();
            insertCommand.CommandText = @"
                INSERT INTO words (word_text)
                VALUES
                    ('APPLE'),
                    ('MANGO'),
                    ('GRAPE'),
                    ('TRAIN'),
                    ('PLANT'),
                    ('BRAIN'),
                    ('STONE'),
                    ('SWEET'),
                    ('LIGHT'),
                    ('SMILE');";
            insertCommand.ExecuteNonQuery();
        }

        public User? AuthenticateUser(string username, string password)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT user_id, username, password
                FROM users
                WHERE username = @username
                  AND password = @password;";
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            using var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            return new User(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2));
        }

        public bool UsernameExists(string username)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT 1
                FROM users
                WHERE username = @username
                LIMIT 1;";
            command.Parameters.AddWithValue("@username", username);

            using var reader = command.ExecuteReader();
            return reader.Read();
        }

        public User RegisterUser(string username, string password)
        {
            if (UsernameExists(username))
            {
                throw new InvalidOperationException("Username already exists. Please choose a different username.");
            }

            using var connection = DbConnectionFactory.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO users (username, password)
                VALUES (@username, @password)
                RETURNING user_id;";
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            var userId = (int)(command.ExecuteScalar() ?? 0);
            return new User(userId, username, password);
        }

        public string GetRandomWord()
        {
            using var connection = DbConnectionFactory.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT word_text
                FROM words
                ORDER BY RANDOM()
                LIMIT 1;";

            var value = command.ExecuteScalar();
            return value?.ToString() ?? string.Empty;
        }

        public void SaveScore(Score score)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO scores (user_id, score_value, difficulty, played_on)
                VALUES (@userId, @scoreValue, @difficulty, @playedOn);";
            command.Parameters.AddWithValue("@userId", score.UserId);
            command.Parameters.AddWithValue("@scoreValue", score.ScoreValue);
            command.Parameters.AddWithValue("@difficulty", score.Difficulty);
            command.Parameters.AddWithValue("@playedOn", score.PlayedOn);

            command.ExecuteNonQuery();
        }

        public List<Score> GetUserScores(int userId)
        {
            var scores = new List<Score>();

            using var connection = DbConnectionFactory.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT score_id, user_id, score_value, difficulty, played_on
                FROM scores
                WHERE user_id = @userId
                ORDER BY played_on DESC;";
            command.Parameters.AddWithValue("@userId", userId);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var score = new Score(
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetString(3))
                {
                    ScoreId = reader.GetInt32(0),
                    PlayedOn = reader.GetDateTime(4)
                };
                scores.Add(score);
            }

            return scores;
        }
    }
}
