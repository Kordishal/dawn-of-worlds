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

        protected override void initialize()
        {
            Name = "Command City";
            BaseCost = new int[] { 6, 4, 2 };
            CostChange = Constants.COST_CHANGE_VALUE;

            BaseWeight = new int[] { Constants.WEIGHT_STANDARD_LOW, Constants.WEIGHT_STANDARD_MEDIUM, Constants.WEIGHT_STANDARD_HIGH };
            WeightChange = Constants.WEIGHT_STANDARD_CHANGE;
        }

        public CommandCity(City command_city)
        {
            _commanded_city = command_city;
        }
    }
}
