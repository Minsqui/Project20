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
    }
}