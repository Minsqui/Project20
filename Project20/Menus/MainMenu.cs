using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project20
{
    /// <summary>
    /// Root menu. Has no parent menu.
    /// </summary>
    internal class MainMenu : Menu
    {
        internal MainMenu(ConsoleManager cm) :base(cm)
        {
            this.name = "Main menu";
            this.cm = cm;
            this.childMenus = [new ChooseCharacterMenu(this.cm,this)];
        }
    }
}
