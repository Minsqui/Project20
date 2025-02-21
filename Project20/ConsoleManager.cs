using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project20
{
    internal class ConsoleManager
    {
        internal Menu activeMenu;
        internal List<Character> characters;

        public ConsoleManager()
        {
            Menu mainMenu = new MainMenu(this);
            this.activeMenu = mainMenu;
        }

        public void Run()
        {
            string input;

            while (true)
            {
                if (activeMenu == null)
                {
                    throw new Exception("Active menu does not exist (activeMenu == null)");
                }

                Console.WriteLine(activeMenu.name + "\n");

                activeMenu.Show();

                input = Console.ReadLine();

                if (input == null)
                {
                    continue;
                }

                activeMenu.React(input);

                Console.Clear();
            }
        }

        internal void Exit()
        {
            Environment.Exit(0);
        }
    }
}
