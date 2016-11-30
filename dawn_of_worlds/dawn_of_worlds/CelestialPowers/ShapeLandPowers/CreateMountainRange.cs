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
        public override bool Precondition(Deity creator)
        {
            // needs a possible terrain in the area.
            if (candidate_terrain().Count == 0)
                return false;

            return true;
        }

        private List<Terrain> candidate_terrain()
        {
            List<Terrain> terrain_list = new List<Terrain>();
            foreach (Terrain terrain in _location.TerrainArea)
            {
                if (terrain.isDefault && terrain.Type == TerrainType.Plain)
                    terrain_list.Add(terrain);
            }

            return terrain_list;
        }

        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

            if (creator.Domains.Contains(Domain.Earth))
                weight += Constants.WEIGHT_MANY_CHANGE;

            return weight >= 0 ? weight : 0;
        }


        public override void Effect(Deity creator)
        {
            List<Terrain> mountain_range_locations = candidate_terrain();
            Terrain mountain_range_location = mountain_range_locations[Constants.RND.Next(mountain_range_locations.Count)];
            MountainRange mountain_range = new MountainRange(Constants.Names.GetName("mountain_ranges"), mountain_range_location, creator);
            mountain_range_location.Type = TerrainType.MountainRange;
            mountain_range_location.PrimaryTerrainFeature = mountain_range;
            mountain_range_location.isDefault = false;
            creator.TerrainFeatures.Add(mountain_range);
            creator.LastCreation = mountain_range;
        }

        public CreateMountainRange(Area location) : base (location)
        {
            Name = "Create Mountain Range in Area " + location.Name;
        }
    }
}
