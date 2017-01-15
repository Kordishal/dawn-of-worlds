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
            base.Precondition(creator);
            if (_location.Provinces.Exists(x => x.LocalClimateModifier != _modifier))
                return true;

            return false;
        }

        public override void Effect(Deity creator)
        {
            Province province = _location.Provinces[Constants.Random.Next(_location.Provinces.Count)];
            province.LocalClimateModifier = _modifier;
        }

        public AddClimateModifier(Area location, ClimateModifier modifier) : base(location)
        {   
            _modifier = modifier;
            initialize();
        }
    }
}
