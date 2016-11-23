using dawn_of_worlds.Actors;
using dawn_of_worlds.Main;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.CelestialPowers.EventPowers
{
    abstract class SpawnEvent : Power
    {
        public override int Cost(int current_age)
        {
            int cost = 0;
            switch (current_age)
            {
                case 1:
                    cost += 10;
                    break;
                case 2:
                    cost += 7;
                    break;
                case 3:
                    cost += 9;
                    break;
                default:
                    cost += 10;
                    break;
            }

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

    }
}
