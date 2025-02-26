using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project20
{
    /// <summary>
    /// Class that represents class from DnD 5e 2014 ruleset.
    /// </summary>
    public class GameClass
    {
        record Feature { string text;  int level; }

        Feature[] features;
    }
}
