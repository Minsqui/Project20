using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project20
{
    internal class Character
    {
        internal record LevelInClass { public GameClass gameClass; public int level; }

        public string name { get; private set; }
        bool race { get; set; }
        List<LevelInClass> classes { get; set; }
        int level
        {
            get { return classes.Sum(gameClass => gameClass.level); }
        }

        int maxHP { get; set; }
        int currentHP { get; set; }


    }
}
