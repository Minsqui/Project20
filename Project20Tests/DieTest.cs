using System.Numerics;

namespace Project20Tests
{
    [TestClass]
    public sealed class DieTest
    {
        [DataTestMethod]
        [DataRow("1d6", 1, 6)]
        [DataRow("4d6", 4, 24)]
        [DataRow("1d20", 1, 20)]
        public void ValidInput_Roll_ValidOutput(string shorthand, int minValue, int maxValue)
        {
            int numberOfTests = 5;
            bool failed = false;

            for (int i = 0; i < numberOfTests; ++i)
            {
                int rollValue = Core.Die.Roll(shorthand);
                if (rollValue < minValue || maxValue < rollValue)
                {
                    failed = true;
                    break;
                }
            }
            Assert.IsFalse(failed);
        }

        [TestMethod]
        public void NullInput_Roll_ValidOutput()
        {
            Assert.ThrowsException<ApplicationException>(
                () => Core.Die.Roll(null)
            );
        }

        [DataTestMethod]
        [DataRow("ahoj")]
        [DataRow("14g58")]
        [DataRow("4d")]
        [DataRow("d8")]
        public void InvalidInput_Roll_InvalidOutput(string shorthand)
        {
            Assert.ThrowsException<ApplicationException>(
                () => Core.Die.Roll(shorthand)
            );
        }
    }
}
