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
    /// Works basically as finite state machine where menus are the states.
    /// </summary>
    internal class ConsoleManager
    {
        internal Menu activeMenu;
        internal List<Character> characters;
        internal Dictionary<string, GameClass> classes { get; private set; }
        internal Dictionary<string, GameRace> races { get; private set; } 
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
            characters = JSONManager.LoadCharacters(charactersPath);
            classes = JSONManager.LoadClasses(classesPath);
            races = JSONManager.LoadRaces(racesPath);
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
            JSONManager.DeleteCharacter(character, charactersPath);
        }

        /// <summary>
        /// Saves character to JSON to charactersPath.
        /// </summary>
        /// <param name="character">Character that is to be saved.</param>
        internal void SaveCharacter(Character character)
        {
            JSONManager.SaveCharacter(character, charactersPath);
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
    }
}
