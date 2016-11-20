using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Organisations;
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


        public CommandArmy(Army commanded_army)
        {
            _commanded_army = commanded_army;
        }
    }
}
