using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Project20.Menus
{
    internal class CharacterMenu:Menu
    {
        private Character character;
        bool showHelp = false;
        bool showMessage = false;
        string message;
        panelTextOptions panelText = panelTextOptions.abilityPanel;

        private enum panelTextOptions { abilityPanel, skillPanel, basicBioPanel };

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
                "   /check save <ability name>\n" + //TODO
                "   /check <skill name>\n" + //TODO
                "'/delete' - Deletes character and goes back to Choose character menu.\n" +
                "'/edit' - To edit values.\n" + 
                "   /edit ability <ability name> <value>\n" +
                "   /edit level <value>\n" + //TODO
                "   /edit name <name>\n" +
                "   /edit skill <skill name> <proficiency multiplier>\n" +
                "   /edit save <ability name> <proficiency multiplier>\n" + //TODO 
                "'/exit' or '/e' - To exit application.\n" +
                "'/show' - changes the main shown panel.\n" +
                "   /show <panelType>\n" +
                "       <panelType> = 'ability', 'skill', 'basicbio'\n" +
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
            int abilityIndex;

            //Not enough arguments
            if (input.Length < 2)
            {
                WriteInvalidCommand();
                return;
            }

            //Checking if ability exist
            if (character.AbilityCheck(input[1]) != null)
            {
                WriteOutput("Ability check: " + character.AbilityCheck(input[1]) + " | " + character.AbilityCheck(input[1]));
                return;
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
            int index;
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

                case "name":
                    EditName(input);
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

                case panelTextOptions.basicBioPanel:
                    DrawBasicBio();
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
            for (int i = 0; i < 6; ++i)
            {
                Console.WriteLine($"{Character.abilityNames[i]}: {character.abilityScore[i]} ({character.GetAbilityModifier(i)})");
            }
        }

        private void DrawBasicBio()
        {
            string raceName;
            string className;


            raceName = cm.GetGameRace(character.raceID)?.name ?? "raceNotFound";
            className = cm.GetGameClass(character.classID)?.name ?? "classNotFound";

            Console.WriteLine($"Race: {raceName}");
            Console.WriteLine($"Class: {className}");
            Console.WriteLine($"Level: {character.level}");
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

                case "skill":
                case "skills":
                    panelText = panelTextOptions.skillPanel;
                    return;

                case "basicbio":
                    panelText = panelTextOptions.basicBioPanel;
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
    }
}
