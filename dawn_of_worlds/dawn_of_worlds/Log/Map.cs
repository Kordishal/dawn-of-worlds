using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.WorldModel;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Creations.Civilisations;

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
                    switch (Program.State.ProvinceGrid[i, j].Type)
                    {
                        case TerrainType.Plain:
                            terrain_map[i, j] = '_';
                            break;
                        case TerrainType.HillRange:
                            terrain_map[i, j] = 'n';
                            break;
                        case TerrainType.MountainRange:
                            terrain_map[i, j] = '^';
                            break;
                        case TerrainType.Island:
                            terrain_map[i, j] = 'o';
                            break;
                        case TerrainType.Ocean:
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
                    switch (Program.State.ProvinceGrid[i, j].PrimaryTerrainFeature.BiomeType)
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
                        case BiomeType.Ocean:
                            biome_map[i, j] = '~';
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
                    switch (Program.State.ProvinceGrid[i, j].LocalClimate)
                    {
                        case Climate.Arctic:
                            climate_map[i, j] = 'A';
                            break;
                        case Climate.SubArctic:
                            climate_map[i, j] = 'a';
                            break;
                        case Climate.Temperate:
                            climate_map[i, j] = '-';
                            break;
                        case Climate.SubTropical:
                            climate_map[i, j] = 't';
                            break;
                        case Climate.Tropical:
                            climate_map[i, j] = 'T';
                            break;
                        case Climate.Inferno:
                            climate_map[i, j] = '@';
                            break;
                    }

                }
            }

            return climate_map;
        }
        public static char[,] generateRaceSettlementMap(Race race)
        {
            char[,] race_settlement_map = new char[Constants.TILE_GRID_X, Constants.TILE_GRID_Y];

            foreach (Province province in Program.State.ProvinceGrid)
            {
                if (race.SettledProvinces.Contains(province))
                    race_settlement_map[province.Coordinates.X, province.Coordinates.Y] = '0';
                else
                    race_settlement_map[province.Coordinates.X, province.Coordinates.Y] = 'x';
            }

            return race_settlement_map;
        }

        public static char[,] generateNationTerritoryMap(Civilisation nation)
        {
            char[,] nation_territory_map = new char[Constants.TILE_GRID_X, Constants.TILE_GRID_Y];

            foreach (Province province in Program.State.ProvinceGrid)
            {
                if (nation.Territory.Contains(province))
                    nation_territory_map[province.Coordinates.X, province.Coordinates.Y] = 'T';
                else
                    nation_territory_map[province.Coordinates.X, province.Coordinates.Y] = 'U';
            }

            return nation_territory_map;
        }

        public static char[,] generateTerritoryMap()
        {
            char[,] territroy_map = new char[Constants.TILE_GRID_X, Constants.TILE_GRID_Y];

            foreach (Province province in Program.State.ProvinceGrid)
            {
                if (province.Owner != null)
                    territroy_map[province.Coordinates.X, province.Coordinates.Y] = 'T';
                else
                    territroy_map[province.Coordinates.X, province.Coordinates.Y] = '-';
            }

            return territroy_map;
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
