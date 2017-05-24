using System;
using System.Collections.Generic;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Main;
using dawn_of_worlds.Effects;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    class AddResource : Power
    {
        public override int Effect(Deity creator)
        {


            return 0;
        }

        protected override void initialize()
        {
            Name = "Shape Land";
            BaseCost = new int[] { 2, 4, 6 };
            CostChange = 2;

            BaseWeight = new int[] { 20, 15, 10 };
            WeightChange = 5;

            Tags = new List<CreationTag>() {  CreationTag.Metal, CreationTag.Trade };
        }
    }
}
