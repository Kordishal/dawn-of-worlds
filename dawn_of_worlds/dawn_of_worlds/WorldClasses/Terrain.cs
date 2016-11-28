using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Creations.Organisations;

namespace dawn_of_worlds.WorldClasses
{
    class Terrain
    {
        public string Name { get; set; }

        public Area Area { get; set; }
        public SystemCoordinates Coordinates { get; set; }

        public TerrainType Type { get; set; }

        public bool isDefault { get; set; }
        public TerrainFeatures PrimaryTerrainFeature { get; set; }
        public List<TerrainFeatures> SecondaryTerrainFeatures { get; set; }

        public bool hasRivers
        {
            get
            {
                foreach (TerrainFeatures terrain in SecondaryTerrainFeatures)
                {
                    if (terrain.GetType() == typeof(River))
                        return true;
                }
                return false;
            }
        }

        public List<Race> SettledRaces { get; set; }

        public List<TerrainFeatures> UnclaimedTerritory { get; set; }

        public Terrain(Area area)
        {
            Name = Constants.Names.GetName("area");
            Area = area;
            SettledRaces = new List<Race>();
            SecondaryTerrainFeatures = new List<TerrainFeatures>();
            UnclaimedTerritory = new List<TerrainFeatures>();
            isDefault = true;
            // Establish a grassland as a base terrain on continents for races/nations to be built on it.
            if (area != null && area.RegionArea.Landmass)
            {
                Type = TerrainType.Plain;
                PrimaryTerrainFeature = new Grassland(Constants.Names.GetName("grasslands"), this, null);
                UnclaimedTerritory.Add(PrimaryTerrainFeature);

                switch (this.Area.ClimateArea)
                {
                    case Climate.Arctic:
                        PrimaryTerrainFeature.BiomeType = BiomeType.Tundra;
                        break;
                    case Climate.SubArctic:
                        PrimaryTerrainFeature.BiomeType = BiomeType.Tundra;
                        break;
                    case Climate.Temperate:
                        PrimaryTerrainFeature.BiomeType = BiomeType.TemperateGrassland;
                        break;
                    case Climate.SubTropical:
                        PrimaryTerrainFeature.BiomeType = BiomeType.TropicalGrassland;
                        break;
                    case Climate.Tropical:
                        PrimaryTerrainFeature.BiomeType = BiomeType.TropicalGrassland;
                        break;
                }

            }           
            else
                Type = TerrainType.Ocean;


        }
    }


    enum TerrainType
    {
        MountainRange,
        HillRange,
        Plain,
        Ocean,
        Island,
        Unknown,
    }
}
