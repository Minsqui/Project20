using Core;
using System.Xml.Schema;

namespace BlazorUI
{
    public class DataAccessJSON : IDataAccess
    {
        private JSONManager _dataManager;

        private Dictionary<string, Character> _characters = new();
        private Dictionary<string, GameClass> _classes = new();
        private Dictionary<string, GameRace> _races = new();

        private const string CHARACTERPATH = ".\\data\\characters";
        private const string CLASSESPATH = ".\\data\\classes";
        private const string RACESPATH = ".\\data\\races";
        public DataAccessJSON()
        {
            _dataManager = new();
            LoadCharacters();
            _classes = JSONManager.LoadClasses(CLASSESPATH);
            _races = JSONManager.LoadRaces(RACESPATH);

        }

        /// <summary>
        /// Deletes character and it's JSON.
        /// </summary>
        /// <param name="character"></param>
        public void DeleteCharacter(Character character)
        {
            _dataManager.DeleteCharacter(character, CHARACTERPATH);
        }

        /// <summary>
        /// Returns dictionary of characters, where keys are their filenames.
        /// </summary>
        /// <returns>Dictionary of characters, where keys are their filenames.</returns>
        public Dictionary<string, Character> GetCharacterDictionary()
        {
            return _characters;
        }

        /// <summary>
        /// Creates new character, adds it to inner dictionary, and saves them in JSON.
        /// </summary>
        public Character NewCharacter()
        {
            Character newCharacter = _dataManager.NewCharacter(CHARACTERPATH);
            return newCharacter;
        }

        /// <summary>
        /// Loads character JSONs into DataAccess.
        /// </summary>
        public void LoadCharacters()
        {
            _characters = _dataManager.LoadCharacters(CHARACTERPATH);
        }

        /// <summary>
        /// Saves character into JSON.
        /// </summary>
        /// <param name="character">Saved character.</param>
        public void SaveCharacter(Character character)
        {
            _dataManager.SaveCharacter(character, CHARACTERPATH);
        }

        /// <summary>
        /// Returns all loaded game classes.
        /// </summary>
        /// <returns>All loaded game classes.</returns>
        public GameClass[] GetAllGameClasses()
        {
            return _classes.Select(x => x.Value).ToArray();
        }

        /// <summary>
        /// Returns character's class.
        /// </summary>
        /// <param name="character">Given character.</param>
        /// <returns></returns>
        public GameClass? GetCharacterGameClass(Character character)
        {
            if (character.ClassID is null) return null;

            return GetGameClass(character.ClassID);
        }

        /// <summary>
        /// Returns class with given id.
        /// </summary>
        /// <param name="id">ID of the class</param>
        /// <returns>Class with given id. Returns null if no race found.</returns>
        public GameClass? GetGameClass(string id)
        {
            if (_classes.ContainsKey(id)) return _classes[id];
            else return null;
        }

        /// <summary>
        /// Returns all loaded game races.
        /// </summary>
        /// <returns>All loaded game races.</returns>
        public GameRace[] GetAllGameRaces()
        {
            return _races.Select(x => x.Value).ToArray();
        }

        /// <summary>
        /// Returns character's race.
        /// </summary>
        /// <param name="character">Given character.</param>>
        /// <returns>Character's race. Returns null if no race found.</returns>
        public GameRace? GetCharacterGameRace(Character character)
        {
            if (character.RaceID is null) return null;

            return GetGameRace(character.RaceID);
        }

        /// <summary>
        /// Returns race with given id.
        /// </summary>
        /// <param name="id">ID of the race.</param>
        /// <returns>Race with given id. Returns null if no race found.</returns>
        public GameRace? GetGameRace(string id)
        {
            if (_races.ContainsKey(id)) return _races[id];
            else return null;
        }
    }
}
