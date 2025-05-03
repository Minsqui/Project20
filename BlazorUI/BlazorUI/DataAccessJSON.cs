using Core;
using System.Xml.Schema;

namespace BlazorUI
{
    public class DataAccessJSON : IDataAccess
    {
        private JSONManager _dataManager;

        private const string CHARACTERPATH = ".\\data\\characters";
        private const string CLASSESPATH = ".\\data\\classes";
        private const string RACESPATH = ".\\data\\races";
        public DataAccessJSON()
        {
            _dataManager = new();
            LoadAll();
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
        /// Returns array of all loaded character IDs.
        /// </summary>
        /// <returns>Array of all loaded character IDs.</returns>
        public string[] GetAllCharactersIDs()
        {
            return _dataManager.GetCharacterIDs();
        }

        /// <summary>
        /// Returns character with given ID.
        /// </summary>
        /// <param name="characterID">ID of the character.</param>
        /// <returns>Character with given ID. Null if no character with given ID was found.</returns>
        public Character? GetCharacter(string characterID)
        {
            return _dataManager.GetCharacter(characterID);
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
        /// Loads characters, classes and races.
        /// </summary>
        public void LoadAll()
        {
            _dataManager.LoadCharacters(CHARACTERPATH);
            _dataManager.LoadClasses(CLASSESPATH);
            _dataManager.LoadRaces(RACESPATH);
        }

        /// <summary>
        /// Loads characters.
        /// </summary>
        public void LoadCharacters()
        {
            _dataManager.LoadCharacters(CHARACTERPATH);
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
            //return _classes.Select(x => x.Value).ToArray();
            return _dataManager.GetAllClasses(); 
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
            return _dataManager.GetClass(id);
        }

        /// <summary>
        /// Returns all loaded game races.
        /// </summary>
        /// <returns>All loaded game races.</returns>
        public GameRace[] GetAllGameRaces()
        {
            return _dataManager.GetAllRaces();
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
            return _dataManager.GetRace(id);
        }
    }
}
