using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project20
{
    /// <summary>
    /// Class that ensures communication between user and character class/database.
    /// </summary>
    internal class ConsoleManager
    {
        internal Menu activeMenu;
        internal List<Character> characters;
        private string charactersPath = ".\\data\\characters";

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
        }

        /// <summary>
        /// Loads all character JSONs from charactersPath.
        /// </summary>
        private void LoadCharacters()
        {
            string[] filePaths = Directory.GetFiles(charactersPath);
            
            foreach (var filePath in filePaths)
            {
                try
                {
                    using (StreamReader r = new StreamReader(filePath))
                    {
                        string json = r.ReadToEnd();
                        characters.Add(JsonSerializer.Deserialize<Character>(json));
                    }
                }
                catch
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// Method that runs ConsoleManager
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

                Console.WriteLine(activeMenu.name + "\n");

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
        /// Adds a character to ConsoleManager database, also saves it as JSON.
        /// </summary>
        /// <param name="character">Character that is added to ConsoleManager database.</param>
        internal void AddCharacter(Character character)
        {
            characters.Add(character);

            SaveCharacter(character);
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

            string filePath = Path.Combine(charactersPath, fileName+".json");
            File.WriteAllText(filePath, jsonString);
        }
    }
}
