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
                    weighted_tile.Weight += 5; // Add so that each possible tile has at least some weight as otherwise none will be chosen.

                    for (int i = 0; i < 8; i++)
                    {
                        SystemCoordinates coords = tile.Coordinates;
                        coords = coords.GetNeighbour(i);

                        if (coords.isInTileGridBounds())
                        {
                            if (Program.World.TileGrid[coords.X, coords.Y].Type == TerrainType.MountainRange)
                                weighted_tile.Weight += 20;
                            if (Program.World.TileGrid[coords.X, coords.Y].Type == TerrainType.HillRange)
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
            List<WeightedObjects<Tile>> mountain_range_locations = candidate_tiles();
            Tile mountain_range_location = WeightedObjects<Tile>.ChooseRandomObject(mountain_range_locations);
            MountainRange mountain_range = new MountainRange(Constants.Names.GetName("mountain_ranges"), mountain_range_location, creator);
            mountain_range_location.Type = TerrainType.MountainRange;
            mountain_range_location.PrimaryTerrainFeature = mountain_range;

            int chance = Constants.Random.Next(100);
            switch (mountain_range_location.LocalClimate)
            {
                case Climate.Arctic:
                    mountain_range.BiomeType = BiomeType.PolarDesert;
                    break;
                case Climate.SubArctic:
                    if (chance < 50)
                        mountain_range.BiomeType = BiomeType.Tundra;
                    else
                        mountain_range.BiomeType = BiomeType.BorealForest;
                    break;
                case Climate.Temperate:
                    if (chance < 50)
                        mountain_range.BiomeType = BiomeType.TemperateGrassland;
                    else
                        mountain_range.BiomeType = BiomeType.TemperateDeciduousForest;
                    break;
                case Climate.SubTropical:
                    if (chance < 50)
                        mountain_range.BiomeType = BiomeType.TropicalGrassland;
                    else
                        mountain_range.BiomeType = BiomeType.TropicalDryForest;
                    break;
                case Climate.Tropical:
                    if (chance < 50)
                        mountain_range.BiomeType = BiomeType.TropicalGrassland;
                    else
                        mountain_range.BiomeType = BiomeType.TropicalRainforest;
                    break;
            }


            mountain_range_location.isDefault = false;
            creator.TerrainFeatures.Add(mountain_range);
            creator.LastCreation = mountain_range;

            Program.WorldHistory.AddRecord(mountain_range, mountain_range.printTerrainFeature);
        }

        public CreateMountainRange(Area location) : base (location)
        {
            Name = "Create Mountain Range in Area " + location.Name;
        }
    }
}
