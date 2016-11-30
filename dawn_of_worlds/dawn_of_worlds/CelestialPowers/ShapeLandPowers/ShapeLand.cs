using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    abstract class ShapeLand : Power
    {
        protected Area _location { get; set; }

        public ShapeLand(Area location)
        {
            _location = location;
        }

        public override int Cost()
        {
            int cost = 0;
            switch (Simulation.Time.CurrentAge)
            {
                case Age.Creation:
                    cost += 3;
                    break;
                case Age.Races:
                    cost += 5;
                    break;
                case Age.Relations:
                    cost += 8;
                    break;
            }

            return cost;
        }

        public override int Weight(Deity creator)
        {
            int weight = 0;
            switch (Simulation.Time.CurrentAge)
            {
                case Age.Creation:
                    weight += Constants.WEIGHT_MANY_HIGH;
                    break;
                case Age.Races:
                    weight += Constants.WEIGHT_MANY_MEDIUM;
                    break;
                case Age.Relations:
                    weight += Constants.WEIGHT_MANY_LOW;
                    break;
                default:
                    weight += 0;
                    break;
            }

            int cost = Cost();
            if (cost > Constants.WEIGHT_COST_DEVIATION_MEDIUM)
                weight += cost * Constants.WEIGHT_MANY_COST_DEVIATION;
            else
                weight -= cost * Constants.WEIGHT_MANY_COST_DEVIATION;

            if (creator.Domains.Contains(Domain.Creation))
                weight += Constants.WEIGHT_MANY_CHANGE;

            return weight >= 0 ? weight : 0;
        }
    }
}
