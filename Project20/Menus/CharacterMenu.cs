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
        bool showHelp;
        bool showOutput;
        string output;

        internal CharacterMenu(ConsoleManager cm, Menu? parent, Character character) : base(cm, parent)
        {
            this.character = character;
            this.name = "Character: " + character.GetName();
            this.cm = cm;
            this.parentMenu = parent;
            this.showHelp = false;
            this.showOutput = false;
        }

        internal override void Show()
        {
            for (int i = 0; i < 6; ++i)
            {
                Console.WriteLine(Character.abilityNames[i] + ": " + character.baseAbiltyScore[i] + " (" + character.GetAbilityModifier(i) + ")");
            }

            if (showHelp)
            {
                Console.Write(help);
                showHelp = false;
            }
            else
            {
                Console.WriteLine("\nFor help write '/help' or '/h'.\n");
            }
            
            if(showOutput)
            {
                Console.Write(output);
                showOutput = false;
            }
        }

        internal override bool React(string input)
        {
            input = input.Trim().ToLower();

            if (input.Length >= 1 && input[0] == '/')
            {
                return ReactToCommand(input);
            }

            return true;
        }

        bool ReactToCommand(string input)
        {
            string[] splitInput = input.Split();

            if (splitInput[0] == "/edit")
            {
                if (splitInput.Length < 2)
                {
                    return true;
                }

                if (splitInput[1] == "ability")
                {
                    return EditAbility(splitInput);
                }
            }

            if (splitInput[0] == "/check")
            {
                return Check(splitInput);
            }

            switch (input)
            {
                case "/exit":
                case "/e":
                cm.Exit();
                return true;

                case "/help":
                case "/h":
                showHelp = true;
                return true;

                case "/back":
                case "/b":
                if (parentMenu == null)
                {
                    return false;
                }
                cm.activeMenu = parentMenu;
                return true;

                default:
                return true;
            }
        }

        string help = "\n" +
                "'/back' or '/b' - Go back to Choose character menu.\n" +
                "'/check' - Makes a roll and adds modifiers.\n" +
                "   /check <ability name>\n" +
                "   /check save <ability name>\n" +
                "   /check <skill name>\n" +
                "'/edit' - To edit values.\n" + 
                "   /edit ability <ability name> <value>\n" +
                "'/exit' or '/e' - To exit application.\n" +
                "'/help' or '/h' - shows all commands and what they do.\n";

        private bool EditAbility(string[] input)
        {
            int index;
            int value;

            if (input.Length < 4)
            {
                return true;
            }

            index = Character.GetAbilityIndex(input[2]);

            if (index < 0)
            {
                return true;
            }

            if (!int.TryParse(input[3], out value))
            {
                return true;
            }

            character.baseAbiltyScore[index] = value;
            cm.SaveCharacter(character);
            return true;
        }

        private bool Check(string[] input)
        {
            int abilityIndex;

            if (input.Length < 2)
            {
                return true;
            }

            abilityIndex = Character.GetAbilityIndex(input[1]);
            if (abilityIndex >= 0)
            {
                WriteOutput("Ability check: " + character.AbilityCheck(abilityIndex) + " | " + character.AbilityCheck(abilityIndex) + "\n");
                return true;
            }

            return true;
        }

        private void WriteOutput(string output)
        {
            this.output = output;
            showOutput = true;
        }
    }
}
