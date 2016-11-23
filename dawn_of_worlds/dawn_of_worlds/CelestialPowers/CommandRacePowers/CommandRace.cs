using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Main;

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
            int weight = 0;

            switch (current_age)
            {
                case 1:
                    weight += Constants.WEIGHT_STANDARD_LOW;
                    break;
                case 2:
                    weight += Constants.WEIGHT_STANDARD_MEDIUM;
                    break;
                case 3:
                    weight += Constants.WEIGHT_STANDARD_HIGH;
                    break;
                default:
                    weight += 0;
                    break;
            }

            int cost = Cost(current_age);
            if (cost > Constants.WEIGHT_COST_DEVIATION_MEDIUM)
                weight += cost * Constants.WEIGHT_STANDARD_COST_DEVIATION;
            else
                weight -= cost * Constants.WEIGHT_STANDARD_COST_DEVIATION;

            return weight >= 0 ? weight : 0;
        }

        public CommandRace(Race commanded_race)
        {
            _commanded_race = commanded_race;
        }

    }
}
