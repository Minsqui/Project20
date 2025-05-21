using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace ConsoleUI.Menus
{
    /// <summary>
    /// Menu where user can see all informations about the character and edit them.
    /// </summary>
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

        /// <summary>
        /// Draws/writes all information user needs.
        /// From three parts.
        /// 1/ Panel - stationary information, based on panelTextOptions.
        /// 2/ Help - Help information. All help information hides after new input.
        /// 3/ Message - information about invalid input or check results. Hides after new input.
        /// </summary>
        internal override void Show()
        {
            PanelDraw();

            Console.WriteLine();

            //Help
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
            
            //Message
            if(showMessage)
            {
                Console.WriteLine(message);
                Console.WriteLine();
                showMessage = false;
            }
        }

        /// <summary>
        /// Menu's reactions to user's input.
        /// </summary>
        /// <param name="input">User's input.</param>
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

        /// <summary>
        /// Menu's reactions to command inputs.
        /// </summary>
        /// <param name="input">User's input.</param>
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
                "           'class', 'race', 'skill', \n" +
                "'/help' or '/h' - shows all commands and what they do.";

        /// <summary>
        /// Go back to parent menu command.
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        private void BackCommand()
        {
            if (parentMenu == null)
            {
                throw new NullReferenceException();
            }
            cm.activeMenu = parentMenu;
        }

        /// <summary>
        /// All check commands.
        /// Makes a roll and adds modifiers based on the character and type of the check.
        /// </summary>
        /// <param name="input">User's input.</param>
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
                WriteMessage($"Ability check: {character.CheckAbility(input[1])} | {character.CheckAbility(input[1])}");
                return;
            }

            //Skill check
            //Checking if skill exists
            if (character.CheckSkill(input[1]) != null)
            {
                WriteMessage($"Skill check: {character.CheckSkill(input[1])} | {character.CheckSkill(input[1])}");
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
                    WriteMessage($"Save check: {character.CheckSave(input[2])} | {character.CheckSave(input[2])}");
                    return;
                }
            }

            WriteInvalidCommand();
            return;
        }

        /// <summary>
        /// Deletes character and goes back to parent menu.
        /// </summary>
        private void DeleteCommand()
        {
            cm.DeleteCharacter(character);
            BackCommand();
        }

        /// <summary>
        /// Edits character's ability command.
        /// </summary>
        /// <param name="input">User's input.</param>
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

        /// <summary>
        /// Edits character's class command.
        /// </summary>
        /// <param name="input">User's input.</param>
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

            character.ClassID = input[2];
        }

        /// <summary>
        /// Edits character's hitpoints command.
        /// </summary>
        /// <param name="input">User's input.</param>
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

            character.currentHP = hpNumber;
        }

        /// <summary>
        /// Edits character's level command.
        /// </summary>
        /// <param name="input">User's input.</param>
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

        /// <summary>
        /// Edits character's name command.
        /// </summary>
        /// <param name="input">User's input.</param>
        private void EditName(string[] input)
        {
            //Not enough arguments
            if (input.Length < 3)
            {
                WriteInvalidCommand();
                return;
            }

            character.Name = input[2];
            return;
        }

        /// <summary>
        /// Edits character's race command.
        /// </summary>
        /// <param name="input">User's input.</param>
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

            character.RaceID = input[2];
        }

        /// <summary>
        /// Edits character's saving throw proficiency multiplier command.
        /// </summary>
        /// <param name="input">User's input.</param>
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

        /// <summary>
        /// Edits character's skill proficiency multiplier command.
        /// </summary>
        /// <param name="input">User's input.</param>
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

        /// <summary>
        /// Chooses which edit command is used.
        /// </summary>
        /// <param name="input">User's input.</param>
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

        /// <summary>
        /// Returns menu name.
        /// </summary>
        /// <returns>Menu name.</returns>
        internal override string GetName()
        {
            return $"{name}: {character.Name}";
        }

        /// <summary>
        /// Function that decides which panel to draw.
        /// </summary>
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

        /// <summary>
        /// Draws\writes all abilities their modifiers and their saving throw modifiers.
        /// </summary>
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

        /// <summary>
        /// Draws/writes all classes in ConsoleManager's database.
        /// </summary>
        private void DrawAllClasses()
        {
            if (cm.GetClassIDs().Length <= 0)
            {
                Console.WriteLine("No class found in database.\n");
                return;
            }

            Console.WriteLine("Class name (class id)");

            foreach (var classID in cm.GetClassIDs())
            {
                GameClass? gameClass = cm.GetGameClass(classID);
                if (gameClass is null) continue;
                Console.WriteLine($"{gameClass.name} ({classID})");
            }
        }

        /// <summary>
        /// Draws/writes all races in ConsoleManager's database.
        /// </summary>
        private void DrawAllRaces()
        {
            if (cm.GetRaceIDs().Length <= 0)
            {
                Console.WriteLine("No race found in database.\n");
                return;
            }

            Console.WriteLine("Race name (race id)");

            foreach (var raceID in cm.GetRaceIDs())
            {
                GameRace? gameRace = cm.GetGameRace(raceID);
                if (gameRace is null) continue;
                Console.WriteLine($"{gameRace.name} ({raceID})");
            }
        }

        /// <summary>
        /// Draws/writes basic information about character. Meaning race, class, level and hitpoints.
        /// </summary>
        private void DrawBasicBio()
        {
            string raceName;
            string className;
            string hitpoints;
            GameClass? gameClass;


            raceName = cm.GetGameRace(character.RaceID)?.name ?? "raceNotFound";
            hitpoints = $"{character.currentHP}";

            gameClass = cm.GetGameClass(character.ClassID);
            if (gameClass == null)
            {
                className = "classNotFound";
            }
            else
            {
                className = gameClass.name;
                hitpoints += $"/{gameClass.hitDice * (character.level + character.GetAbilityModifier(2))}";
            }
            

            Console.WriteLine(
                $"Race: {raceName}\n" +
                $"Class: {className}\n" +
                $"Level: {character.level}\n" + 
                $"HP: {hitpoints}"
                );
        }

        /// <summary>
        /// Draws/writes character's class name and all class's features.
        /// Features above character's level are written darker.
        /// </summary>
        private void DrawClass()
        {
            GameClass? gameClass = cm.GetGameClass(character.ClassID);

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

        /// <summary>
        /// Draws/writes character's race name and all race's traits.
        /// </summary>
        private void DrawRace()
        {
            GameRace? gameRace = cm.GetGameRace(character.RaceID);

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

        /// <summary>
        /// Draws/writes all skills and their modifiers.
        /// </summary>
        private void DrawSkills()
        {
            for(int i = 0; i < Character.skillNames.Length; ++i)
            {
                Console.WriteLine($"{Character.skillNames[i]}: {character.GetSkillModifier(i)}");
            }
            return;
        }

        /// <summary>
        /// Show command. Decides which panel was chosen to be shown.
        /// </summary>
        /// <param name="input">User's input.</param>
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

        /// <summary>
        /// Sets message to Invalid input message.
        /// </summary>
        private void WriteInvalidCommand()
        {
            WriteMessage("Invalid command.");
        }

        /// <summary>
        /// Writes given message. 
        /// </summary>
        /// <param name="message">Message to be shown</param>
        private void WriteMessage(string message)
        {
            this.message = message;
            showMessage = true;
        }

        /// <summary>
        /// Naive function that writes long text without spliting in the middle of the word.
        /// </summary>
        /// <param name="text"></param>
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
