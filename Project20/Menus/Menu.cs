using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Project20
{
    internal class Menu
    {
        internal string name;
        protected bool isMainMenu;
        protected Menu parentMenu;
        protected Menu[] childMenus;
        protected ConsoleManager cm;

        protected virtual int optionsLength
        {
            get
            {
                int sum = 1; //1 for exit

                if (childMenus != null)
                {
                    sum += childMenus.Length;
                }

                if (isMainMenu == false)
                {
                    sum += 1; //+1 for go back option
                }
                return sum;
            }
        }

        public Menu(ConsoleManager cm, Menu parent = null, string name = "Unnamed menu", bool isMainMenu = false)
        {
            this.name = name;
            this.cm = cm;
            this.isMainMenu = isMainMenu;
        }

        internal virtual void Show()
        {
            int i = 0;

            if (childMenus != null)
            {
                for (; i < childMenus.Length; ++i)
                {
                    Console.WriteLine(i + ": " + childMenus[i].name);
                }
            }

            if (isMainMenu == false)
            {
                Console.WriteLine((i) + ": Go back");
                ++i;
            }
            Console.WriteLine(i + ": Exit");           
        }

        internal virtual bool React(string input)
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
            else if (index == optionsLength - 2 && isMainMenu == false)
            {
                cm.activeMenu = parentMenu;
            }
            else
            {
                cm.activeMenu = childMenus[index];
            }                    

            Console.Clear();
            return true;
        }
    }
}
