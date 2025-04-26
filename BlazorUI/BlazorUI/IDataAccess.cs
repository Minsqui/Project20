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
        /// Returns dictionary of all loaded characters, where their filename is the key.
        /// </summary>
        /// <returns>Dictionary of all loaded characters, where their name is the key.</returns>
        public Dictionary<string, Character> GetCharacterDictionary();
    }
}
