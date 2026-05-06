namespace WordleGame.Core
{
    internal class FeedbackGenerator
    {
        public string ValidateLetters(string hiddenWord, string userInput)
        {
            char[] res = new char[5];

            // tracks letters already matched
            bool[] alreadyChecked = new bool[5];

            for (int i = 0; i < hiddenWord.Length; i++)
            {
                // exact match then 'G'
                if (userInput[i] == hiddenWord[i])
                {
                    res[i] = 'G';
                    alreadyChecked[i] = true;
                }
            }
            //remaining letters
            for (int i = 0; i < hiddenWord.Length; i++)
            {
                if (res[i] == 'G')
                {
                    continue;
                }

                bool found = false;

                for (int j = 0; j < hiddenWord.Length; j++)
                {
                    if (!alreadyChecked[j] && userInput[i] == hiddenWord[j])
                    {
                        found = true;
                        alreadyChecked[j] = true;
                        break;
                    }
                }

                if (found)
                {
                    res[i] = 'Y';
                }
                else
                {
                    res[i] = 'X';
                }
            }

            return new string(res);
        }    
    }
}

