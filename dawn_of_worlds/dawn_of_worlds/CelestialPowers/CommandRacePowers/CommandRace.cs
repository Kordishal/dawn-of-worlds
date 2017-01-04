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

        protected override void initialize()
        {
            Name = "Command Race";
            BaseCost = new int[] { 8, 4, 3 };
            CostChange = Constants.COST_CHANGE_VALUE;

            BaseWeight = new int[] { Constants.WEIGHT_STANDARD_LOW, Constants.WEIGHT_STANDARD_MEDIUM, Constants.WEIGHT_STANDARD_HIGH };
            WeightChange = Constants.WEIGHT_STANDARD_CHANGE;
        }

        public CommandRace(Race commanded_race)
        {
            _commanded_race = commanded_race;
        }

    }
}
