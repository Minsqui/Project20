using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project20.Menus
{
    /// <summary>
    /// Menu where user creates a new character.
    /// </summary>
    internal class CharacterCreationMenu:Menu
    {
        /// <summary>
        /// Phase in creation.
        /// </summary>
        int phase;
        bool showInvalidInput;

        Character newCharacter;

        internal CharacterCreationMenu(ConsoleManager cm, Menu parent): base(cm, parent)
        {
            this.name = "Character Creation";
            this.cm = cm;
            this.parentMenu = parent;
            this.phase = 0;
            this.newCharacter = new Character();
            this.showInvalidInput = false;
        }

        /// <summary>
        /// Draws/writes all information for user to create a character.
        /// </summary>
        internal override void Show()
        {
            //Basic control message
            Console.Write(
                "All inputs starting with /, are taken as non-valid.\n" +
                "To exit application (progress will not be saved) write '/exit' or '/e'\n" +
                "To return back in creation write '/back' or '/b'\n" +
                "To save unfinished work and exit creation write '/done' or '/d'\n\n"
                );

            //Invalid input message
            if (showInvalidInput)
            {
                Console.WriteLine("Invalid input.\n");
                showInvalidInput = false;
            }

            //Message based on creation phase
            switch (phase)
            {
                //Name
                case 0:
                    Console.WriteLine("Write characters's name:");
                    return;

                //Base ability score
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    Console.WriteLine("Write character's " + Character.abilityNames[phase - 1]);
                    return;

                //Choose race
                case 7:
                    ShowRaces();
                    return;

                //Choose class
                case 8:
                    ShowClasses();
                    return;

                //End case
                default:
                    Console.WriteLine("Character creation is finished!");
                    return;

            }
        }

        /// <summary>
        /// Reactions to user input.
        /// </summary>
        /// <param name="input">Users input.</param>
        internal override void React(string input)
        {
            input = input.Trim();

            //Checking for control inputs
            if (input.Length >= 1 && input[0] == '/')
            switch (input)
            {
                case "/exit":
                case "/e":
                    cm.Exit();
                    return;

                case "/back":
                case "/b":
                    if(phase >= 1)
                    {
                        --phase;
                    }   
                    return;

                case "/done":
                case "/d":
                    End();
                    return;

                default:
                    return;
            }

            //Reactions based on phase
            switch (phase)
            {
                //Choose name phase
                case 0:
                    newCharacter.EditName(input);
                    ++phase;
                    return;

                //Ability score phases
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    EditBaseAbilityScore(phase - 1, input);
                    return;

                //Choose race phase
                case 7:
                    ChooseRace(input);
                    return;

                //Choose class phase
                case 8:
                    ChooseClass(input);
                    return;

                //End
                default:
                    End();
                    return;
            }
        }

        /// <summary>
        /// Adds character to database and returns to parent menu.
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        private void End()
        {
            cm.AddCharacter(newCharacter);

            if (parentMenu == null)
            {
                throw new NullReferenceException();
            }
            cm.activeMenu = parentMenu;

            return;
        }

        /// <summary>
        /// Edits abilities score, ability score phases.
        /// </summary>
        /// <param name="index">Index of the ability</param>
        /// <param name="input">Users input.</param>
        /// <returns>Returns true if the edit was successful.</returns>
        private bool EditBaseAbilityScore(int index, string input)
        {
            int value;

            if(!int.TryParse(input, out value))
            {
                showInvalidInput = true;
                return true;
            }

            ++phase;
            return newCharacter.EditAbilityScore(index, value);
        }

        /// <summary>
        /// If input is valid, sets character's race. If not, shows Invalid input message.
        /// </summary>
        /// <param name="input">Users input.</param>
        private void ChooseRace(string input)
        {
            GameRace? chosenRace;

            //No races found in database
            if (cm.races.Count <= 0)
            {
                ++phase;
                return;
            }

            chosenRace = cm.GetGameRace(input);

            //Checking if given input is valid.
            if (chosenRace == null)
            {
                showInvalidInput = true;
                return;
            }

            newCharacter.raceID = chosenRace.id;
            newCharacter.AddAbilityScore(chosenRace.abilityScore);
            ++phase;
            return;
        }

        /// <summary>
        /// If input is valid, sets character's class. If not, shows Invalid input message.
        /// </summary>
        /// <param name="input">Users input.</param>
        private void ChooseClass(string input)
        {
            GameClass? chosenClass;

            //No class in database
            if (cm.classes.Count <= 0)
            {
                ++phase;
                return;
            }

            chosenClass = cm.GetGameClass(input);

            //Checking if given input is valid
            if (chosenClass == null)
            {
                showInvalidInput = true;
                return;
            }

            newCharacter.classID = chosenClass.id;
            newCharacter.EditSaveThrow(chosenClass.saveThrows);
            ++phase;
            return;
        }

        /// <summary>
        /// Shows all possible races.
        /// </summary>
        private void ShowRaces()
        {
            if (cm.races.Count <= 0)
            {
                Console.WriteLine(
                    "No race found in database.\n" +
                    "Give any input to continue."
                    );
                return;
            }

            Console.WriteLine(
                "Choose character's race.\n" +
                "To choose write race's id.\n" +
                "Race name (race id)"
                );

            foreach (var gameRace in cm.races)
            {
                Console.WriteLine($"{gameRace.Value.name} ({gameRace.Key})");
            }
        }

        /// <summary>
        /// Shows all possible classes.
        /// </summary>
        private void ShowClasses()
        {
            if (cm.classes.Count <= 0)
            {
                Console.WriteLine(
                    "No class found in database.\n" +
                    "Give any input to continue."
                    );
                return;
            }

            Console.WriteLine(
                "Choose character's class.\n" +
                "To choose write class's id.\n" +
                "Class name (class id)"
                );

            foreach (var gameClass in cm.classes)
            {
                Console.WriteLine($"{gameClass.Value.name} ({gameClass.Key})");
            }
        }
    }
}
