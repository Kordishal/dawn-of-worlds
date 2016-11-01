using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.CelestialPowers.CommandCityPowers
{
    abstract class CommandCity : Power
    {

        protected City _commanded_city;

        public override int Cost(int current_age)
        {
            switch (current_age)
            {
                case 1:
                    return 6;
                case 2:
                    return 4;
                case 3:
                    return 2;
                default:
                    return 1;
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

        public CommandCity(City command_city)
        {
            _commanded_city = command_city;
        }
    }
}
