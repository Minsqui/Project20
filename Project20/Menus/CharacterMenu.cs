using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Project20.Menus
{
    internal class CharacterMenu:Menu
    {
        private Character character;
        bool showHelp = false;
        bool showMessage = false;
        string message = "";
        panelTextOptions panelText = panelTextOptions.basicBioPanel;

        private enum panelTextOptions {
            abilityPanel, allclasses, allraces, characterClass, characterRace, skillPanel, basicBioPanel 
        };

        internal CharacterMenu(ConsoleManager cm, Menu? parent, Character character) : base(cm, parent)
        {
            this.character = character;
            this.name = "Character";
            this.cm = cm;
            this.parentMenu = parent;
        }

        internal override void Show()
        {
            PanelDraw();

            Console.WriteLine();

            if (showHelp)
            {
                Console.WriteLine(help);
                showHelp = false;
            }
            else
            {
                Console.WriteLine("For help write '/help' or '/h'.");
            }

            Console.WriteLine();
            
            if(showMessage)
            {
                Console.WriteLine(message);
                Console.WriteLine();
                showMessage = false;
            }
        }

        internal override void React(string input)
        {
            input = input.Trim();

            if (input.Length >= 1 && input[0] == '/')
            {
                ReactToCommand(input);
                return;
            }

            return;
        }

        void ReactToCommand(string input)
        {
            string[] splitInput = input.Split();

            switch (splitInput[0].ToLower())
            {
                case "/back":
                case "/b":
                    BackCommand();
                    return;

                case "/check":
                    CheckCommand(splitInput);
                    return;

                case "/delete":
                    DeleteCommand();
                    return;

                case "/edit":
                    EditCommand(splitInput);
                    return;

                case "/exit":
                case "/e":
                    cm.Exit();
                    return;

                case "/help":
                case "/h":
                    showHelp = true;
                    return;

                case "/show":
                    ShowCommand(splitInput);
                    return;

                default:
                    WriteInvalidCommand();
                    return;
            }
        }

        string help =
                "'/back' or '/b' - Go back to Choose character menu.\n" +
                "'/check' - Makes a roll and adds modifiers.\n" +
                "   /check <ability name>\n" +
                "   /check save <ability name>\n" +
                "   /check <skill name>\n" +
                "'/delete' - Deletes character and goes back to Choose character menu.\n" +
                "'/edit' - To edit values.\n" + 
                "   /edit ability <ability name> <value>\n" +
                "   /edit class <class id>\n" +
                "   /edit hp <value>" +
                "   /edit level <value>\n" +
                "   /edit name <name>\n" +
                "   /edit race <race id>\n" +
                "   /edit skill <skill name> <proficiency multiplier>\n" +
                "   /edit save <ability name> <proficiency multiplier>\n" +
                "'/exit' or '/e' - To exit application.\n" +
                "'/show' - changes the main shown panel.\n" +
                "   /show <panelType>\n" +
                "       <panelType> = 'ability', 'allclasses', 'allraces', 'basicbio',\n" +
                "       <panelType> = 'ability', 'allclasses', 'allraces', 'basicbio',\n" +
                "           'class', 'race', 'skill', \n" +
                "'/help' or '/h' - shows all commands and what they do.";

        private void BackCommand()
        {
            if (parentMenu == null)
            {
                throw new NullReferenceException();
            }
            cm.activeMenu = parentMenu;
        }

        private void CheckCommand(string[] input)
        {
            //Not enough arguments
            if (input.Length < 2)
            {
                WriteInvalidCommand();
                return;
            }

            //Ability check
            //Checking if ability exists
            if (character.CheckAbility(input[1]) != null)
            {
                WriteOutput($"Ability check: {character.CheckAbility(input[1])} | {character.CheckAbility(input[1])}");
                return;
            }

            //Skill check
            //Checking if skill exists
            if (character.CheckSkill(input[1]) != null)
            {
                WriteOutput($"Skill check: {character.CheckSkill(input[1])} | {character.CheckSkill(input[1])}");
                return;
            }

            //Not enough arguments
            if (input.Length < 3)
            {
                WriteInvalidCommand();
                return;
            }
            
            //Save throw check
            if (input[1] == "save")
            {
                //Checking if ability exists
                if (character.CheckSave(input[2]) != null)
                {
                    WriteOutput($"Save check: {character.CheckSave(input[2])} | {character.CheckSave(input[2])}");
                    return;
                }
            }

            WriteInvalidCommand();
            return;
        }

        private void DeleteCommand()
        {
            cm.DeleteCharacter(character);
            character = null;
            BackCommand();
        }

        private void EditAbility(string[] input)
        {
            int value;

            //Not enough arguments
            if (input.Length < 4)
            {
                WriteInvalidCommand();
                return;
            }

            //Checking if input the fourth argument is number + converting the argument to number
            if (!int.TryParse(input[3], out value))
            {
                WriteInvalidCommand();
                return;
            }

            if (character.EditAbilityScore(input[2], value))
            {
                return;
            }

            WriteInvalidCommand();
            return;
        }

        private void EditClass(string[] input)
        {
            //Not enough arguments
            if (input.Length < 3)
            {
                WriteInvalidCommand();
                return;
            }

            //Checking if class exists
            if (cm.GetGameClass(input[2]) == null)
            {
                WriteInvalidCommand();
                return;
            }

            character.EditClass(input[2]);
        }

        private void EditHP(string[] input)
        {
            int hpNumber;

            //Not enough arguments
            if (input.Length < 3)
            {
                WriteInvalidCommand();
                return;
            }

            //Checking if input the third argument is number + converting the argument to number
            if (!int.TryParse(input[2], out hpNumber))
            {
                WriteInvalidCommand();
                return;
            }

            character.EditHP(hpNumber);
        }

        private void EditLevel(string[] input)
        {
            int levelNumber;

            //Not enough arguments
            if (input.Length < 3)
            {
                WriteInvalidCommand();
                return;
            }

            //Checking if input the third argument is number + converting the argument to number
            if (!int.TryParse(input[2], out levelNumber))
            {
                WriteInvalidCommand();
                return;
            }

            //Checking if level is in bound of rules
            if (character.EditLevel(levelNumber))
            {
                return;
            }

            WriteInvalidCommand();
            return;
        }

        private void EditName(string[] input)
        {
            //Not enough arguments
            if (input.Length < 3)
            {
                WriteInvalidCommand();
                return;
            }

            character.EditName(input[2]);
            return;
        }

        private void EditRace(string[] input)
        {
            //Not enough arguments
            if (input.Length < 3)
            {
                WriteInvalidCommand();
                return;
            }

            //Checking if race exists
            if (cm.GetGameRace(input[2]) == null)
            {
                WriteInvalidCommand();
                return;
            }

            character.EditRace(input[2]);
        }

        private void EditSaveThrow(string[] input)
        {
            int value;

            //Not enough arguments
            if (input.Length < 4)
            {
                WriteInvalidCommand();
                return;
            }

            //Checking if input the fourth argument is number + converting the argument to number
            if (!int.TryParse(input[3], out value))
            {
                WriteInvalidCommand();
                return;
            }

            if (character.EditSaveThrow(input[2], value))
            {
                return;
            }

            WriteInvalidCommand();
            return;
        }

        private void EditSkill(string[] input)
        {
            int value;

            //Not enough arguments
            if (input.Length < 4)
            {
                WriteInvalidCommand();
                return;
            }

            //Checking if input the fourth argument is number + converting the argument to number
            if (!int.TryParse(input[3], out value))
            {
                WriteInvalidCommand();
                return;
            }

            if (character.EditSkillProficiency(input[2], value))
            {
                return;
            }

            WriteInvalidCommand();
            return;
        }

        private void EditCommand(string[] input)
        {
            //Not enough arguments
            if (input.Length < 2)
            {
                WriteInvalidCommand();
                return;
            }

            switch (input[1].ToLower())
            {
                case "ability":
                    EditAbility(input);
                    break;

                case "class":
                    EditClass(input);
                    break;

                case "hp":
                    EditHP(input);
                    break;

                case "level":
                    EditLevel(input);
                    break;

                case "name":
                    EditName(input);
                    break;

                case "race":
                    EditRace(input);
                    break;

                case "save":
                    EditSaveThrow(input);
                    break;

                case "skill":
                    EditSkill(input);
                    break;

                default:
                    WriteInvalidCommand();
                    return;
            }
            cm.SaveCharacter(character);
        }

        internal override string GetName()
        {
            return $"{name}: {character.name}";
        }

        private void PanelDraw()
        {
            switch (panelText)
            {
                case panelTextOptions.abilityPanel:
                    DrawAbilities();
                    return;

                case panelTextOptions.allclasses:
                    DrawAllClasses();
                    return;

                case panelTextOptions.allraces:
                    DrawAllRaces();
                    return;

                case panelTextOptions.basicBioPanel:
                    DrawBasicBio();
                    return;

                case panelTextOptions.characterClass:
                    DrawClass();
                    return;

                case panelTextOptions.characterRace:
                    DrawRace();
                    return;

                case panelTextOptions.skillPanel:
                    DrawSkills();
                    return;

                default:
                    Console.WriteLine("No panel was selected");
                    return;
            }
        }

        private void DrawAbilities()
        {
            Console.WriteLine("Ability name (modifier) (save modifier)");
            for (int i = 0; i < 6; ++i)
            {
                Console.WriteLine(
                    $"{Character.abilityNames[i]}:" +
                    $" {character.abilityScore[i]} " +
                    $"({character.GetAbilityModifier(i)}) " +
                    $"({character.GetSaveModifier(i)})"
                    );
            }
        }

        private void DrawAllClasses()
        {
            if (cm.classes.Count <= 0)
            {
                Console.WriteLine("No class found in database.\n");
                return;
            }

            Console.WriteLine("Class name (class id)");

            foreach (var gameClass in cm.classes)
            {
                Console.WriteLine($"{gameClass.Value.name} ({gameClass.Key})");
            }
        }

        private void DrawAllRaces()
        {
            if (cm.races.Count <= 0)
            {
                Console.WriteLine("No race found in database.\n");
                return;
            }

            Console.WriteLine("Race name (race id)");

            foreach (var gameRace in cm.races)
            {
                Console.WriteLine($"{gameRace.Value.name} ({gameRace.Key})");
            }
        }

        private void DrawBasicBio()
        {
            string raceName;
            string className;
            string hitpoints;
            GameClass? gameClass;


            raceName = cm.GetGameRace(character.raceID)?.name ?? "raceNotFound";
            hitpoints = $"{character.currentHP}";

            gameClass = cm.GetGameClass(character.classID);
            if (gameClass == null)
            {
                className = "classNotFound";
            }
            else
            {
                className = gameClass.name;
                hitpoints += $"/{gameClass.hitDice * character.level}";
            }
            

            Console.WriteLine(
                $"Race: {raceName}\n" +
                $"Class: {className}\n" +
                $"Level: {character.level}\n" + 
                $"HP: {hitpoints}"
                );
        }

        private void DrawClass()
        {
            GameClass? gameClass = cm.GetGameClass(character.classID);

            if (gameClass == null)
            {
                Console.WriteLine("Character's class not found.");
                return;
            }

            Console.WriteLine($"Class name: {gameClass.name}");

            if (gameClass.features == null || gameClass.features.Length == 0)
            {
                Console.WriteLine("No class features found");
                return;
            }

            ConsoleColor originalColor = Console.ForegroundColor;

            Console.WriteLine("Features:");

            foreach (var feature in gameClass.features)
            {
                if (feature.level <= character.level)
                {
                    Console.ForegroundColor = originalColor;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }

                PrintText(feature.text);
                Console.WriteLine("\n");
            }

            Console.ForegroundColor = originalColor;
        }

        private void DrawRace()
        {
            GameRace? gameRace = cm.GetGameRace(character.raceID);

            if (gameRace == null)
            {
                Console.WriteLine("Character's race not found.");
                return;
            }

            Console.WriteLine($"Race name: {gameRace.name}");

            if (gameRace.traits == null || gameRace.traits.Length == 0)
            {
                Console.WriteLine("No race traits found");
                return;
            }

            Console.WriteLine("Traits:");

            foreach (var trait in gameRace.traits)
            {
                PrintText(trait.description);
                Console.WriteLine("\n");
            }

        }

        private void DrawSkills()
        {
            for(int i = 0; i < Character.skillNames.Length; ++i)
            {
                Console.WriteLine($"{Character.skillNames[i]}: {character.GetSkillModifier(i)}");
            }
            return;
        }

        private void ShowCommand(string[] input)
        {
            //Not enough arguments
            if (input.Length < 2)
            {
                WriteInvalidCommand();
                return;
            }

            switch (input[1].ToLower())
            {
                case "ability":
                case "abilities":
                    panelText = panelTextOptions.abilityPanel;
                    return;

                case "allclasses":
                    panelText = panelTextOptions.allclasses;
                    return;

                case "allraces":
                    panelText = panelTextOptions.allraces;
                    return;

                case "basicbio":
                    panelText = panelTextOptions.basicBioPanel;
                    return;

                case "class":
                    panelText = panelTextOptions.characterClass;
                    return;

                case "race":
                    panelText = panelTextOptions.characterRace;
                    return;

                case "skill":
                case "skills":
                    panelText = panelTextOptions.skillPanel;
                    return;                

                default:
                    WriteInvalidCommand();
                    return;
            }
        }

        private void WriteInvalidCommand()
        {
            WriteOutput("Invalid command.");
        }

        private void WriteOutput(string output)
        {
            this.message = output;
            showMessage = true;
        }

        private static void PrintText(string text)
        {
            int consoleWidth = Console.WindowWidth;

            foreach (var word in text.Split())
            {
                if (Console.CursorLeft + word.Length >= consoleWidth)
                {
                    Console.WriteLine();
                }
                Console.Write(word + " ");
            }
        }
    }

}
