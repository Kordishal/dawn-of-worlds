using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Main;
using System;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    abstract class ShapeLand : Power
    {
        protected Area _location { get; set; }

        protected override void initialize()
        {
            Name = "Shape Land";
            BaseCost = new int[] { 2, 4, 6 };
            CostChange = 2;

            BaseWeight = new int[] { 20, 15, 10 };
            WeightChange = 5; 
        }

        public ShapeLand(Area location)
        {
            _location = location;
            initialize();
        }
    }
}
