using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Project20
{
    /// <summary>
    /// Class that ensures communication between user and character class/database.
    /// Works basically as finite state machine where menus are states.
    /// </summary>
    internal class ConsoleManager
    {
        internal Menu activeMenu;
        internal List<Character> characters;
        internal Dictionary<string, GameClass> classes { get; } = new Dictionary<string, GameClass>();
        internal Dictionary<string, GameRace> races { get; } = new Dictionary<string, GameRace>();
        private string charactersPath = ".\\data\\characters";
        private string classesPath = ".\\data\\classes";
        private string racesPath = ".\\data\\races";

        public ConsoleManager()
        {
            Menu mainMenu = new MainMenu(this);
            this.activeMenu = mainMenu;
            this.characters = new List<Character>();
        }

        /// <summary>
        /// Method that contains all tasks needed to be done on start of ConsoleManager.Run()
        /// </summary>
        private void OnStart()
        {
            LoadCharacters();
            LoadClasses();
            LoadRaces();
        }

        /// <summary>
        /// Loads all character JSONs from charactersPath.
        /// </summary>
        private void LoadCharacters()
        {
            if (!Directory.Exists(charactersPath))
            {
                Directory.CreateDirectory(charactersPath);
            }

            string[] filePaths = Directory.GetFiles(charactersPath);
            
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
        }

        /// <summary>
        /// Loads all classes JSONs from classesPath.
        /// </summary>
        private void LoadClasses()
        {
            if (!Directory.Exists(classesPath))
            {
                Directory.CreateDirectory(classesPath);
            }

            string[] filePaths = Directory.GetFiles(classesPath);

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
        }

        /// <summary>
        /// Loads all races JSONs from racesPath.
        /// </summary>
        private void LoadRaces()
        {
            if (!Directory.Exists(racesPath))
            {
                Directory.CreateDirectory(racesPath);
            }

            string[] filePaths = Directory.GetFiles(racesPath);

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
        }

        /// <summary>
        /// Method that runs ConsoleManager.
        /// While loop created from two stages.
        /// Show stage is drawing all the things that menu needs to draw.
        /// React stage is getting input from user and doing all needed tasks with it.
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public void Run()
        {
            string? input;

            OnStart();

            while (true)
            {
                if (activeMenu == null)
                {
                    throw new NullReferenceException("Active menu does not exist (activeMenu == null)");
                }

                Console.WriteLine(activeMenu.GetName() + "\n");

                activeMenu.Show();

                input = Console.ReadLine();

                if (input == null)
                {
                    continue;
                }

                activeMenu.React(input);

                Console.Clear();
            }
        }

        /// <summary>
        /// Method of exiting console.
        /// </summary>
        internal void Exit()
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Adds given character to ConsoleManager database, also saves it as JSON.
        /// </summary>
        /// <param name="character">Character that is to be added to the ConsoleManager database.</param>
        internal void AddCharacter(Character character)
        {
            characters.Add(character);

            SaveCharacter(character);
        }

        /// <summary>
        /// Removes given character from cm database and also deletes it's JSON.
        /// </summary>
        /// <param name="character">Character that is to be removed.</param>
        internal void DeleteCharacter(Character character)
        {
            characters.Remove(character);

            if (!Directory.Exists(charactersPath))
            {
                return;
            }
            string filePath = Path.Combine(charactersPath, character.filename);
            File.Delete(filePath);
        }

        /// <summary>
        /// Saves character to JSON to charactersPath
        /// </summary>
        /// <param name="character">Character that is to be saved.</param>
        /// <exception cref="ArgumentNullException"></exception>
        internal void SaveCharacter(Character character)
        {
            string fileName;

            if (character == null)
            {
                throw new ArgumentNullException();
            }

            if (!Directory.Exists(charactersPath))
            {
                Directory.CreateDirectory(charactersPath);
            }

            string jsonString = JsonSerializer.Serialize(character);

            //Checking if character has name
            if (character.GetName() == Character.nameBaseValue)
            {
                //Generating file name
                fileName = Character.nameBaseValue + $@"{DateTime.Now.Ticks}";
            }
            else
            {
                fileName = character.GetName();
            }

            character.filename = fileName+".json";
            string filePath = Path.Combine(charactersPath, character.filename);
            File.WriteAllText(filePath, jsonString);
        }

        /// <summary>
        /// Returns GameClass with corresponding given id.
        /// </summary>
        /// <param name="classID">ID of the class.</param>
        /// <returns>GameClass with corresponding id, null if GameClass does not exist.</returns>
        public GameClass? GetGameClass(string classID)
        {
            if (classID == null) return null;

            if (!classes.TryGetValue(classID, out var chosenClass))
            {
                return null;
            }

            return chosenClass;
        }

        /// <summary>
        /// Returns GameRace with corresponding given id.
        /// </summary>
        /// <param name="raceID">ID of the race.</param>
        /// <returns>GameRace with corresponding id, null if GameRace does not exist.</returns>
        public GameRace? GetGameRace(string raceID)
        {
            if (raceID == null) return null;

            if (!races.TryGetValue(raceID, out var chosenRace))
            {
                return null;
            }

            return chosenRace;
        }

        /// <summary>
        /// Generates empty GameRace JSON in given path.
        /// </summary>
        /// <param name="path">Path were the JSON should be created.</param>
        public static void GenerateGameRaceEmptyJSON(string path)
        {
            string rndName = $"{DateTime.Now.Ticks}";
            GameRace gameRace = new GameRace() {
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
