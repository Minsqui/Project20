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

        Character newCharacter;

        internal CharacterCreationMenu(ConsoleManager cm, Menu parent): base(cm, parent)
        {
            this.name = "Character Creation";
            this.cm = cm;
            this.parentMenu = parent;
            this.phase = 0;
            this.newCharacter = new Character();
        }

        internal override void Show()
        {
            Console.Write(
                "All inputs starting with #, are taken as non-valid.\n" +
                "To exit application (progress will not be saved) write '#exit' or '#e'\n" +
                "To return back in creation write '#back' or '#b'\n" +
                "To save unfinished work and exit creation write '#done' or '#d'\n\n"
                );

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

                //End case
                default:
                    Console.WriteLine("Character creation is finished!");
                    return;

            }
        }

        internal override bool React(string input)
        {
            input = input.Trim();

            //Checking for control inputs
            if (input.Length >= 1 && input[0] == '#')
            switch (input)
            {
                case "#exit":
                case "#e":
                    cm.Exit();
                    return true;

                case "#back":
                case "#b":
                    if(phase >= 1)
                    {
                        --phase;
                    }   
                    return true;

                case "#done":
                case "#d":
                    return End();

                default:
                    return true;
            }

            switch (phase)
            {
                case 0:
                    newCharacter.name = input;
                    break;

                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                EditBaseAbilityScore(phase - 1, input);
                    break;

                default:
                    return End();
            }
            ++phase;
            return true;
        }

        private bool End()
        {
            cm.AddCharacter(newCharacter);

            if (parentMenu == null)
            {
                return false;
            }
            cm.activeMenu = parentMenu;

            //TODO - clear memory

            return true;
        }

        private bool EditBaseAbilityScore(int index, string input)
        {
            int value;

            if(!int.TryParse(input, out value))
            {
                return false;
            }

            return newCharacter.EditBaseAbilityScore(index, value);
        }
    }
}
