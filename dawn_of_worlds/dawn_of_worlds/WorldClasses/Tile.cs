using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Creations.Organisations;

namespace dawn_of_worlds.WorldClasses
{
    class Tile
    {
        public string Name { get; set; }

        public Area Area { get; set; }
        public SystemCoordinates Coordinates { get; set; }

        public TerrainType Type { get; set; }

        public Climate LocalClimate { get; set; }
        public ClimateModifier LocalClimateModifier { get; set; }

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

        public List<TerrainFeatures> UnclaimedTerritories { get; set; }
        public List<TerrainFeatures> UnclaimedTravelAreas { get; set; }
        public List<TerrainFeatures> UnclaimedHuntingGrounds { get; set; }

        public Tile(Area area, SystemCoordinates coordinates)
        {
            Name = Constants.Names.GetName("area");
            Area = area;
            Coordinates = coordinates;
        }

        public void initialize()
        {
            SettledRaces = new List<Race>();
            SecondaryTerrainFeatures = new List<TerrainFeatures>();
            UnclaimedTerritories = new List<TerrainFeatures>();
            UnclaimedTravelAreas = new List<TerrainFeatures>();
            UnclaimedHuntingGrounds = new List<TerrainFeatures>();
            isDefault = true;

            if (Coordinates.X < Constants.ARCTIC_CLIMATE_BORDER)
                LocalClimate = Climate.Arctic;
            else if (Coordinates.X < Constants.SUB_ARCTIC_CLIMATE_BORDER)
                LocalClimate = Climate.SubArctic;
            else if (Coordinates.X < Constants.TEMPERATE_CLIMATE_BORDER)
                LocalClimate = Climate.Temperate;
            else if (Coordinates.X < Constants.SUB_TROPICAL_CLIMATE_BORDER)
                LocalClimate = Climate.SubTropical;
            else if (Coordinates.X < Constants.TROPICAL_CLIMATE_BORDER)
                LocalClimate = Climate.Tropical;

            LocalClimateModifier = ClimateModifier.None;

            // Establish a grassland as a base terrain on continents for races/nations to be built on it.
            if (Area != null && Area.RegionArea.Landmass)
            {
                Type = TerrainType.Plain;
                switch (LocalClimate)
                {
                    case Climate.Arctic:
                        PrimaryTerrainFeature = new Desert(Constants.Names.GetName("deserts"), this, null);
                        PrimaryTerrainFeature.BiomeType = BiomeType.PolarDesert;
                        break;
                    case Climate.SubArctic:
                        PrimaryTerrainFeature = new Grassland(Constants.Names.GetName("grasslands"), this, null);
                        PrimaryTerrainFeature.BiomeType = BiomeType.Tundra;
                        break;
                    case Climate.Temperate:
                        PrimaryTerrainFeature = new Grassland(Constants.Names.GetName("grasslands"), this, null);
                        PrimaryTerrainFeature.BiomeType = BiomeType.TemperateGrassland;
                        break;
                    case Climate.SubTropical:
                        PrimaryTerrainFeature = new Grassland(Constants.Names.GetName("grasslands"), this, null);
                        PrimaryTerrainFeature.BiomeType = BiomeType.TropicalGrassland;
                        break;
                    case Climate.Tropical:
                        PrimaryTerrainFeature = new Grassland(Constants.Names.GetName("grasslands"), this, null);
                        PrimaryTerrainFeature.BiomeType = BiomeType.TropicalGrassland;
                        break;
                }

                UnclaimedTerritories.Add(PrimaryTerrainFeature);
                UnclaimedTravelAreas.Add(PrimaryTerrainFeature);
                UnclaimedHuntingGrounds.Add(PrimaryTerrainFeature);

            }
            else
            {
                Type = TerrainType.Ocean;
                PrimaryTerrainFeature = new Ocean("The Ocean", this, null);
            }
                
        }

        public override string ToString()
        {
            return "Terrain: " + Name;
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
