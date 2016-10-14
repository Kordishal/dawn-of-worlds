using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.CelestialPowers.ShapeClimatePowers
{
    abstract class ShapeClimate : Power
    {
        public override int Cost(int current_age)
        {
            if (current_age == 1)
                return 2;
            else if (current_age == 2)
                return 4;
            else if (current_age == 3)
                return 6;
            else
                return 8;
        }

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            return true;
        }

        public override int Weight(World current_world, Deity creator, int current_age)
        {
            return 50 - (current_age * 10);
        }
    }
}
