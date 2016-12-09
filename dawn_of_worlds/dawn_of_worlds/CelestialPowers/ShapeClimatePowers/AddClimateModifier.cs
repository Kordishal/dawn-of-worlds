using System;
using System.Collections.Generic;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Main;
using dawn_of_worlds.WorldClasses;

namespace dawn_of_worlds.CelestialPowers.ShapeClimatePowers
{
    class AddClimateModifier : ShapeClimate
    {
        private ClimateModifier _modifier { get; set; }

        public AddClimateModifier(Area location, ClimateModifier modifier) : base(location)
        {
            Name = "Add Climate Modifer: " + modifier.ToString();
            _modifier = modifier;
        }

        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

            if (creator.Domains.Contains(Domain.Magic))
                weight += Constants.WEIGHT_MANY_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        public override void Effect(Deity creator)
        {
            throw new NotImplementedException();
        }
    }
}
