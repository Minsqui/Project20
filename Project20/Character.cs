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
        internal string filename;
        public record LevelInClass { public GameClass gameClass; public int level; }

        public string name { get; set; }
        static public string nameBaseValue = "Unnamed character";
        public int[] baseAbiltyScore { get; set; } = [10, 10, 10, 10, 10, 10];
        public int[] proficiencies { get; set; } = new int[18];
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

        public static ImmutableArray<string> abilityNames { get; } = ["STR", "DEX", "CON", "INT", "WIS", "CHA"];
        public static ImmutableArray<string> skillNames { get; } = [
            "acrobatics", "animal handling", "arcana", "athletics","deception",
            "history", "insight", "intimidation", "investigation", "medicine",
            "nature", "perception", "preformance", "persuasion", "religion",
            "sleight of hand", "stealth", "survival"
            ];
        public static ImmutableArray<int> skillAbility { get; } = [
            1,4,3,0,5,3,4,5,3,4,3,4,5,5,3,1,1,4
            ];

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

        internal bool EditBaseAbilityScore(string abilityName, int value)
        {
            int index = GetAbilityIndex(abilityName);
            if (index < 0)
            {
                return false;
            }

            return EditBaseAbilityScore(index, value);
        }

        internal void EditName(string newName)
        {
            name = newName.Trim();
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

        public int GetSkillModifier(int index)
        {
            return GetAbilityModifier(skillAbility[index]) + GetProficiency() * proficiencies[index];
        }

        public int GetSkillModifier(string skillName)
        {
            int index = GetSkillIndex(skillName);
            if (index < 0)
            {
                return 0;
            }
            return GetSkillModifier(GetSkillIndex(skillName));
        }

        public int GetProficiency()
        {
            return (int)(Math.Ceiling((Convert.ToSingle(level)) / 4)) + 1;
        }

        public int AbilityCheck(int index)
        {
            return Die.Roll("1d20") + GetAbilityModifier(index);
        }

        public int? AbilityCheck(string abilityName)
        {
            int index = GetAbilityIndex(abilityName);
            if (index < 0)
            {
                return null;
            }
            return AbilityCheck(index);
        }
        
        static int CountModifier(int value)
        {
            return (int)Math.Floor(((float)value - 10) / 2);
        }

        public static int GetAbilityIndex(string abilityName)
        {
            return abilityNames.IndexOf(abilityName.ToUpper());
        }

        public static int GetSkillIndex(string skillName)
        {
            return skillNames.IndexOf(skillName.ToLower());
        }

    }
}
