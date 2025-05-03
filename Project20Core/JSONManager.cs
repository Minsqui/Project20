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
        private Dictionary<string, GameClass> _classes = new();
        private Dictionary<string, GameRace> _races = new();

        private const string BASICNAME = "character";

        private const string EXTENSION = ".json";
        private static string GetFilename(string id)
        {
            return $"{id}{EXTENSION}";
        }

        /// <summary>
        /// Tries to find characters ID. Returns true if finding was successful.
        /// </summary>
        /// <param name="character">Character whose ID is being searched for.</param>
        /// <param name="id">ID that was found.</param>
        /// <returns>True if finding was successful.</returns>
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
        /// Returns array of all character ids.
        /// </summary>
        /// <returns>Array of all character ids.</returns>
        public string[] GetCharacterIDs()
        {
            return _characters.Select(x => x.Key).ToArray();
        }

        /// <summary>
        /// Returns array of all class ids.
        /// </summary>
        /// <returns>Array of all class ids.</returns>
        public string[] GetClassIDs()
        {
            return _classes.Select(x => x.Key).ToArray();
        }

        /// <summary>
        /// Returns array of all race ids.
        /// </summary>
        /// <returns>Array of all race ids.</returns>
        public string[] GetRaceIDs()
        {
            return _races.Select(x => x.Key).ToArray();
        }

        /// <summary>
        /// Returns character with given ID.
        /// </summary>
        /// <param name="id">ID of the character.</param>
        /// <returns>Character with given ID. Null if there is no character with given ID.</returns>
        public Character? GetCharacter(string id)
        {
            if (!_characters.ContainsKey(id)) return null;

            return _characters[id];
        }

        /// <summary>
        /// Returns array of all loaded characters.
        /// </summary>
        /// <returns>Array of all loaded characters.</returns>
        public Character[] GetAllCharacters()
        {
            return _characters.Values.ToArray();
        }

        /// <summary>
        /// Returns class with given ID.
        /// </summary>
        /// <param name="id">ID of the class.</param>
        /// <returns>Class with given ID. Null if there is no class with given ID.</returns>
        public GameClass? GetClass(string id)
        {
            if (!_classes.ContainsKey(id)) return null;

            return _classes[id];
        }

        /// <summary>
        /// Returns array of all loaded classes.
        /// </summary>
        /// <returns>Array of all loaded classes.</returns>
        public GameClass[] GetAllClasses()
        {
            return _classes.Values.ToArray();
        }

        /// <summary>
        /// Returns race with given ID.
        /// </summary>
        /// <param name="id">ID of the race.</param>
        /// <returns>Race with given ID. Null if there is no race with given ID.</returns>
        public GameRace? GetRace(string id)
        {
            if (!_races.ContainsKey(id)) return null;

            return _races[id];
        }

        /// <summary>
        /// Returns array of all loaded races.
        /// </summary>
        /// <returns>Array of all loaded races.</returns>
        public GameRace[] GetAllRaces()
        {
            return _races.Values.ToArray();
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
        public Dictionary<string, GameClass> LoadClasses(string path)
        {
            _classes = new();

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

                        _classes.Add(newClass.id, newClass);
                    }
                }
                catch
                {
                    continue;
                }
            }

            return _classes;
        }

        /// <summary>
        /// Loads all races JSONs from given path.
        /// <param name="path">Path to the race folder.</param>
        /// </summary>
        public Dictionary<string, GameRace> LoadRaces(string path)
        {
            _races = new();

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

                        _races.Add(newRace.id, newRace);
                    }
                }
                catch
                {
                    continue;
                }
            }

            return _races;
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
