namespace WordleGame.Models
{
    internal class Word
    {
        public int WordId { get; set; }
        public string WordText { get; set; }

        public Word(int wordId, string wordText)
        {
            WordId = wordId;
            WordText = wordText;
        }
    }
}
