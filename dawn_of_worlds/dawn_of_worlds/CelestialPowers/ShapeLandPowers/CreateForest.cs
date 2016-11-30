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
    class CreateForest : ShapeLand
    {
        public override bool Precondition(Deity creator)
        {
            if (_location.ClimateArea == Climate.Arctic)
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
                if (terrain.isDefault && terrain.Type == TerrainType.Plain)
                    terrain_list.Add(terrain);
            }

            return terrain_list;
        }


        public override void Effect(Deity creator)
        {
            List<Terrain> forest_locations = candidate_terrain();
            Terrain forest_location = forest_locations[Constants.RND.Next(forest_locations.Count)];           
                           
            Forest forest = new Forest("PlaceHolder", forest_location, creator);
            
            switch (_location.ClimateArea)
            {
                case Climate.SubArctic:
                    forest.BiomeType = BiomeType.BorealForest;
                    break;
                case Climate.Temperate:
                    forest.BiomeType = BiomeType.TemperateDeciduousForest;
                    break;
                case Climate.SubTropical:
                    forest.BiomeType = BiomeType.TropicalDryForest;
                    break;
                case Climate.Tropical:
                    forest.BiomeType = BiomeType.TropicalRainforest;
                    break;
            }

            forest.Name = Constants.Names.GetName("forests");

            forest_location.PrimaryTerrainFeature = forest;
            forest_location.UnclaimedTerritories.Add(forest);
            forest_location.UnclaimedTravelAreas.Add(forest);
            forest_location.UnclaimedHuntingGrounds.Add(forest);
            forest_location.isDefault = false;

            // Add forest to the deity.
            creator.TerrainFeatures.Add(forest);
            creator.LastCreation = forest;            
        }

        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

            if (creator.Domains.Contains(Domain.Nature))
                weight += Constants.WEIGHT_MANY_CHANGE;

            if (creator.Domains.Contains(Domain.Drought))
                weight -= Constants.WEIGHT_MANY_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        public CreateForest(Area location) : base (location)
        {
            Name = "Create Forest in Area " + location.Name;
        }
    }
}
