using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project20
{
    /// <summary>
    /// Class that represents race from DnD 5e 2014 ruleset.
    /// </summary>
    public class GameRace
    {
        public required string id { get; init; }
        public required string name { get; init; }

        /// <summary>
        /// Ability bonus of the race.
        /// </summary>
        public int[] abilityScore { get; init; } = new int[6];
        public int speed { get; init; }
        public Trait[] traits { get; init; } = [];

        public record Trait(string name, string description);
    }

    
}
