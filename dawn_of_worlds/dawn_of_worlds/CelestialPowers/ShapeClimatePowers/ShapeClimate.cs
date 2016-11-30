using dawn_of_worlds.Actors;
using dawn_of_worlds.Main;
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
        public override int Cost()
        {
            int cost = 0;
            switch (Simulation.Time.CurrentAge)
            {
                case Age.Creation:
                    cost += 2;
                    break;
                case Age.Races:
                    cost += 4;
                    break;
                case Age.Relations:
                    cost += 6;
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
            }

            int cost = Cost();
            if (cost > Constants.WEIGHT_COST_DEVIATION_MEDIUM)
                weight += cost * Constants.WEIGHT_MANY_COST_DEVIATION;
            else
                weight -= cost * Constants.WEIGHT_MANY_COST_DEVIATION;


            return weight >= 0 ? weight : 0;
        }
    }
}
