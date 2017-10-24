﻿using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Creations.Civilisations;
using dawn_of_worlds.Effects;
using Newtonsoft.Json;

namespace dawn_of_worlds.WorldClasses
{
    [Serializable]
    class Province
    {
        public static int _id_counter = 0;

        public int Identifier { get; set; }

        public string Name { get; set; }

        public Area Area { get; set; }

        [JsonIgnore]
        public SystemCoordinates Coordinates { get; set; }

        public TerrainType Type { get; set; }

        public Climate LocalClimate { get; set; }

        public ClimateModifier LocalClimateModifier { get; set; }

        public bool isDefault { get; set; }

        public TerrainFeatures PrimaryTerrainFeature { get; set; }

        [JsonIgnore]
        public List<TerrainFeatures> SecondaryTerrainFeatures { get; set; }

        public bool hasOwner { get { return Owner != null; } }

        [JsonIgnore]
        public Civilisation Owner { get; set; }

        [JsonIgnore]
        public List<Civilisation> NomadicPresence { get; set; }

        [JsonIgnore]
        public List<Modifier> ProvincialModifiers { get; set; }

        [JsonIgnore]
        public List<Resource> ProvincialResources { get; set; }

        public void ChangeOwnership(Civilisation winner)
        {
            List<City> local_cities = Owner.Cities.FindAll(x => x.TerrainFeature.Province == this);
            foreach (City city in local_cities)
            {
                Owner.Cities.Remove(city);
                city.Owner = winner;
            }
            Owner.Territory.Remove(this);
            Owner = winner;
        }

        [JsonIgnore]
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

        public Province(Area area, SystemCoordinates coordinates)
        {
            Identifier = _id_counter;
            _id_counter += 1;

            Name = Program.GenerateNames.GetName("area_names");
            Area = area;
            Coordinates = coordinates;
            NomadicPresence = new List<Civilisation>();
            ProvincialModifiers = new List<Modifier>();
        }

        public void initialize()
        {
            SettledRaces = new List<Race>();
            SecondaryTerrainFeatures = new List<TerrainFeatures>();
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
            if (Area != null && Area.Type == AreaType.Continent)
            {
                Type = TerrainType.Plain;
                switch (LocalClimate)
                {
                    case Climate.Arctic:
                        PrimaryTerrainFeature = new Desert(Program.GenerateNames.GetName("desert_names"), this, null);
                        PrimaryTerrainFeature.BiomeType = BiomeType.PolarDesert;
                        ProvincialModifiers.Add(new Modifier(ModifierCategory.Province, ModifierTag.Permafrost));
                        break;
                    case Climate.SubArctic:
                        PrimaryTerrainFeature = new Grassland(Program.GenerateNames.GetName("grassland_names"), this, null);
                        PrimaryTerrainFeature.BiomeType = BiomeType.Tundra;
                        break;
                    case Climate.Temperate:
                        PrimaryTerrainFeature = new Grassland(Program.GenerateNames.GetName("grassland_names"), this, null);
                        PrimaryTerrainFeature.BiomeType = BiomeType.TemperateGrassland;
                        break;
                    case Climate.SubTropical:
                        PrimaryTerrainFeature = new Grassland(Program.GenerateNames.GetName("grassland_names"), this, null);
                        PrimaryTerrainFeature.BiomeType = BiomeType.TropicalGrassland;
                        break;
                    case Climate.Tropical:
                        PrimaryTerrainFeature = new Grassland(Program.GenerateNames.GetName("grassland_names"), this, null);
                        PrimaryTerrainFeature.BiomeType = BiomeType.TropicalGrassland;
                        break;
                }
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
