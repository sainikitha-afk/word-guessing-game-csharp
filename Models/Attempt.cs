namespace WordleGame.Models
{
    internal class Attempt
    {
        public string Word { get; set; }

        public string Result { get; set; }

        public int AttemptNo { get; set; }

        public Attempt(string word, string result, int no)
        {
            Word = word;
            Result = result;
            AttemptNo = no;
        }
    }
}
