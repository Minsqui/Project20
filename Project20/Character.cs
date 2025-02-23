using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project20
{
    public class Character
    {
        public record LevelInClass { public GameClass gameClass; public int level; }

        public string name { get; set; }
        static public string nameBaseValue = "Unnamed character";
        public int[] baseAbiltyScore { get; set; } = [10, 10, 10, 10, 10, 10];
        public GameRace race { get; set; }
        public List<LevelInClass> classes { get; set; } = new List<LevelInClass>();
        public int level
        {
            get
            {
                if (classes == null)
                {
                    return 0;
                }

                return classes.Sum(gameClass => gameClass.level);
            }
        }

        public int maxHP { get; set; } = 0;
        public int currentHP { get; set; } = 0;

        public static ImmutableArray<String> abilityNames { get; } = ["STR", "DEX", "CON", "INT", "WIS", "CHA"];
        public string GetName()
        {
            if (name == null || name.Trim().Length == 0)
            {
                return nameBaseValue;
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

        public static int GetAbilityIndex(string abilityName)
        {
            return abilityNames.IndexOf(abilityName.ToUpper());
        }

        public int GetAbilityModifier(int index)
        {
            int value = baseAbiltyScore[index];
            if (race != null && race.abilityScore != null)
            {
                value += race.abilityScore[index];
            }
            return CountModifier(value);
        }

        public int AbilityCheck(int index)
        {
            return Die.Roll("1d20") + GetAbilityModifier(index);
        }
    }
}
