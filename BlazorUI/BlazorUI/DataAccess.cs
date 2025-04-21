using Project20;

namespace BlazorUI
{
    public class DataAccess
    {
        public Dictionary<string, Character> characters;
        public Dictionary<string, GameClass> classes;
        public Dictionary<string, GameRace> races;

        private const string CHARACTERPATH = ".\\data\\characters";
        private const string CLASSESPATH = ".\\data\\classes";
        private const string RACESPATH = ".\\data\\races";
        public DataAccess()
        {
            LoadCharacters();
            classes = JSONManager.LoadClasses(CLASSESPATH);
            races = JSONManager.LoadRaces(RACESPATH);

        }

        /// <summary>
        /// Converts list of characters to dictionary of characters, where the character's name is the key.
        /// </summary>
        /// <param name="characters">The list of characters to be converted.</param>
        /// <returns>Dictionary of characters, where character's name is the key.</returns>
        private Dictionary<string, Character> CharactersListToDict(List<Character> characters)
        {
            Dictionary<string, Character> result = new();

            foreach (var character in characters)
            {
                result[character.Name] = character;
            }

            return result;
        }

        /// <summary>
        /// Loads character JSONs into DataAccess.
        /// </summary>
        public void LoadCharacters()
        {
            characters = CharactersListToDict(JSONManager.LoadCharacters(CHARACTERPATH));
        }
    }
}
