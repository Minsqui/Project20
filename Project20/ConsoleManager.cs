using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using ConsoleUI.Menus;
using Core;

namespace ConsoleUI
{
    /// <summary>
    /// Class that ensures communication between user and character class/database.
    /// Works basically as finite state machine where menus are the states.
    /// </summary>
    internal class ConsoleManager
    {
        internal Menu activeMenu;

        private JSONManager _dataManager;

        const string CHARACTERSPATH = ".\\data\\characters";
        const string CLASSESPATH = ".\\data\\classes";
        const string RACESPATH = ".\\data\\races";

        public ConsoleManager()
        {
            Menu mainMenu = new MainMenu(this);
            this.activeMenu = mainMenu;

            _dataManager = new JSONManager();
        }

        /// <summary>
        /// Method that contains all tasks needed to be done on start of ConsoleManager.Run()
        /// </summary>
        private void OnStart()
        {
            _dataManager.LoadCharacters(CHARACTERSPATH);
            _dataManager.LoadClasses(CLASSESPATH);
            _dataManager.LoadRaces(RACESPATH);
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
        /// Creates a new character and registers them to database and saves them.
        /// </summary>
        /// <returns>A new registered character.</returns>
        internal Character NewCharacter()
        {
            return _dataManager.NewCharacter(CHARACTERSPATH);
        }

        /// <summary>
        /// Removes given character from cm database and also deletes it's JSON.
        /// </summary>
        /// <param name="character">Character that is to be removed.</param>
        internal void DeleteCharacter(Character character)
        {
            _dataManager.DeleteCharacter(character, CHARACTERSPATH);
        }

        /// <summary>
        /// Saves character to JSON to charactersPath.
        /// </summary>
        /// <param name="character">Character that is to be saved.</param>
        internal void SaveCharacter(Character character)
        {
            _dataManager.SaveCharacter(character, CHARACTERSPATH);
        }

        /// <summary>
        /// Returns Character with corresponding given id.
        /// </summary>
        /// <param name="characterID">ID of the character.</param>
        /// <returns>Character with corresponding id, null if Character does not exist.</returns>
        public Character? GetCharacter(string characterID)
        {
            return _dataManager.GetCharacter(characterID);
        }

        /// <summary>
        /// Returns array of all loaded characters.
        /// </summary>
        /// <returns>Array of all loaded characters.</returns>
        public Character[] GetAllCharacters()
        {
            return _dataManager.GetAllCharacters();
        }

        /// <summary>
        /// Returns GameClass with corresponding given id.
        /// </summary>
        /// <param name="classID">ID of the class.</param>
        /// <returns>GameClass with corresponding id, null if GameClass does not exist.</returns>
        public GameClass? GetGameClass(string classID)
        {
            return _dataManager.GetClass(classID);
        }

        /// <summary>
        /// Returns GameRace with corresponding given id.
        /// </summary>
        /// <param name="raceID">ID of the race.</param>
        /// <returns>GameRace with corresponding id, null if GameRace does not exist.</returns>
        public GameRace? GetGameRace(string raceID)
        {
            return _dataManager.GetRace(raceID);
        }

        /// <summary>
        /// Returns array of all character ids.
        /// </summary>
        /// <returns>Array of all character ids.</returns>
        public string[] GetChraracterIDs()
        {
            return _dataManager.GetCharacterIDs();
        }

        /// <summary>
        /// Returns array of all class ids.
        /// </summary>
        /// <returns>Array of all class ids.</returns>
        public string[] GetClassIDs()
        {
            return _dataManager.GetClassIDs();
        }

        /// <summary>
        /// Returns array of all race ids.
        /// </summary>
        /// <returns>Array of all race ids.</returns>
        public string[] GetRaceIDs()
        {
            return _dataManager.GetRaceIDs();
        }
    }

}
