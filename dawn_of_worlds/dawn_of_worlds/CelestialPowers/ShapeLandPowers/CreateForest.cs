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

            // needs a possible terrain in the area.
            if (candidate_terrain().Count == 0)
                return false;

            return true;
        }

        private List<Tile> candidate_terrain()
        {
            List<Tile> tile_list = new List<Tile>();
            foreach (Tile tile in _location.Tiles)
            {
                if (tile.isDefault && tile.Type == TerrainType.Plain)
                    if (tile.LocalClimate != Climate.Arctic)
                        tile_list.Add(tile);
            }

            return tile_list;
        }


        public override void Effect(Deity creator)
        {
            List<Tile> forest_locations = candidate_terrain();
            Tile forest_location = forest_locations[Constants.Random.Next(forest_locations.Count)];           
                           
            Forest forest = new Forest("PlaceHolder", forest_location, creator);
            
            switch (forest_location.LocalClimate)
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



            forest_location.PrimaryTerrainFeature = forest;
            forest_location.UnclaimedTerritories.Add(forest);
            forest_location.UnclaimedTravelAreas.Add(forest);
            forest_location.UnclaimedHuntingGrounds.Add(forest);
            forest_location.isDefault = false;

            // Add forest to the deity.
            creator.TerrainFeatures.Add(forest);
            creator.LastCreation = forest;

            forest.Name = Constants.Names.GetForestName(forest);
            Program.WorldHistory.AddRecord(forest, forest.printTerrainFeature);        
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
