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

        internal CharacterMenu(ConsoleManager cm, Menu? parent, Character character) : base(cm, parent)
        {
            this.character = character;
            this.name = "Character: " + character.GetName();
            this.cm = cm;
            this.parentMenu = parent;
            this.showHelp = false;
        }

        internal override void Show()
        {
            for (int i = 0; i < 6; ++i)
            {
                Console.WriteLine(Character.abilityNames[i] + ": " + character.baseAbiltyScore[i] + " (" + character.GetModifier(i) + ")");
            }

            if (showHelp)
            {
                Console.Write(help);
                showHelp = false;
            }
            else
            {
                Console.WriteLine("\nFor help write '#help' or '#h'.\n");
            }            
        }

        internal override bool React(string input)
        {
            input = input.Trim();

            if (input.Length >= 1 && input[0] == '#')
            {
                return ReactToCommand(input);
            }

            return true;
        }

        bool ReactToCommand(string input)
        {
            switch (input)
            {
                case "#exit":
                case "#e":
                cm.Exit();
                return true;

                case "#help":
                case "#h":
                showHelp = true;
                return true;

                case "#back":
                case "#b":
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
                "'#back' or '#b' - Go back to Choose character menu.\n" +
                "'#exit' or '#e' - To exit application.\n" +
                "'#help' or '#h' - shows all commands and what they do.\n";
    }
}
