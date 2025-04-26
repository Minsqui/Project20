using Core;

namespace BlazorUI
{
    public interface IDataAccess
    {
        /// <summary>
        /// Loads character JSONs into DataAccess.
        /// </summary>
        public void LoadCharacters();

        /// <summary>
        /// Saves character into JSON.
        /// </summary>
        /// <param name="character">Saved character.</param>
        public void SaveCharacter(Character character);

        public Dictionary<string, Character> GetCharacterDictionary();
    }
}
