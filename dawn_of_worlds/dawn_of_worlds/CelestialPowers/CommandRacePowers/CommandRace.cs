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

        public override int Cost()
        {
            int cost = 0;
            switch (Simulation.Time.CurrentAge)
            {
                case Age.Creation:
                    cost += 8;
                    break;
                case Age.Races:
                    cost += 4;
                    break;
                case Age.Relations:
                    cost += 3;
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
                    weight += Constants.WEIGHT_STANDARD_LOW;
                    break;
                case Age.Races:
                    weight += Constants.WEIGHT_STANDARD_MEDIUM;
                    break;
                case Age.Relations:
                    weight += Constants.WEIGHT_STANDARD_HIGH;
                    break;
            }

            int cost = Cost();
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
