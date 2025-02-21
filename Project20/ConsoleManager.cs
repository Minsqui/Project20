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
                Console.WriteLine("To choose from given options write index of option.");

                if (activeMenu == null)
                {
                    throw new Exception("Active menu does not exist (activeMenu == null)");
                }

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

        /*
        List<Character> characters { get; set; }
        Action<ConsoleCharacterManager>[] mainMenu = [ShowCharacterMenu,Exit];

        public ConsoleCharacterManager()
        {
            characters = new List<Character>();
        }

        static void ShowCharacters(ConsoleCharacterManager ccm)
        {
            if (ccm.characters.Count == 0)
            {
                Console.WriteLine("No characters found");
                return;
            }

            for (int i = 0; i < ccm.characters.Count; i++)
            {
                Console.WriteLine(i + ": " + ccm.characters[i].name);
            }
        }

        static void ShowCharacterMenu(ConsoleCharacterManager ccm)
        {
            ShowCharacters(ccm);
            Console.ReadKey();
        }

        static void Exit(ConsoleCharacterManager ccm)
        {
            Environment.Exit(0);
        }

        public void Run()
        {
            string input;
            int index;

            while (true)
            {
                Console.WriteLine("To choose from given options write index of option.");

                //Write menu options
                for (int i = 0; i < mainMenu.Length; i++)
                {
                    Console.WriteLine(i + ": " + mainMenu[i].Method.Name);
                }

                input = Console.ReadLine();

                if (input == null)
                {
                    continue;
                }

                if (int.TryParse(input, out index))
                {
                    if (index >= 0 && index < mainMenu.Length)
                    {
                        Console.Clear();
                        mainMenu[index](this);
                    }
                }

                Console.Clear();
            }
            
        }
        */
    }
}
