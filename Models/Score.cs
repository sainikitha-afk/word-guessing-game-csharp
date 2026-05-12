namespace WordleGame.Models
{
    internal class Score
    {
        public int ScoreId { get; set; }
        public int UserId { get; set; }
        public int ScoreValue { get; set; }
        public string Difficulty { get; set; }
        public DateTime PlayedOn { get; set; }

        public Score(int userId, int scoreValue, string difficulty)
        {
            UserId = userId;
            ScoreValue = scoreValue;
            Difficulty = difficulty;
            PlayedOn = DateTime.UtcNow;
        }
    }
}
