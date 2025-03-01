using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project20
{
    /// <summary>
    /// Class that represents class from DnD 5e 2014 ruleset.
    /// </summary>
    public class GameClass
    {
        public required string id { get; init; }
        public required string name { get; init; }
        public int hitDice { get; init; }

        public int[] saveThrows { get; init; } = new int[6];

        public Feature[] features { get; init; } = [];

        public record Feature(int level, string text);
    }
}
