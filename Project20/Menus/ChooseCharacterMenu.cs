using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project20
{
    internal class ChooseCharacterMenu:Menu
    {
        protected virtual int optionsLength
        {
            get
            {
                int sum = 2; //2 for exit and go back options

                if (cm.characters != null)
                {
                    sum += cm.characters.Count;
                }

                return sum;
            }
        }

        public ChooseCharacterMenu(ConsoleManager cm, Menu parent) : base(cm, parent)
        {
            this.name = "Choose Character";
            this.cm = cm;
            this.isMainMenu = false;
            this.parentMenu = parent;
            this.childMenus = [];
        }

        internal override void Show()
        {
            int i = 0;

            if (cm.characters == null || cm.characters.Count == 0)
            {
                Console.WriteLine("No characters found.\n");
            }
            else
            {
                for (; i < cm.characters.Count; ++i)
                {
                    Console.WriteLine(i + ": " + cm.characters[i].name);
                }
            }

            if (isMainMenu == false)
            {
                Console.WriteLine((i) + ": Go back");
                ++i;
            }
            Console.WriteLine(i + ": Exit");
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
            else
            {
                //cm.activeMenu = cm.characters[index]; -TODO create ShowCharacterMenu
            }

            Console.Clear();
            return true;

        }
    }
}
