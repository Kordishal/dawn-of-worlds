using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;
using dawn_of_worlds.WorldModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.CelestialPowers.ShapeClimatePowers
{
    abstract class ShapeClimate : Power
    {
        protected Province _chosen_location { get; set; }

        protected override void initialize()
        {
            Name = "Shape Climate";
            BaseCost = new int[] { 2, 4, 6 };
            CostChange = 2;

            BaseWeight = new int[] { 20, 15, 10 };
            WeightChange = 5;
        }

        protected int[] countClimateNeighbours(Province province)
        {
            int[] climate_count = new int[6] { 0, 0, 0, 0, 0, 0 };
            SystemCoordinates coords = null;
            for (int i = 0; i < 8; i++)
            {
                coords = province.Coordinates.GetNeighbour(i);
                // ignore areas outside of the world.
                if (coords.isInTileGridBounds())
                {
                    switch (Program.State.ProvinceGrid[coords.X, coords.Y].LocalClimate)
                    {
                        case Climate.Arctic:
                            climate_count[0] += 1;
                            break;
                        case Climate.SubArctic:
                            climate_count[1] += 1;
                            break;
                        case Climate.Temperate:
                            climate_count[2] += 1;
                            break;
                        case Climate.SubTropical:
                            climate_count[3] += 1;
                            break;
                        case Climate.Tropical:
                            climate_count[4] += 1;
                            break;
                        case Climate.Inferno:
                            climate_count[5] += 1;
                            break;
                        default:
                            break;
                    }
                }
            }

            return climate_count;
        }

        protected void adjustTerrainFeatureBiomes()
        {
            int chance = Constants.Random.Next(100);

                // change forest biome type
            if (_chosen_location.Type == TerrainType.Plain)
            {
                if (_chosen_location.PrimaryTerrainFeature.GetType() == typeof(Forest))
                {
                    switch (_chosen_location.LocalClimate)
                    {
                        case Climate.SubArctic:
                            _chosen_location.PrimaryTerrainFeature.BiomeType = BiomeType.BorealForest;
                            break;
                        case Climate.Temperate:
                            _chosen_location.PrimaryTerrainFeature.BiomeType = BiomeType.TemperateDeciduousForest;
                            break;
                        case Climate.SubTropical:
                            _chosen_location.PrimaryTerrainFeature.BiomeType = BiomeType.TropicalDryForest;
                            break;
                        case Climate.Tropical:
                            _chosen_location.PrimaryTerrainFeature.BiomeType = BiomeType.TropicalRainforest;
                            break;
                    }
                }
                // change desert biome type
                else if (_chosen_location.PrimaryTerrainFeature.GetType() == typeof(Desert))
                {
                    switch (_chosen_location.LocalClimate)
                    {
                        case Climate.SubArctic:
                            if (chance < 50)
                                _chosen_location.PrimaryTerrainFeature.BiomeType = BiomeType.ColdDesert;
                            else
                                _chosen_location.PrimaryTerrainFeature.BiomeType = BiomeType.Tundra;
                            break;
                        case Climate.Temperate:
                            if (chance < 50)
                                _chosen_location.PrimaryTerrainFeature.BiomeType = BiomeType.ColdDesert;
                            else
                                _chosen_location.PrimaryTerrainFeature.BiomeType = BiomeType.HotDesert;
                            break;
                        case Climate.SubTropical:
                                _chosen_location.PrimaryTerrainFeature.BiomeType = BiomeType.HotDesert;
                            break;
                        case Climate.Tropical:
                            _chosen_location.PrimaryTerrainFeature.BiomeType = BiomeType.HotDesert;
                            break;
                    }
                }
                else if (_chosen_location.PrimaryTerrainFeature.GetType() == typeof(Grassland))
                {
                    switch (_chosen_location.LocalClimate)
                    {
                        case Climate.SubArctic:
                            _chosen_location.PrimaryTerrainFeature.BiomeType = BiomeType.Tundra;
                            break;
                        case Climate.Temperate:
                            _chosen_location.PrimaryTerrainFeature.BiomeType = BiomeType.TemperateGrassland;
                            break;
                        case Climate.SubTropical:
                            _chosen_location.PrimaryTerrainFeature.BiomeType = BiomeType.TropicalGrassland;
                            break;
                        case Climate.Tropical:
                            _chosen_location.PrimaryTerrainFeature.BiomeType = BiomeType.TropicalGrassland;
                            break;
                    }
                }
            }
            else if (_chosen_location.Type == TerrainType.MountainRange)
            {
                foreach (Mountain mountain in ((MountainRange)_chosen_location.PrimaryTerrainFeature).Mountains)
                {
                    switch (_chosen_location.LocalClimate)
                    {
                        case Climate.Arctic:
                            mountain.BiomeType = BiomeType.Tundra;
                            break;
                        case Climate.SubArctic:
                            if (chance < 50)
                                mountain.BiomeType = BiomeType.Tundra;
                            else
                                mountain.BiomeType = BiomeType.BorealForest;
                            break;
                        case Climate.Temperate:
                            if (chance < 50)
                                mountain.BiomeType = BiomeType.TemperateGrassland;
                            else
                                mountain.BiomeType = BiomeType.TemperateDeciduousForest;
                            break;
                        case Climate.SubTropical:
                            if (chance < 50)
                                mountain.BiomeType = BiomeType.TropicalGrassland;
                            else
                                mountain.BiomeType = BiomeType.TropicalDryForest;
                            break;
                        case Climate.Tropical:
                            if (chance < 50)
                                mountain.BiomeType = BiomeType.TropicalGrassland;
                            else
                                mountain.BiomeType = BiomeType.TropicalRainforest;
                            break;
                    }
                }
            }
            else if (_chosen_location.Type == TerrainType.HillRange)
            {
                foreach (Hill hill in ((HillRange)_chosen_location.PrimaryTerrainFeature).Hills)
                {
                    switch (_chosen_location.LocalClimate)
                    {
                        case Climate.Arctic:
                            hill.BiomeType = BiomeType.Tundra;
                            break;
                        case Climate.SubArctic:
                            if (chance < 33)
                                hill.BiomeType = BiomeType.Tundra;
                            else if (chance < 66)
                                hill.BiomeType = BiomeType.ColdDesert;
                            else
                                hill.BiomeType = BiomeType.BorealForest;
                            break;
                        case Climate.Temperate:
                            if (chance < 25)
                                hill.BiomeType = BiomeType.TemperateGrassland;
                            else if (chance < 50)
                                hill.BiomeType = BiomeType.ColdDesert;
                            else if (chance < 75)
                                hill.BiomeType = BiomeType.HotDesert;
                            else
                                hill.BiomeType = BiomeType.TemperateDeciduousForest;
                            break;
                        case Climate.SubTropical:
                            if (chance < 33)
                                hill.BiomeType = BiomeType.TropicalGrassland;
                            else if (chance < 66)
                                hill.BiomeType = BiomeType.HotDesert;
                            else
                                hill.BiomeType = BiomeType.TropicalRainforest;
                            break;
                        case Climate.Tropical:
                            if (chance < 33)
                                hill.BiomeType = BiomeType.TropicalGrassland;
                            else if (chance < 66)
                                hill.BiomeType = BiomeType.HotDesert;
                            else
                                hill.BiomeType = BiomeType.TropicalRainforest;
                            break;
                    }
                }
            }
        }
    }
}
