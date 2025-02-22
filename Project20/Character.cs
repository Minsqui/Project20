using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project20
{
    internal class Character
    {
        internal record LevelInClass { public GameClass gameClass; public int level; }

        internal string name { get; set; }
        internal int[] baseAbiltyScore { get; set; }

        bool race { get; set; }
        List<LevelInClass> classes { get; set; }
        int level
        {
            get { return classes.Sum(gameClass => gameClass.level); }
        }

        int maxHP { get; set; }
        int currentHP { get; set; }

        public static ImmutableArray<String> abilityNames { get; } = ["STR", "DEX", "CON", "INT", "WIS", "CHA"];

        public Character()
        {
            baseAbiltyScore = new int[6];
        }

        public string GetName()
        {
            if (name == null || name.Trim().Length == 0)
            {
                return "Unnamed character";
            }
            return name;
        }

        internal bool EditBaseAbilityScore(int index, int value)
        {
            if (value < 0)
            {
                return false;
            }

            baseAbiltyScore[index] = value;
            return true;
        }

        static int CountModifier(int value)
        {
            return (int)Math.Floor(((float)value - 10) / 2);
        }

        internal int GetModifier(int index)
        {
            return CountModifier(baseAbiltyScore[index]);
        }
    }
}
