using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project20
{
    /// <summary>
    /// Class with function of a die.
    /// </summary>
    public static class Die
    {
        /// <summary>
        /// Converts a die shorthand to actual dice throw, and returns the throw value.
        /// </summary>
        /// <param name="dieShorthand">Die shorthand, for example: 2d6</param>
        /// <returns></returns>
        public static int Roll(string dieShorthand)
        {
            ApplicationException invalidShorthand = new ApplicationException($"Invalid die shorthand: \"{dieShorthand}\"");
            
            int result = 0;
            string[] shorthandSplit;
            Random rand = new Random();

            int numberOfDices;
            int dieValue;

            if (dieShorthand is null) throw invalidShorthand;                

            shorthandSplit = dieShorthand.Split("d");

            if (shorthandSplit.Length < 2) throw invalidShorthand;
            if (!int.TryParse(shorthandSplit[0], out numberOfDices)) throw invalidShorthand;
            if (!int.TryParse(shorthandSplit[1], out dieValue)) throw invalidShorthand;

            for (int i = 0; i < numberOfDices; ++i)
            {
                result += rand.Next(1, dieValue + 1);
            }

            return result;
        }

    }
}
