using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Main;
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
            int cost = 0;
            switch (current_age)
            {
                case 1:
                    cost += 6;
                    break;
                case 2:
                    cost += 4;
                    break;
                case 3:
                    cost += 2;
                    break;
                default:
                    cost += 2;
                    break;
            }

            if (_commanded_city.Owner.Tags.Contains(NationalTags.VeryRich))
                cost -= 2;

            return cost;
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

        public CommandCity(City command_city)
        {
            _commanded_city = command_city;
        }
    }
}
