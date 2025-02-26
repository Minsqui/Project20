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
        protected Menu? parentMenu;
        protected Menu[]? childMenus;
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

                if (parentMenu != null)
                {
                    sum += 1; //+1 for go back option
                }
                return sum;
            }
        }

        internal Menu(ConsoleManager cm, Menu? parent = null, string name = "Unnamed menu")
        {
            this.name = name;
            this.cm = cm;
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

            if (parentMenu != null)
            {
                Console.WriteLine((i) + ": Go back");
                ++i;
            }
            Console.WriteLine(i + ": Exit");           
        }

        internal virtual void React(string input)
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
            else if (index == optionsLength - 2 && parentMenu != null)
            {
                cm.activeMenu = parentMenu;
            }
            //Go to sub menu option
            else
            {
                if (childMenus == null)
                {
                    throw new NullReferenceException();
                }

                cm.activeMenu = childMenus[index];
            }                    
            return;
        }

        virtual internal string GetName()
        {
            return name;
        }
    }
}
