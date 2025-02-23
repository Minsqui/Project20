using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project20
{
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

        private void OnStart()
        {
            LoadCharacters();
        }

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

        public void Run()
        {
            string? input;

            OnStart();

            while (true)
            {
                if (activeMenu == null)
                {
                    throw new Exception("Active menu does not exist (activeMenu == null)");
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

        internal void Exit()
        {
            Environment.Exit(0);
        }

        internal void AddCharacter(Character character)
        {
            characters.Add(character);

            SaveCharacter(character);
        }

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
