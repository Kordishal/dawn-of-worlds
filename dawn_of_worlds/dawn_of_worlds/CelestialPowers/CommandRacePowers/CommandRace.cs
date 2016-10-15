using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Inhabitants;

namespace dawn_of_worlds.CelestialPowers.CommandRacePowers
{
    abstract class CommandRace : Power
    {
        protected Race _commanded_race { get; set; }


        public override int Cost(int current_age)
        {
            switch (current_age)
            {
                case 1:
                    return 8;
                case 2:
                    return 4;
                case 3:
                    return 3;
                default:
                    return 2;
            }
        }

        public override int Weight(World current_world, Deity creator, int current_age)
        {
            switch (current_age)
            {
                case 1:
                    return 10;
                case 2:
                    return 40;
                case 3:
                    return 60;
                default:
                    return 100;
            }
        }

        public CommandRace(Race commanded_race)
        {
            _commanded_race = commanded_race;
        }

    }
}
