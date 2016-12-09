using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using dawn_of_worlds.Creations.Geography;

namespace dawn_of_worlds.Log
{
    class Map
    {
        public static char[,] generateTerrainMap()
        {
            char[,] terrain_map = new char[Constants.TILE_GRID_X, Constants.TILE_GRID_Y];


            for (int i = 0; i < Constants.TILE_GRID_X; i++)
            {
                for (int j = 0; j < Constants.TILE_GRID_Y; j++)
                {
                    switch (Program.World.TileGrid[i, j].Type)
                    {
                        case WorldClasses.TerrainType.Plain:
                            terrain_map[i, j] = '_';
                            break;
                        case WorldClasses.TerrainType.HillRange:
                            terrain_map[i, j] = 'n';
                            break;
                        case WorldClasses.TerrainType.MountainRange:
                            terrain_map[i, j] = '^';
                            break;
                        case WorldClasses.TerrainType.Island:
                            terrain_map[i, j] = 'o';
                            break;
                        case WorldClasses.TerrainType.Ocean:
                            terrain_map[i, j] = '~';
                            break;
                    }

                }
            }

            return terrain_map;
        }
        public static char[,] generateBiomeMap()
        {
            char[,] biome_map = new char[Constants.TILE_GRID_X, Constants.TILE_GRID_Y];
            for (int i = 0; i < Constants.TILE_GRID_X; i++)
            {
                for (int j = 0; j < Constants.TILE_GRID_Y; j++)
                {
                    switch (Program.World.TileGrid[i, j].PrimaryTerrainFeature.BiomeType)
                    {
                        case BiomeType.PolarDesert:
                            biome_map[i, j] = 'p';
                            break;
                        case BiomeType.ColdDesert:
                            biome_map[i, j] = '=';
                            break;
                        case BiomeType.HotDesert:
                            biome_map[i, j] = '!';
                            break;
                        case BiomeType.BorealForest:
                            biome_map[i, j] = '¦';
                            break;
                        case BiomeType.TemperateDeciduousForest:
                            biome_map[i, j] = '§';
                            break;
                        case BiomeType.TropicalDryForest:
                            biome_map[i, j] = '#';
                            break;
                        case BiomeType.TropicalRainforest:
                            biome_map[i, j] = '%';
                            break;
                        case BiomeType.TemperateGrassland:
                            biome_map[i, j] = ',';
                            break;
                        case BiomeType.TropicalGrassland:
                            biome_map[i, j] = '.';
                            break;
                        case BiomeType.Tundra:
                            biome_map[i, j] = '_';
                            break;
                    }
                }
            }

            return biome_map;
        }
        public static char[,] generateClimateMap()
        {
            char[,] climate_map = new char[Constants.TILE_GRID_X, Constants.TILE_GRID_Y];

            for (int i = 0; i < Constants.TILE_GRID_X; i++)
            {
                for (int j = 0; j < Constants.TILE_GRID_Y; j++)
                {
                    switch (Program.World.TileGrid[i, j].LocalClimate)
                    {
                        case WorldClasses.Climate.Arctic:
                            climate_map[i, j] = 'A';
                            break;
                        case WorldClasses.Climate.SubArctic:
                            climate_map[i, j] = 'a';
                            break;
                        case WorldClasses.Climate.Temperate:
                            climate_map[i, j] = '-';
                            break;
                        case WorldClasses.Climate.SubTropical:
                            climate_map[i, j] = 't';
                            break;
                        case WorldClasses.Climate.Tropical:
                            climate_map[i, j] = 'T';
                            break;
                    }

                }
            }

            return climate_map;
        }
        public static string printMap(Record record)
        {
            string result = "";

            result += "################### Turn: " + record.Turn + " ######################\n";

            for (int i = 0; i < Constants.TILE_GRID_X; i++)
            {
                for (int j = 0; j < Constants.TILE_GRID_Y; j++)
                {
                    result += " " + record.Map[i, j];
                }
                result += "\n";
            }
            return result;
        }
    }
}
