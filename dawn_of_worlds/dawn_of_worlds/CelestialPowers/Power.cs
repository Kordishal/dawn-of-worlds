using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.CelestialPowers
{
    abstract class Power
    {
        virtual public int Cost(int current_age)
        {
            return 10;
        }

        virtual public int Weight(World current_world, Deity creator, int current_age)
        {
            return 0;
        }

        virtual public bool Precondition(World current_world, Deity creator, int current_age)
        {
            return true;
        }

        abstract public void Effect(World current_world, Deity creator, int current_age);

    }
}
