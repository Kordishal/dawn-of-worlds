using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    class CreateMountainRange : ShapeLand
    {
        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            // can't be created in oceans
            if (_location.Type == TerrainType.Ocean)
                return false;

            // can only be used when the primary terrain feature is not yet defined.
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
            // Create the mountain range.
            MountainRange mountain_range = new MountainRange(Constants.Names.GetName("mountain_ranges"), _location, creator);
            _location.Type = TerrainType.MountainRange;
            _location.PrimaryTerrainFeature = mountain_range;

            // Add mountain range to deity
            creator.TerrainFeatures.Add(mountain_range);
            creator.LastCreation = mountain_range;
        }

        public CreateMountainRange(Terrain location) : base (location)
        {
            Name = "Create Mountain Range in Location " + location.Name;
        }
    }
}
