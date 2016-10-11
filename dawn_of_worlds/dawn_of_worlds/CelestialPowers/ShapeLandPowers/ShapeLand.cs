using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    abstract class ShapeLand : Power
    {
        public override int Cost(int current_age)
        {
            if (current_age == 1)
                return 3;
            else if (current_age == 2)
                return 5;
            else if (current_age == 3)
                return 8;
            else
                return 12;
        }

        public override bool Precondition(World current_world, Deity deity, int current_age)
        {
            return true;
        }

        public override int Weight(World current_world, Deity deity, int current_age)
        {
            return 50 - (current_age * 10);
        }
    }
}
