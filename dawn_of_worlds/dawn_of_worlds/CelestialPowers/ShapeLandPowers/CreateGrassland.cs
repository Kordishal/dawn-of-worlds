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
    class CreateGrassland : ShapeLand
    {
        // Grasslands are never created in landmass, as they are the default terrain type.
        // but are kept on ocean tiles in case of islands (NYI)
        public override bool isObsolete
        {
            get
            {
                return _location.RegionArea.Landmass;
            }
        }


        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            if (isObsolete)
                return false;

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
                if (terrain.Type == TerrainType.Island)
                    terrain_list.Add(terrain);
            }

            return terrain_list;
        }

        public override int Weight(World current_world, Deity creator, int current_age)
        {
            int weight = base.Weight(current_world, creator, current_age);

            if (creator.Domains.Contains(Domain.Nature))
                weight += Constants.WEIGHT_MANY_CHANGE;

            if (creator.Domains.Contains(Domain.Drought))
                weight -= Constants.WEIGHT_MANY_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        public CreateGrassland(Area location) : base(location)
        {
            Name = "Create Grassland in Area " + location.Name;
        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            List<Terrain> grassland_locations = candidate_terrain();
            Terrain grassland_location = grassland_locations[Constants.RND.Next(grassland_locations.Count)];

            Grassland grassland = new Grassland("PlaceHolder", grassland_location, creator);

            switch (_location.ClimateArea)
            {
                case Climate.Arctic:
                    grassland.BiomeType = BiomeType.Tundra;
                    break;
                case Climate.SubArctic:
                    grassland.BiomeType = BiomeType.Tundra;
                    break;
                case Climate.Temperate:
                    grassland.BiomeType = BiomeType.TemperateGrassland;
                    break;
                case Climate.SubTropical:
                    grassland.BiomeType = BiomeType.TropicalGrassland;
                    break;
                case Climate.Tropical:
                    grassland.BiomeType = BiomeType.TropicalGrassland;
                    break;
            }

            grassland.Name = Constants.Names.GetName("grasslands");
            grassland_location.PrimaryTerrainFeature = grassland;
            grassland_location.UnclaimedTerritory.Add(grassland);

            creator.TerrainFeatures.Add(grassland);
            creator.LastCreation = grassland;
        }
    }
}
