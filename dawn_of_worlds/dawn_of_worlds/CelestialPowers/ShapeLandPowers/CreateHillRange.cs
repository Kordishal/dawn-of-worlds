using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    class CreateHillRange : ShapeLand
    {
        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            // can't be created in oceans
            if (_location.Type == TerrainType.Ocean)
                return false;

            // Can only be created when nothing else has been added.
            if (_location.PrimaryTerrainFeature != null)
                return false;

            return true;
        }

        public override int Weight(World current_world, Deity creator, int current_age)
        {
            int weight = base.Weight(current_world, creator, current_age);

            if (creator.Domains.Contains(Domain.Earth))
                weight += Constants.WEIGHT_MANY_CHANGE;

            return weight >= 0 ? weight : 0;
        }


        public override void Effect(World current_world, Deity creator, int current_age)
        {
            // Create the mountainrange.
            HillRange hill_range = new HillRange(Constants.Names.GetName("hill_ranges"), _location, creator);
            _location.Type = TerrainType.HillRange;
            _location.PrimaryTerrainFeature = hill_range;

            // Add mountain range to deity
            creator.TerrainFeatures.Add(hill_range);
            creator.LastCreation = hill_range;
        }

        public CreateHillRange(Terrain location) : base (location)
        {
            Name = "Create Hill Range in Terrain " + location.Name;
        }
    }
}
