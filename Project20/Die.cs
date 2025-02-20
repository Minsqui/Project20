using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project20
{
    internal class Die
    {
        /// <summary>
        /// Converts a die shorthand to actual dice throw, and returns the throw value.
        /// </summary>
        /// <param name="dieShorthand">Die shorthand, for example: 2d6</param>
        /// <returns></returns>
        public static int Roll(string dieShorthand)
        {
            int result = 0;
            string[] shorthandSplit;
            Random rand = new Random();
            int die;
            
            try
            {
                shorthandSplit = dieShorthand.Split("d");
                die = Convert.ToInt32(shorthandSplit[1]);

                for (int i = 0; i < Convert.ToInt32(shorthandSplit[0]); ++i)
                {
                    result += rand.Next(1, die+1);
                }
                
            }
            catch
            {
                throw new Exception("Invalid die shorthand");
            }

            return result;
        }

    }
}
