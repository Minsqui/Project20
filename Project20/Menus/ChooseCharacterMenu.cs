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

        internal override bool React(string input)
        {
            int index;

            if (int.TryParse(input, out index) == false)
            {
                return false;
            }

            if (index < 0 || index >= optionsLength)
            {
                return false;
            }

            if (index == optionsLength - 1)
            {
                cm.Exit();
            }
            else if (index == optionsLength - 2)
            {
                cm.activeMenu = parentMenu;
            }
            else if (index == optionsLength - 3)
            {
                cm.activeMenu = new CharacterCreationMenu(cm, this);
            }
            else
            {
                cm.activeMenu = new CharacterMenu(cm, this, cm.characters[index]);
            }
            return true;

        }
    }
}
