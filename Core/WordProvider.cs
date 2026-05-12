using WordleGame.Database;

namespace WordleGame.Core
{
    internal class WordProvider
    {
        private readonly GameRepository repository;

        public WordProvider(GameRepository repository)
        {
            this.repository = repository;
        }

        public string ChooseWord()
        {
            return repository.GetRandomWord();
        }
    }
}
