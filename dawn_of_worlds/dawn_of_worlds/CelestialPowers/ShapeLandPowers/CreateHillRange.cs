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
        public override bool Precondition(Deity creator)
        {
            // needs a possible terrain in the area.
            if (candidate_tiles().Count == 0)
                return false;

            return true;
        }

        private List<WeightedObjects<Tile>> candidate_tiles()
        {
            List<WeightedObjects<Tile>> tile_list = new List<WeightedObjects<Tile>>();
            foreach (Tile tile in _location.Tiles)
            {
                if (tile.isDefault && tile.Type == TerrainType.Plain)
                {
                    WeightedObjects<Tile> weighted_tile = new WeightedObjects<Tile>(tile);
                    weighted_tile.Weight += 5;

                    for (int i = 0; i < 8; i++)
                    {
                        SystemCoordinates coords = tile.Coordinates;
                        coords = coords.GetNeighbour(i);

                        if (coords.isInTileGridBounds())
                        {
                            if (Program.World.TileGrid[coords.X, coords.Y].Type == TerrainType.HillRange)
                                weighted_tile.Weight += 20;
                            if (Program.World.TileGrid[coords.X, coords.Y].Type == TerrainType.MountainRange)
                                weighted_tile.Weight += 10;
                        }
                    }

                    tile_list.Add(weighted_tile);
                }
            }

            return tile_list;
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
            List<WeightedObjects<Tile>> hill_range_locations = candidate_tiles();
            Tile hill_range_location = WeightedObjects<Tile>.ChooseRandomObject(hill_range_locations);

            HillRange hill_range = new HillRange(Constants.Names.GetName("hill_ranges"), hill_range_location, creator);
            hill_range_location.Type = TerrainType.HillRange;
            hill_range_location.PrimaryTerrainFeature = hill_range;
            hill_range_location.isDefault = false;
            creator.TerrainFeatures.Add(hill_range);
            creator.LastCreation = hill_range;

            int chance = Constants.Random.Next(100);
            switch (hill_range_location.LocalClimate)
            {
                case Climate.Arctic:
                    hill_range.BiomeType = BiomeType.PolarDesert;
                    break;
                case Climate.SubArctic:
                    if (chance < 50)
                        hill_range.BiomeType = BiomeType.Tundra;
                    else
                        hill_range.BiomeType = BiomeType.BorealForest;
                    break;
                case Climate.Temperate:
                    if (chance < 50)
                        hill_range.BiomeType = BiomeType.TemperateGrassland;
                    else
                        hill_range.BiomeType = BiomeType.TemperateDeciduousForest;
                    break;
                case Climate.SubTropical:
                    if (chance < 50)
                        hill_range.BiomeType = BiomeType.TropicalGrassland;
                    else
                        hill_range.BiomeType = BiomeType.TropicalDryForest;
                    break;
                case Climate.Tropical:
                    if (chance < 50)
                        hill_range.BiomeType = BiomeType.TropicalGrassland;
                    else
                        hill_range.BiomeType = BiomeType.TropicalRainforest;
                    break;
            }

            Program.WorldHistory.AddRecord(hill_range, hill_range.printTerrainFeature);
        }

        public CreateHillRange(Area location) : base (location)
        {
            Name = "Create Hill Range in Area " + location.Name;
        }
    }
}
