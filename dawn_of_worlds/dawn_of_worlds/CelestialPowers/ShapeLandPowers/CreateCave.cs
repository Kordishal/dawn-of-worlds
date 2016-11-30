using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    class CreateCave : ShapeLand
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
                if (terrain.Type == TerrainType.MountainRange || terrain.Type == TerrainType.MountainRange)
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

        public CreateCave(Area location) : base(location)
        {
            Name = "Create Cave in Area " + location.Name;
        }

        public override void Effect(Deity creator)
        {
            List<Terrain> cave_locations = candidate_terrain();

            // Caves are placed in a random location within the territory.
            Terrain cave_location = cave_locations[Constants.RND.Next(cave_locations.Count)];

            Cave cave = new Cave("PlaceHolder", cave_location, creator);

            int chance = Constants.RND.Next(100);
            switch (_location.ClimateArea)
            {
                case Climate.SubArctic:
                    cave.BiomeType = BiomeType.Subterranean;
                    break;
                case Climate.Temperate:
                    cave.BiomeType = BiomeType.Subterranean;
                    break;
                case Climate.SubTropical:
                    cave.BiomeType = BiomeType.Subterranean;
                    break;
                case Climate.Tropical:
                    cave.BiomeType = BiomeType.Subterranean;
                    break;
            }

            cave.Name = Constants.Names.GetName("caves");
            cave_location.SecondaryTerrainFeatures.Add(cave);
            cave_location.UnclaimedTerritories.Add(cave);
            cave_location.UnclaimedTravelAreas.Add(cave);
            cave_location.UnclaimedHuntingGrounds.Add(cave);

            // Add forest to the deity.
            creator.TerrainFeatures.Add(cave);
            creator.LastCreation = cave;
        }
    }
}
