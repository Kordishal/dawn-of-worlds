﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.CelestialPowers.ShapeClimatePowers
{
    class MakeClimateWarmer : ShapeClimate
    {
        private Area _location { get; set; }

        public override int Cost(int current_age)
        {
            return base.Cost(current_age) + 2;
        }

        public override int Weight(World current_world, Deity creator, int current_age)
        {
            int weight = base.Weight(current_world, creator, current_age);

            if (creator.Domains.Contains(Domain.Heat))
                weight += Constants.WEIGHT_MANY_CHANGE;

            if (creator.Domains.Contains(Domain.Fire))
                weight += Constants.WEIGHT_MANY_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            int[] climate_count = new int[5] { 0, 0, 0, 0, 0 };

            SystemCoordinates coords = null;
            for (int i = 0; i < 8; i++)
            {
                coords = _location.Coordinates.GetNeighbour(i);
                // ignore areas outside of the world.
                if (coords.X >= 0 || coords.Y >= 0 || coords.X < Constants.AREA_GRID_X || coords.Y < Constants.AREA_GRID_Y) 
                    continue;

                switch (current_world.AreaGrid[coords.X, coords.Y].ClimateArea)
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
                }
            }


            switch (_location.ClimateArea)
            {
                case Climate.Arctic:
                    if (climate_count[1] >= 2) // needs two sub-arctic
                        return true;
                    else
                        return false;
                case Climate.SubArctic:
                    if (climate_count[2] >= 2) // needs two temperate
                        return true;
                    else
                        return false;
                case Climate.Temperate:
                    if (climate_count[3] >= 2) // need two sub tropical
                        return true;
                    else
                        return false;
                case Climate.SubTropical:
                    if (climate_count[4] >= 2) // needs two tropical
                        return true;
                    else
                        return false;
                case Climate.Tropical: // not possible to make warmer.
                    return false;
            }


            return false;
        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            int chance = Constants.RND.Next(100);

            // change climate.
            switch (_location.ClimateArea)
            {
                case Climate.Arctic:
                    _location.ClimateArea = Climate.SubArctic;
                    break;
                case Climate.SubArctic:
                    _location.ClimateArea = Climate.Temperate;
                    break;
                case Climate.Temperate:
                    _location.ClimateArea = Climate.SubTropical;
                    break;
                case Climate.SubTropical:
                    _location.ClimateArea = Climate.Tropical;
                    break;
            }

            
            foreach (Terrain terrain in _location.TerrainArea)
            {
                // change forest biome type
                if (terrain.Type == TerrainType.Plain)
                {
                    if (terrain.PrimaryTerrainFeature.GetType() == typeof(Forest))
                    {
                        switch (_location.ClimateArea)
                        {
                            case Climate.SubArctic:
                                terrain.PrimaryTerrainFeature.BiomeType = BiomeType.BorealForest;
                                break;
                            case Climate.Temperate:
                                terrain.PrimaryTerrainFeature.BiomeType = BiomeType.TemperateDeciduousForest;
                                break;
                            case Climate.SubTropical:
                                terrain.PrimaryTerrainFeature.BiomeType = BiomeType.TropicalDryForest;
                                break;
                            case Climate.Tropical:
                                terrain.PrimaryTerrainFeature.BiomeType = BiomeType.TropicalRainforest;
                                break;
                        }
                    }
                    // change desert biome type
                    else if (terrain.PrimaryTerrainFeature.GetType() == typeof(Desert))
                    {               
                        switch (_location.ClimateArea)
                        {
                            case Climate.SubArctic:
                                if (chance < 50)
                                    terrain.PrimaryTerrainFeature.BiomeType = BiomeType.ColdDesert;
                                else
                                    terrain.PrimaryTerrainFeature.BiomeType = BiomeType.Tundra;
                                break;
                            case Climate.Temperate:
                                if (chance < 50)
                                    terrain.PrimaryTerrainFeature.BiomeType = BiomeType.ColdDesert;
                                else
                                    terrain.PrimaryTerrainFeature.BiomeType = BiomeType.HotDesert;
                                break;
                            case Climate.SubTropical:
                                terrain.PrimaryTerrainFeature.BiomeType = BiomeType.HotDesert;
                                break;
                            case Climate.Tropical:
                                terrain.PrimaryTerrainFeature.BiomeType = BiomeType.HotDesert;
                                break;
                        }              
                    }   
                    else if (terrain.PrimaryTerrainFeature.GetType() == typeof(Grassland))
                    {
                        switch (_location.ClimateArea)
                        {
                            case Climate.SubArctic:
                                terrain.PrimaryTerrainFeature.BiomeType = BiomeType.Tundra;
                                break;
                            case Climate.Temperate:
                                terrain.PrimaryTerrainFeature.BiomeType = BiomeType.TemperateGrassland;
                                break;
                            case Climate.SubTropical:
                                terrain.PrimaryTerrainFeature.BiomeType = BiomeType.TropicalGrassland;
                                break;
                            case Climate.Tropical:
                                terrain.PrimaryTerrainFeature.BiomeType = BiomeType.TropicalGrassland;
                                break;
                        }
                    }       
                }

                // change mountain biome type
                if (terrain.Type == TerrainType.MountainRange)
                {
                    foreach (Mountain mountain in ((MountainRange)terrain.PrimaryTerrainFeature).Mountains)
                    {
                        switch (_location.ClimateArea)
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

                // Change hill biome type
                if (terrain.Type == TerrainType.HillRange)
                {
                    foreach (Hill hill in ((HillRange)terrain.PrimaryTerrainFeature).Hills)
                    {
                        switch (_location.ClimateArea)
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

        public MakeClimateWarmer(Area location)
        {
            Name = "Make Climate Warmer in " + location.Name;
            _location = location;
        }
    }
}
