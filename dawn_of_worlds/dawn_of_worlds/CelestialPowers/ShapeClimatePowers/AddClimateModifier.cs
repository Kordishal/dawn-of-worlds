using System;
using System.Collections.Generic;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Main;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Effects;

namespace dawn_of_worlds.CelestialPowers.ShapeClimatePowers
{
    class AddClimateModifier : ShapeClimate
    {
        private ClimateModifier _modifier { get; set; }

        protected override void initialize()
        {
            base.initialize();
            Name = "Add Climate Modifer (" + _modifier.ToString() + ")";
            Tags = new List<CreationTag>() { CreationTag.Climate, CreationTag.Magic };
        }

        public override bool Precondition(Deity creator)
        {
            return false;
        }

        public override int Effect(Deity creator)
        {
            return 1;
        }

        public AddClimateModifier(ClimateModifier modifier)
        {   
            _modifier = modifier;
            initialize();
        }
    }
}
