using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Project20
{
    /// <summary>
    /// Class that represents character from DnD 5e 2014 ruleset. 
    /// </summary>
    public class Character
    {
        internal string filename;
        public string name { get; set; }

        /// <summary>
        /// Array of base ability score.
        /// To know index of ability, use GetAbilityIndex().  
        /// </summary>
        public int[] abilityScore { get; set; } = [10, 10, 10, 10, 10, 10];

        /// <summary>
        /// Represents skill proficiencies.
        /// Number in given position represents proficiency multiplier.
        /// 0 means not profiecient, 1 means profiecient, 2 means expertise.
        /// To know index of skill, use GetSkillIndex().  
        /// </summary>
        public int[] proficiencies { get; set; } = new int[numberOfSkills];

        /// <summary>
        /// Represents saving throw proficiencies.
        /// Number in given position represents proficiency multiplier.
        /// 0 means not profiecient, 1 means profiecient, 2 means expertise.
        /// To know index of ability, use GetAbilityIndex().  
        /// </summary>
        public int[] saveThrows { get; set; } = new int[numberOfAbilities];
        public string raceID { get; set; }
        public string classID { get; set; }
        public int level { get; set; } = 1;

        public int maxHP { get; set; } = 0;
        public int currentHP { get; set; } = 0;

        static public string nameBaseValue = "Unnamed character";
        private static int numberOfAbilities = 6;
        private static int numberOfSkills = 18;
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

        public Character()
        {
            filename = GetFilename();
            name = string.Empty;
        }

        public string GetFilename()
        {
            string fileName;

            //Checking if character has name
            if (GetName() == Character.nameBaseValue)
            {
                //Generating file name
                fileName = Character.nameBaseValue + $@"{DateTime.Now.Ticks}";
            }
            else
            {
                fileName = GetName();
            }

            return fileName + ".json";
        }

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
        /// Adds given bonuses to ability score.
        /// </summary>
        /// <param name="abilityBonus">Array with bonuses to ability</param>
        internal void AddAbilityScore(int[] abilityBonus)
        {
            if (abilityBonus == null || abilityBonus.Length != numberOfAbilities) return;

            for (int i = 0; i < numberOfAbilities; ++i)
            {
                abilityScore[i] += abilityBonus[i];
            }
        }

        /// <summary>
        /// Method that edits ability score.
        /// </summary>
        /// <param name="index">Index of edited ability.</param>
        /// <param name="score">New score of the ability.</param>
        /// <returns>If the ability edit was successful.</returns>
        internal bool EditAbilityScore(int index, int score)
        {
            if (index < 0 || index >= numberOfAbilities)

            if (score < 0) return false;

            abilityScore[index] = score;
            return true;
        }

        /// <summary>
        /// Method that edits ability score.
        /// </summary>
        /// <param name="abilityName">Name of the edited ability.</param>
        /// <param name="score">New score of the ability.</param>
        /// <returns>If the ability edit was successful.</returns>
        internal bool EditAbilityScore(string abilityName, int score)
        {
            int index = GetAbilityIndex(abilityName);

            if (index < 0) return false;

            return EditAbilityScore(index, score);
        }

        /// <summary>
        /// Edits level
        /// </summary>
        /// <param name="newLevel">New level value, must be between 1 and 20</param>
        /// <returns>Returns false if newLevel not in rule bounds.</returns>
        internal bool EditLevel(int newLevel)
        {
            if (newLevel > 20 || newLevel < 1) return false;

            level = newLevel;
            return true;
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
        /// Changes saveThrows proficiency multiplier of given ability to given multiplier.
        /// </summary>
        /// <param name="index">Index of the ability.</param>
        /// <param name="multiplier">New multiplier of save throw proficiency.</param>
        /// <returns>If the save throw edit was successful.</returns>
        internal bool EditSaveThrow(int index, int multiplier)
        {
            if (index < 0 || index >= numberOfAbilities) return false;

            if (multiplier < 0) return false;

            this.saveThrows[index] = multiplier;
            return true;
        }

        /// <summary>
        /// Changes saveThrows proficiency multiplier of given ability to given multiplier.
        /// </summary>
        /// <param name="abilityName">Name of the ability.</param>
        /// <param name="multiplier">New multiplier of save throw proficiency.</param>
        /// <returns>If the save throw edit was successful.</returns>
        internal bool EditSaveThrow(string abilityName, int multiplier)
        {
            int index = GetAbilityIndex(abilityName);

            if (index < 0) return false;

            return EditSaveThrow(index, multiplier);
        }

        /// <summary>
        /// Changes saveThrows proficiencies array to given array
        /// </summary>
        /// <param name="saveThrows">New saveThrow proficiencies array</param>
        internal void EditSaveThrow(int[] saveThrows)
        {
            if (saveThrows == null || saveThrows.Length != numberOfAbilities) return;

            this.saveThrows = saveThrows;
        }

        /// <summary>
        /// Method that edits skill score.
        /// </summary>
        /// <param name="index">Index of edited skill.</param>
        /// <param name="score">New score of the skill.</param>
        /// <returns>If the skill edit was successful.</returns>
        internal bool EditSkillProficiency(int index, int score)
        {
            if (index < 0 || index >= numberOfSkills) return false;

            if (score < 0) return false;

            proficiencies[index] = score;
            return true;
        }

        /// <summary>
        /// Method that edits skill score.
        /// </summary>
        /// <param name="skillName">Name of the edited skill.</param>
        /// <param name="score">New score of the skill.</param>
        /// <returns>If the skill edit was successful.</returns>
        internal bool EditSkillProficiency(string skillName, int score)
        {
            int index = GetSkillIndex(skillName);

            if (index < 0) return false;

            return EditSkillProficiency(index, score);
        }

        /// <summary>
        /// Method that returns character's given ability modifier.
        /// </summary>
        /// <param name="index">Index od the ability.</param>
        /// <returns>Given ability modifier.</returns>
        public int GetAbilityModifier(int index)
        {
            if (index < 0 || index >= numberOfAbilities) throw new IndexOutOfRangeException();

            return CountModifier(abilityScore[index]);
        }

        /// <summary>
        /// Method that returns character's given ability save modifier.
        /// </summary>
        /// <param name="index">Index od the ability.</param>
        /// <returns>Given ability modifier.</returns>
        public int GetSaveModifier(int index)
        {
            if (index < 0 || index >= numberOfAbilities) throw new IndexOutOfRangeException();

            return GetAbilityModifier(index) + GetProficiency() * saveThrows[index];
        }

        /// <summary>
        /// Method that returns character's given skill modifier.
        /// </summary>
        /// <param name="index">Index od the skill.</param>
        /// <returns>Given skill modifier.</returns>
        public int GetSkillModifier(int index)
        {
            if (index < 0 || index >= numberOfSkills) throw new IndexOutOfRangeException();

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
        public int CheckAbility(int index)
        {
            if (index < 0 || index >= numberOfAbilities) throw new IndexOutOfRangeException();

            return Die.Roll("1d20") + GetAbilityModifier(index);
        }

        /// <summary>
        /// Makes an ability check, rolls 1d20 and adds modifier.
        /// </summary>
        /// <param name="abilityName">Name of the ability.</param>
        /// <returns>Value of the check. Null if given ability name does not exist.</returns>
        public int? CheckAbility(string abilityName)
        {
            int index = GetAbilityIndex(abilityName);
            if (index < 0)
            {
                return null;
            }
            return CheckAbility(index);
        }

        /// <summary>
        /// Makes an save throw check, rolls 1d20 adds modifier and save throw proficiency. 
        /// </summary>
        /// <param name="abilityName">Name of the ability.</param>
        /// <returns>Value of the check. Null if given ability name does not exist.</returns>
        public int? CheckSave(string abilityName)
        {
            int? abilityCheck = CheckAbility(abilityName);

            if (abilityCheck == null) return null;

            return abilityCheck + GetProficiency() * saveThrows[GetAbilityIndex(abilityName)];
        }

        /// <summary>
        /// Makes a skill check, rolls 1d20 and adds modifier.
        /// </summary>
        /// <param name="index">Index of the skill.</param>
        /// <returns>Value of the check.</returns>
        public int CheckSkill(int index)
        {
            if (index < 0 || index >= numberOfSkills) throw new IndexOutOfRangeException();

            return Die.Roll("1d20") + GetSkillModifier(index);
        }

        /// <summary>
        /// Makes a skill check, rolls 1d20 and adds modifier.
        /// </summary>
        /// <param name="skillName">Name of the skill.</param>
        /// <returns>Value of the check. Null if given skill name does not exist.</returns>
        public int? CheckSkill(string skillName)
        {
            int index = GetSkillIndex(skillName);
            if (index < 0)
            {
                return null;
            }
            return CheckSkill(index);
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
