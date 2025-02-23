using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project20
{
    public class GameRace
    {
        public record Trait(string name, string description);

        public int[] abilityScore { get; set; }
        public int speed { get; set; }
        public GameRace[] subraces { get; set; }
        public Trait[] traits { get; set; }
    }

    
}
