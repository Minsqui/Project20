using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI.Menus
{
    /// <summary>
    /// Menu where users chooses character and can choose to create new one.
    /// </summary>
    internal class ChooseCharacterMenu:Menu
    {
        override protected int optionsLength
        {
            get
            {
                int sum = 3; //3 for exit, go back and new character options

                if (cm.characters != null)
                {
                    sum += cm.characters.Count;
                }

                return sum;
            }
        }

        internal ChooseCharacterMenu(ConsoleManager cm, Menu? parent) : base(cm, parent)
        {
            this.name = "Choose Character";
            this.cm = cm;
            this.parentMenu = parent;
            this.childMenus = [];
        }

        /// <summary>
        /// Show all the characters + New character, Go back and Exit options.
        /// </summary>
        internal override void Show()
        {
            int i = 0;

            Core.Character[] chArr = cm.characters.Select(x => x.Value).ToArray();

            if (cm.characters == null || cm.characters.Count == 0)
            {
                Console.WriteLine("No characters found.");
            }
            else
            {
                for (; i < cm.characters.Count; ++i)
                {
                    Console.WriteLine(i + ": " + chArr[i].Name);
                }
            }

            Console.Write(
                "\n"+ i + ": New character\n" +
                (i + 1) + ": Go back\n" +
                (i + 2) + ": Exit\n"
                );
        }

        /// <summary>
        /// Reactions to user input.
        /// </summary>
        /// <param name="input">Input of the user.</param>
        /// <exception cref="NullReferenceException"></exception>
        internal override void React(string input)
        {
            int index;

            Core.Character[] chArr = cm.characters.Select(x => x.Value).ToArray();

            //Checking if input is number + converting input to number
            if (int.TryParse(input, out index) == false)
            {
                return;
            }

            //Checking if given index is one of menu options
            if (index < 0 || index >= optionsLength)
            {
                return;
            }

            //Last option - Exit app
            if (index == optionsLength - 1)
            {
                cm.Exit();
            }
            //Pre-last option - Go back
            else if (index == optionsLength - 2)
            {
                if (parentMenu == null)
                {
                    throw new NullReferenceException();
                }
                cm.activeMenu = parentMenu;
            }
            //Pre-pre-last option - Create new character
            else if (index == optionsLength - 3)
            {
                cm.activeMenu = new CharacterCreationMenu(cm, this);
            }
            //Choose character option
            else
            {
                cm.activeMenu = new CharacterMenu(cm, this, chArr[index]);
            }
            return;

        }
    }
}
