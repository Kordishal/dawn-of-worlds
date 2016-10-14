using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;

namespace dawn_of_worlds.CelestialPowers.RaceCreationPowers.SubRaceCreationPowers
{
    abstract class CreateSubRace : Power
    {
        public override int Cost(int current_age)
        {
            switch (current_age)
            {
                case 1:
                    return 12;
                case 2:
                    return 4;
                case 3:
                    return 10;
                default:
                    return 40;
            }
        }

        public override int Weight(World current_world, Deity creator, int current_age)
        {
            switch (current_age)
            {
                case 1:
                    return 10;
                case 2:
                    return 200;
                case 3:
                    return 50;
                default:
                    return 100;
            }
        }
    }
}
