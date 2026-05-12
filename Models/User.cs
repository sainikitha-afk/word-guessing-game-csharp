namespace WordleGame.Models
{
    internal class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public User(int userId, string username, string password)
        {
            UserId = userId;
            Username = username;
            Password = password;
        }
    }
}
