using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Main;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.CelestialPowers.CommandArmyPowers
{
    abstract class CommandArmy : Power
    {
        protected Army _commanded_army { get; set; }


        public override bool isObsolete
        {
            get
            {
                return _commanded_army.isScattered;
            }
        }

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


        public CommandArmy(Army commanded_army)
        {
            _commanded_army = commanded_army;
        }
    }
}
