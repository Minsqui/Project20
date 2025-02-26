using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project20
{
    /// <summary>
    /// Class that represents character from DnD 5e 2014 ruleset. 
    /// </summary>
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

        /// <summary>
        /// Method that returns character's name.
        /// </summary>
        /// <returns>Character's name.</returns>
        public string GetName()
        {
            if (name == null || name.Trim().Length == 0)
            {
                return nameBaseValue;
            }
            return name;
        }

        /// <summary>
        /// Method that edits base ability score.
        /// </summary>
        /// <param name="index">Index of edited ability.</param>
        /// <param name="score">New score of the ability.</param>
        /// <returns>If the ability edit was successful.</returns>
        internal bool EditBaseAbilityScore(int index, int score)
        {
            if (score < 0)
            {
                return false;
            }

            baseAbiltyScore[index] = score;
            return true;
        }

        /// <summary>
        /// Method that edits base ability score.
        /// </summary>
        /// <param name="abilityName">Name of the edited ability.</param>
        /// <param name="score">New score of the ability.</param>
        /// <returns>If the ability edit was successful.</returns>
        internal bool EditBaseAbilityScore(string abilityName, int score)
        {
            int index = GetAbilityIndex(abilityName);
            if (index < 0)
            {
                return false;
            }

            return EditBaseAbilityScore(index, score);
        }

        /// <summary>
        /// Edits name.
        /// </summary>
        /// <param name="newName"></param>
        internal void EditName(string newName)
        {
            name = newName.Trim();
        }

        /// <summary>
        /// Method that returns character's given ability modifier.
        /// </summary>
        /// <param name="index">Index od the ability.</param>
        /// <returns>Given ability modifier.</returns>
        public int GetAbilityModifier(int index)
        {
            int value = baseAbiltyScore[index];
            if (race != null && race.abilityScore != null)
            {
                value += race.abilityScore[index];
            }
            return CountModifier(value);
        }

        /// <summary>
        /// Method that returns character's given skill modifier.
        /// </summary>
        /// <param name="index">Index od the skill.</param>
        /// <returns>Given skill modifier.</returns>
        public int GetSkillModifier(int index)
        {
            return GetAbilityModifier(skillAbility[index]) + GetProficiency() * proficiencies[index];
        }

        /// <summary>
        /// Method that returns character's given skill modifier.
        /// </summary>
        /// <param name="skillName">Name of the skill.</param>
        /// <returns>Given skill modifier.</returns>
        public int GetSkillModifier(string skillName)
        {
            int index = GetSkillIndex(skillName);
            if (index < 0)
            {
                return 0;
            }
            return GetSkillModifier(GetSkillIndex(skillName));
        }

        /// <summary>
        /// Method that calculates proficiency bonus from character's level.
        /// </summary>
        /// <returns>Proficiency bonus of the character.</returns>
        public int GetProficiency()
        {
            return (int)(Math.Ceiling((Convert.ToSingle(level)) / 4)) + 1;
        }

        /// <summary>
        /// Makes an ability check, rolls 1d20 and adds modifier.
        /// </summary>
        /// <param name="index">Index of the ability.</param>
        /// <returns>Value of the check.</returns>
        public int AbilityCheck(int index)
        {
            return Die.Roll("1d20") + GetAbilityModifier(index);
        }

        /// <summary>
        /// Makes an ability check, rolls 1d20 and adds modifier.
        /// </summary>
        /// <param name="abilityName">Name of the ability.</param>
        /// <returns>Value of the check. Null if given ability name does not exist.</returns>
        public int? AbilityCheck(string abilityName)
        {
            int index = GetAbilityIndex(abilityName);
            if (index < 0)
            {
                return null;
            }
            return AbilityCheck(index);
        }
        
        /// <summary>
        /// Counts modifier from given value.
        /// </summary>
        /// <param name="value">Value from which the modifier is counted.</param>
        /// <returns></returns>
        static int CountModifier(int value)
        {
            return (int)Math.Floor(((float)value - 10) / 2);
        }

        /// <summary>
        /// From given ability name returns the ability index in the ability array.
        /// </summary>
        /// <param name="abilityName">Name of the ability.</param>
        /// <returns>Index in ability array. -1 if given ability name does not exist.</returns>
        public static int GetAbilityIndex(string abilityName)
        {
            return abilityNames.IndexOf(abilityName.ToUpper());
        }

        /// <summary>
        /// From given skill name returns the skill index in the skill array.
        /// </summary>
        /// <param name="skillName">Name of the skill.</param>
        /// <returns>Index in skill array. -1 if given skill name does not exist.</returns>
        public static int GetSkillIndex(string skillName)
        {
            return skillNames.IndexOf(skillName.ToLower());
        }

    }
}
