namespace WordleGame.Core
{
    internal class WordProvider
    {
        // list collection to store all the hidden words
        private List<string> hiddenWords;
        
        // random variable to choose the hidden word randomly
        private Random rnd;
        
        public WordProvider()
        {
            rnd = new Random();

            // hardcoded the words to be guessed
            hiddenWords = new List<string>()
            {
                "APPLE",
                "MANGO",
                "GRAPE",
                "TRAIN",
                "PLANT",
                "BRAIN",
                "STONE",
                "SWEET",
                "LIGHT",
                "SMILE"
            };
        }

        public string ChooseWord()
        {
            int pos = rnd.Next(hiddenWords.Count);

            return hiddenWords[pos];
        }
    }
}