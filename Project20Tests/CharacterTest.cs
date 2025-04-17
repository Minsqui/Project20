using Microsoft.ApplicationInsights.Channel;

namespace Project20Tests
{
    [TestClass]
    public sealed class CharacterTest
    {
        #region Name
        const string UNNAMED_CHARACTER_NAME = "Unnamed character";

        [TestMethod]
        public void NoName_Name_UnnamedCharacter()
        {
            Project20.Character character = new();
            Assert.AreEqual(UNNAMED_CHARACTER_NAME, character.Name);
        }

        [TestMethod]
        public void WhiteSpaceName_Name_UnnamedCharacter()
        {
            Project20.Character character = new();
            character.Name = "\n \t";
            Assert.AreEqual(UNNAMED_CHARACTER_NAME, character.Name);
        }

        [TestMethod]
        public void ValidName_Name_NamedCharacter()
        {
            string expected = "Unit von Test";
            Project20.Character character = new();
            character.Name = expected;
            Assert.AreEqual(expected, character.Name);
        }
        #endregion

        #region AddAbilityScore
        [TestMethod]
        public void ValidAddition_AddAbilityScore_ValidScore()
        {
            int[] addition = [2, -3, 0, 0, 0, 1];
            int[] expected = [12, 7, 10, 10, 10, 11];
            bool failed = false;

            Project20.Character character = new();

            failed = !character.AddAbilityScore(addition);

            for (int i = 0; i < 6; ++i)
            {
                if (character.abilityScore[i] != expected[i])
                {
                    failed = true;
                    break;
                }
            }

            Assert.IsFalse(failed);
        }

        [TestMethod]
        public void InvalidLength_AddAbilityScore_False()
        {
            int[] addition = [0, 0, 8];
            Project20.Character character = new();

            Assert.IsFalse(character.AddAbilityScore(addition));
        }

        [TestMethod]
        public void Null_AddAbilityScore_False()
        {
            int[] addition = null;
            Project20.Character character = new();

            Assert.IsFalse(character.AddAbilityScore(addition));
        }
        #endregion

        #region EditAbilityScore
        [TestMethod]
        public void InvalidName_EditAbilityScore_False()
        {
            Project20.Character character = new();

            Assert.IsFalse(character.EditAbilityScore("not", 12));
        }

        [TestMethod]
        public void InvalidIndex_EditAbilityScore_False()
        {
            Project20.Character character = new();

            Assert.IsFalse(character.EditAbilityScore(6, 12));
        }

        [DataTestMethod]
        [DataRow("STR", 15, 0)]
        [DataRow("dex", 7, 1)]
        [DataRow("Con", 8, 2)]
        [DataRow("iNt", 12, 3)]
        [DataRow("wiS", 18, 4)]
        [DataRow("Cha", 14, 5)]
        public void ValidInput_EditAbilityScore_True(string name, int value, int expectedIndex)
        {
            Project20.Character character = new();

            character.EditAbilityScore(name, value);

            Assert.AreEqual(value, character.abilityScore[expectedIndex]);
        }
        #endregion

        #region EditLevel
        [DataTestMethod]
        [DataRow(-4)]
        [DataRow(0)]
        [DataRow(21)]
        public void InvalidLevel_EditLevel_False(int level)
        {
            Project20.Character character = new();

            Assert.IsFalse(character.EditLevel(level));
        }
        #endregion

        #region EditSaveThrow
        [TestMethod]
        public void InvalidName_EditSaveThrow_False()
        {
            Project20.Character character = new();

            Assert.IsFalse(character.EditSaveThrow("not", 1));
        }

        [TestMethod]
        public void InvalidIndex_EditSaveThrow_False()
        {
            Project20.Character character = new();

            Assert.IsFalse(character.EditSaveThrow(6, 1));
        }

        [DataTestMethod]
        [DataRow("STR", 1, 0)]
        [DataRow("dex", 2, 1)]
        [DataRow("Con", 1, 2)]
        [DataRow("iNt", 1, 3)]
        [DataRow("wiS", 1, 4)]
        [DataRow("Cha", 1, 5)]
        public void ValidInput_EditSaveThrow_True(string name, int value, int expectedIndex)
        {
            Project20.Character character = new();

            character.EditSaveThrow(name, value);

            Assert.AreEqual(value, character.saveThrows[expectedIndex]);
        }

        [TestMethod]
        public void InvalidArrayLength_EditSaveThrow_False()
        {
            int[] saveThrows = [0, 0, 0];
            Project20.Character character = new();

            Assert.IsFalse(character.EditSaveThrow(saveThrows));
        }

        [TestMethod]
        public void NullArray_EditSaveThrow_False()
        {
            int[] saveThrows = null;
            Project20.Character character = new();

            Assert.IsFalse(character.EditSaveThrow(saveThrows));
        }

        [TestMethod]
        public void ValidArray_EditSaveThrow_True()
        {
            int[] saveThrows = [0, 1, 0, 2, 1, 0];
            Project20.Character character = new();
            bool failed = false;

            failed = !character.EditSaveThrow(saveThrows);

            for (int i = 0; i < 6; ++i)
            {
                if (character.saveThrows[i] != saveThrows[i])
                {
                    failed = true;
                    break;
                }
            }

            Assert.IsFalse(failed);
        }
        #endregion

        #region EditSkillProficiency
        [TestMethod]
        public void InvalidName_EditSkillProficiency_False()
        {
            Project20.Character character = new();

            Assert.IsFalse(character.EditSkillProficiency("programming", 2));
        }

        [TestMethod]
        public void InvalidIndex_EditSkillProficiency_False()
        {
            Project20.Character character = new();

            Assert.IsFalse(character.EditSkillProficiency(42, 1));
        }

        [DataTestMethod]
        [DataRow("acrobatics", 1, 0)]
        [DataRow("animal handling", 1, 1)]
        [DataRow("ARCANA", 1, 2)]
        [DataRow("athletics", 1, 3)]
        [DataRow("deception", 1, 4)]
        [DataRow("history", 1, 5)]
        [DataRow("insight", 1, 6)]
        [DataRow("intimidation", 1, 7)]
        [DataRow("investigation", 1, 8)]
        [DataRow("medicine", 1, 9)]
        [DataRow("nature", 1, 10)]
        [DataRow("perception", 1, 11)]
        [DataRow("preformance", 1, 12)]
        [DataRow("perSUASion", 1, 13)]
        [DataRow("religion", 1, 14)]
        [DataRow("sleight of hand", 1, 15)]
        [DataRow("stealth", 1, 16)]
        [DataRow("survival", 1, 17)]
        public void ValidInput_EditSkillProficiency_True(string name, int value, int expectedIndex)
        {
            Project20.Character character = new();

            character.EditSkillProficiency(name, value);

            Assert.AreEqual(value, character.proficiencies[expectedIndex]);
        }
        #endregion

        #region GetAbilityModifier
        [TestMethod]
        public void ValidInput_GetAbilityModifier()
        {
            int index = 0;
            int result;

            int expected = 1;
            int value = 12;

            Project20.Character character = new();

            character.abilityScore[index] = 12;

            result = character.GetAbilityModifier(index);

            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(6)]
        public void OutOfRangeIndex_GetAbilityModifier_ThrowException(int index)
        {
            Project20.Character character = new();

            Assert.ThrowsException<IndexOutOfRangeException>(() => character.GetAbilityModifier(index));
        }
        #endregion

        #region GetSaveModifier
        [TestMethod]
        public void ValidInput_GetSaveModifier()
        {
            int index = 0;
            int result;

            int expected = 1;
            int value = 12;

            Project20.Character character = new();

            character.abilityScore[index] = 12;

            Assert.AreEqual(expected, character.GetSaveModifier(index));
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(6)]
        public void OutOfRangeIndex_GetSaveModifier_ThrowException(int index)
        {
            Project20.Character character = new();

            Assert.ThrowsException<IndexOutOfRangeException>(() => character.GetSaveModifier(index));
        }
        #endregion

        #region GetSkillModifier
        [TestMethod]
        public void ValidAcrobatics_GetSkillModifier()
        {
            int index = 0;
            int prof = 1;
            int dex = 12;
            int dexPos = 1;
            int level = 1;

            int expected = 3;

            Project20.Character character = new();
            character.proficiencies[index] = prof;
            character.level = level;
            character.abilityScore[dexPos] = dex;

            Assert.AreEqual(expected, character.GetSkillModifier(index));
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(18)]
        public void OutOfRangeIndex_GetSkillModifier(int index)
        {
            Project20.Character character = new();

            Assert.ThrowsException<IndexOutOfRangeException>(() => character.GetSkillModifier(index));
        }
        #endregion

        [DataTestMethod]
        [DataRow(1, 2)]
        [DataRow(4,2)]
        [DataRow(5, 3)]
        [DataRow(8, 3)]
        [DataRow(9, 4)]
        [DataRow(12, 4)]
        [DataRow(13, 5)]
        [DataRow(16, 5)]
        [DataRow(17, 6)]
        [DataRow(20, 6)]
        public void GetProficiency(int level, int expected)
        {
            Project20.Character character = new();
            character.level = level;

            Assert.AreEqual(expected, character.GetProficiency());
        }

        #region CheckAbility
        [TestMethod]
        public void ValidIndex_CheckAbility()
        {
            int numberOfTests = 5;
            bool failed = false;
            int abilityIndex = 0;
            int abilityScore = 14;
            int expectedBonus = 2;

            int minValue = 1 + expectedBonus;
            int maxValue = 20 + expectedBonus;

            Project20.Character character = new();
            character.abilityScore[abilityIndex] = abilityScore;

            for (int i = 0; i < numberOfTests; ++i)
            {
                int checkValue = character.CheckAbility(abilityIndex);
                if (checkValue < minValue && maxValue < checkValue)
                {
                    failed = true;
                    break;
                }
            }
            Assert.IsFalse(failed);
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(6)]
        public void OutOfRangeIndex_CheckAbility(int index)
        {
            Project20.Character character = new();

            Assert.ThrowsException<IndexOutOfRangeException>(
                () => character.CheckAbility(index)
            );
        }

        [TestMethod]
        public void ValidName_CheckAbility()
        {
            int numberOfTests = 5;
            bool failed = false;
            int abilityIndex = 0;
            string abilityName = "sTr";
            int abilityScore = 14;
            int expectedBonus = 2;

            int minValue = 1 + expectedBonus;
            int maxValue = 20 + expectedBonus;

            Project20.Character character = new();
            character.abilityScore[abilityIndex] = abilityScore;

            for (int i = 0; i < numberOfTests; ++i)
            {
                int? checkValue = character.CheckAbility(abilityName);
                if (checkValue is null || checkValue < minValue || maxValue < checkValue)
                {
                    failed = true;
                    break;
                }
            }
            Assert.IsFalse(failed);
        }

        [TestMethod]
        public void InvalidName_CheckAbility_Null()
        {
            string name = "Cat";

            Project20.Character character = new();

            Assert.IsNull(character.CheckAbility(name));
        }
        #endregion

        #region CheckSave
        [TestMethod]
        public void ValidName_CheckSave()
        {
            int numberOfTests = 5;
            bool failed = false;
            int abilityIndex = 0;
            string abilityName = "sTr";
            int abilityScore = 14;
            int level = 1;
            int proficiency = 1;
            int expectedBonus = 4;

            int minValue = 1 + expectedBonus;
            int maxValue = 20 + expectedBonus;

            Project20.Character character = new();
            character.abilityScore[abilityIndex] = abilityScore;
            character.saveThrows[abilityIndex] = proficiency;

            for (int i = 0; i < numberOfTests; ++i)
            {
                int? checkValue = character.CheckSave(abilityName);
                if (checkValue is null || checkValue < minValue || maxValue < checkValue)
                {
                    failed = true;
                    break;
                }
            }
            Assert.IsFalse(failed);
        }

        [TestMethod]
        public void InvalidName_CheckSave_Null()
        {
            string name = "Cat";

            Project20.Character character = new();

            Assert.IsNull(character.CheckSave(name));
        }
        #endregion

        [DataTestMethod]
        [DataRow(11, 0)]
        [DataRow(9, -1)]
        [DataRow(12, 1)]
        public void ValidInput_CountModifier(int score, int expected)
        {
            Assert.AreEqual(expected, Project20.Character.CountModifier(score));
        }

        [DataTestMethod]
        [DataRow("stR",0)]
        [DataRow("dEx",1)]
        public void ValidInput_GetAbilityIndex(string name, int expected)
        {
            Assert.AreEqual(expected, Project20.Character.GetAbilityIndex(name));
        }

        [DataTestMethod]
        [DataRow("acroBatiCs", 0)]
        [DataRow("arCANa", 2)]
        public void ValidInput_GetSkillIndex(string name, int expected)
        {
            Assert.AreEqual(expected, Project20.Character.GetSkillIndex(name));
        }
    }
}