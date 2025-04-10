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
            int[] saveThrows = [0,1,0,2,1,0];
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
    }
}