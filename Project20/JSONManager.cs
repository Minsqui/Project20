using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project20
{
    public class JSONManager
    {
        /// <summary>
        /// Deletes character's JSON.
        /// </summary>
        /// <param name="character">Character that is to be removed.</param>
        /// <param name="path">Path to the character folder.</param>
        public static void DeleteCharacter(Character character, string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }
            string filePath = Path.Combine(path, character.filename);
            File.Delete(filePath);
        }

        /// <summary>
        /// Loads all character JSONs from given path.
        /// <param name="path">Path to the character folder.</param>
        /// </summary>
        static public List<Character> LoadCharacters(string path)
        {
            List<Character> characters = new();

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

                        Character? newCharacter = JsonSerializer.Deserialize<Character>(json);

                        if (newCharacter == null) continue;

                        characters.Add(newCharacter);
                        newCharacter.filename = Path.GetFileName(filePath);
                    }
                }
                catch
                {
                    continue;
                }
            }

            return characters;
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
        /// Saves character to JSON to given path.
        /// </summary>
        /// <param name="character">Character that is to be saved.</param>
        /// <param name="path">Path to the character folder.</param>>
        /// <exception cref="ArgumentNullException"></exception>
        public static void SaveCharacter(Character character, string path)
        {
            string fileName;

            if (character == null)
            {
                throw new ArgumentNullException();
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string jsonString = JsonSerializer.Serialize(character);

            //Checking if character has name
            if (character.Name == Character.nameBaseValue)
            {
                //Generating file name
                fileName = Character.nameBaseValue + $@"{DateTime.Now.Ticks}";
            }
            else
            {
                fileName = character.Name;
            }

            character.filename = fileName + ".json";
            string filePath = Path.Combine(path, character.filename);
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
