using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project20.Menus
{
    internal class CharacterCreationMenu:Menu
    {
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

        internal override void Show()
        {
            Console.Write(
                "All inputs starting with /, are taken as non-valid.\n" +
                "To exit application (progress will not be saved) write '/exit' or '/e'\n" +
                "To return back in creation write '/back' or '/b'\n" +
                "To save unfinished work and exit creation write '/done' or '/d'\n\n"
                );

            if (showInvalidInput)
            {
                Console.WriteLine("Invalid input.\n");
                showInvalidInput = false;
            }

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

                case 7:
                    ShowRaces();
                    return;

                case 8:
                    ShowClasses();
                    return;

                //End case
                default:
                    Console.WriteLine("Character creation is finished!");
                    return;

            }
        }

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

            switch (phase)
            {
                case 0:
                    newCharacter.EditName(input);
                    ++phase;
                    return;

                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    EditBaseAbilityScore(phase - 1, input);
                    return;

                case 7:
                    ChooseRace(input);
                    return;

                case 8:
                    ChooseClass(input);
                    return;

                default:
                    End();
                    return;
            }
        }

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

        private void ChooseRace(string input)
        {
            GameRace? chosenRace;

            if (cm.races.Count <= 0)
            {
                ++phase;
                return;
            }

            chosenRace = cm.GetGameRace(input);
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

        private void ChooseClass(string input)
        {
            GameClass? chosenClass;

            if (cm.classes.Count <= 0)
            {
                ++phase;
                return;
            }

            chosenClass = cm.GetGameClass(input);
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
