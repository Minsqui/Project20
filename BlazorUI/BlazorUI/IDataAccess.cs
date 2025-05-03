using Core;

namespace BlazorUI
{
    public interface IDataAccess
    {
        /// <summary>
        /// Deletes character.
        /// </summary>
        public void DeleteCharacter(Character character);

        /// <summary>
        /// Loads characters.
        /// </summary>
        public void LoadCharacters();

        /// <summary>
        /// Creates a new character.
        /// </summary>
        /// <returns></returns>
        public Character NewCharacter();

        /// <summary>
        /// Saves character.
        /// </summary>
        /// <param name="character">Saved character.</param>
        public void SaveCharacter(Character character);

        /// <summary>
        /// Returns array of all loaded character IDs.
        /// </summary>
        /// <returns>Array of all loaded character IDs.</returns>
        public string[] GetAllCharactersIDs();

        /// <summary>
        /// Returns character with given ID.
        /// </summary>
        /// <param name="characterID">ID of the character.</param>
        /// <returns>Character with given ID. Null if no character with given ID was found.</returns>
        public Character? GetCharacter(string characterID);

        /// <summary>
        /// Returns all loaded game classes.
        /// </summary>
        /// <returns>All loaded game classes.</returns>
        public GameClass[] GetAllGameClasses();

        /// <summary>
        /// Returns character's class.
        /// </summary>
        /// <param name="character">Given character.</param>
        /// <returns></returns>
        public GameClass? GetCharacterGameClass(Character character);

        /// <summary>
        /// Returns class with given id.
        /// </summary>
        /// <param name="id">ID of the class</param>
        /// <returns>Class with given id. Returns null if no race found.</returns>
        public GameClass? GetGameClass(string id);

        /// <summary>
        /// Returns all loaded game races.
        /// </summary>
        /// <returns>All loaded game races.</returns>
        public GameRace[] GetAllGameRaces();

        /// <summary>
        /// Returns character's race.
        /// </summary>
        /// <param name="character">Given character.</param>>
        /// <returns>Character's race. Returns null if no race found.</returns>
        public GameRace? GetCharacterGameRace(Character character);

        /// <summary>
        /// Returns race with given id.
        /// </summary>
        /// <param name="id">ID of the race.</param>
        /// <returns>Race with given id. Returns null if no race found.</returns>
        public GameRace? GetGameRace(string id);
    }
}
