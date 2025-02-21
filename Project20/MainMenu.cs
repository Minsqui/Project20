using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project20
{
    internal class MainMenu : Menu
    {
        public MainMenu(ConsoleManager cm) :base(cm)
        {
            this.name = "Main menu";
            this.cm = cm;
            this.isMainMenu = true;
            this.childMenus = [];
        }
    }
}
