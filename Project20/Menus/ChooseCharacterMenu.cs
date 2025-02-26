using Project20.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project20
{
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

        internal override void Show()
        {
            int i = 0;

            if (cm.characters == null || cm.characters.Count == 0)
            {
                Console.WriteLine("No characters found.");
            }
            else
            {
                for (; i < cm.characters.Count; ++i)
                {
                    Console.WriteLine(i + ": " + cm.characters[i].GetName());
                }
            }

            Console.Write(
                "\n"+ i + ": New character\n" +
                (i + 1) + ": Go back\n" +
                (i + 2) + ": Exit\n"
                );
        }

        internal override void React(string input)
        {
            int index;

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
                cm.activeMenu = new CharacterMenu(cm, this, cm.characters[index]);
            }
            return;

        }
    }
}
