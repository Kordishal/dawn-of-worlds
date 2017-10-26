using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Creations.Civilisations;
using dawn_of_worlds.Effects;
using Newtonsoft.Json;

namespace dawn_of_worlds.WorldModel
{
    [Serializable]
    class Province
    {
        public static int _id_counter = 0;

        public int Identifier { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
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

        [JsonIgnore]
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
            SettledRaces = new List<Race>();
            SecondaryTerrainFeatures = new List<TerrainFeatures>();
            isDefault = true;
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
