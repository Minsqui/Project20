using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core
{
    public class JSONManager
    {
        private Dictionary<string, Character> _characters = new();

        private const string BASICNAME = "character";

        private const string EXTENSION = ".json";
        private static string GetFilename(string id)
        {
            return $"{id}{EXTENSION}";
        }

        // TODO - Needs checking, also use it as minumum as possible
        private bool TryGetCharacterID(Character character, out string? id)
        {
            if (_characters.ContainsValue(character))
            {
                id = _characters.FirstOrDefault(x => x.Value == character).Key;
                return true;
            }
            else
            {
                id = null;
                return false;
            }
        }

        /// <summary>
        /// Deletes character's JSON.
        /// </summary>
        /// <param name="character">Character that is to be removed.</param>
        /// <param name="path">Path to the character folder.</param>
        public void DeleteCharacter(Character character, string path)
        {
            if (!TryGetCharacterID(character, out string id)) return;

            if (id is null) throw new ApplicationException();

            _characters.Remove(id);

            if (!Directory.Exists(path))
            {
                return;
            }

            string filePath = Path.Combine(path, GetFilename(id));
            File.Delete(filePath);
        }

        /// <summary>
        /// Loads all character JSONs from given path.
        /// <param name="path">Path to the character folder.</param>
        /// </summary>
        public Dictionary<string,Character> LoadCharacters(string path)
        {
            string[] filePaths;

            // Checking if path exists, if not, creating one.
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            filePaths = Directory.GetFiles(path);

            foreach (var filePath in filePaths)
            {
                try
                {
                    using (StreamReader r = new StreamReader(filePath))
                    {
                        string json = r.ReadToEnd();

                        Character? newCharacter = JsonSerializer.Deserialize<Character>(json);

                        if (newCharacter == null) continue;

                        string filename = Path.GetFileName(filePath);
                        string id = filename.Remove(filename.Length - EXTENSION.Length); // Removing extension

                        _characters[id] = (newCharacter);
                    }
                }
                catch
                {
                    continue;
                }
            }

            return _characters;
        }

        /// <summary>
        /// Loads all classes JSONs from given path.
        /// <param name="path">Path to the class folder.</param>>
        /// </summary>
        public static Dictionary<string, GameClass> LoadClasses(string path)
        {
            Dictionary<string, GameClass> classes = new();

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string[] filePaths = Directory.GetFiles(path);

            foreach (var filePath in filePaths)
            {
                try
                {
                    using (StreamReader r = new StreamReader(filePath))
                    {
                        string json = r.ReadToEnd();

                        GameClass? newClass = JsonSerializer.Deserialize<GameClass>(json);

                        if (newClass == null) continue;

                        classes.Add(newClass.id, newClass);
                    }
                }
                catch
                {
                    continue;
                }
            }

            return classes;
        }

        /// <summary>
        /// Loads all races JSONs from given path.
        /// <param name="path">Path to the race folder.</param>
        /// </summary>
        public static Dictionary<string, GameRace> LoadRaces(string path)
        {
            Dictionary<string, GameRace> races = new();

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string[] filePaths = Directory.GetFiles(path);

            foreach (var filePath in filePaths)
            {
                try
                {
                    using (StreamReader r = new StreamReader(filePath))
                    {
                        string json = r.ReadToEnd();

                        GameRace? newRace = JsonSerializer.Deserialize<GameRace>(json);

                        if (newRace == null) continue;

                        races.Add(newRace.id, newRace);
                    }
                }
                catch
                {
                    continue;
                }
            }

            return races;
        }

        /// <summary>
        /// Creates new character, generates it's id and saves it.
        /// </summary>
        /// <param name="path">Path to the character folder.</param>
        /// <returns>New character.</returns>
        public Character NewCharacter(string path)
        {
            Character newCharacter = new();
            string id = BASICNAME;
            string originalID = id;

            for (int i = 1; _characters.ContainsKey(id); ++i)
            {
                id = $"{originalID}{i}";
            }

            _characters[id] = newCharacter;

            SaveCharacter(newCharacter, path);
            return newCharacter;
        }

        /// <summary>
        /// Saves character to JSON to given path.
        /// </summary>
        /// <param name="character">Character that is to be saved.</param>
        /// <param name="path">Path to the character folder.</param>>
        /// <exception cref="ArgumentNullException"></exception>
        public void SaveCharacter(Character character, string path)
        {
            string fileName;
            string filePath;
            string jsonString;

            if (character == null)
            {
                throw new ArgumentNullException();
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            jsonString = JsonSerializer.Serialize(character);

            if (TryGetCharacterID(character, out string id))
            {
                fileName = id;
            }
            else
            {
                throw new ApplicationException("Character with not registered ID.");
            }

            fileName = GetFilename(fileName);
            filePath = Path.Combine(path, fileName);
            File.WriteAllText(filePath, jsonString);
        }

        /// <summary>
        /// Generates empty GameRace JSON in given path.
        /// </summary>
        /// <param name="path">Path were the JSON should be created.</param>
        public static void GenerateGameRaceEmptyJSON(string path)
        {
            string rndName = $"{DateTime.Now.Ticks}";
            GameRace gameRace = new GameRace()
            {
                id = rndName,
                name = rndName,
                traits = [new GameRace.Trait("traitName", "traitDescription")]
            };


            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string jsonString = JsonSerializer.Serialize(gameRace);

            string filePath = Path.Combine(path, gameRace.id + ".json");
            File.WriteAllText(filePath, jsonString);
        }

        /// <summary>
        /// Generates empty GameClass JSON in given path.
        /// </summary>
        /// <param name="path">Path were the JSON should be created.</param>
        public static void GenerateGameClassEmptyJSON(string path)
        {
            string rndName = $"{DateTime.Now.Ticks}";
            GameClass gameClass = new GameClass()
            {
                id = rndName,
                name = rndName,
                features = [new GameClass.Feature(0, "featureDescription")]
            };


            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string jsonString = JsonSerializer.Serialize(gameClass);

            string filePath = Path.Combine(path, gameClass.id + ".json");
            File.WriteAllText(filePath, jsonString);
        }
    }
}
