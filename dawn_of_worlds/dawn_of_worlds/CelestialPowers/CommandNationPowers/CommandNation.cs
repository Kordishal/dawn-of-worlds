using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.CelestialPowers.CommandNationPowers
{
    abstract class CommandNation : Power
    {
        protected Civilisation _commanded_nation;

        protected override void initialize()
        {
            Name = "Command Nation";
            BaseCost = new int[] { 6, 4, 2 };
            CostChange = Constants.COST_CHANGE_VALUE;

            BaseWeight = new int[] { Constants.WEIGHT_STANDARD_LOW, Constants.WEIGHT_STANDARD_MEDIUM, Constants.WEIGHT_STANDARD_HIGH };
            WeightChange = Constants.WEIGHT_STANDARD_CHANGE;
        }

        public override bool isObsolete
        {
            get
            {
                return _commanded_nation.isDestroyed;
            }
        }

        public CommandNation(Civilisation commanded_nation)
        {
            _commanded_nation = commanded_nation;
        }
    }
}
