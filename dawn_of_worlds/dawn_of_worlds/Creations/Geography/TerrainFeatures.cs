using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Objects;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Log;
using dawn_of_worlds.WorldModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Creations.Geography
{
    class TerrainFeatures : Creation
    {
        public BiomeType BiomeType { get; set; }

        [JsonIgnore]
        public Province Province { get; set; }

        public TerrainFeatureModifiers Modifiers { get; set; }

        [JsonIgnore]
        public City City { get; set; }

        [JsonIgnore]
        public List<Building> Buildings { get; set; }


        public TerrainFeatures(string name, Province location, Deity creator) : base(name, creator)
        {
            Province = location;
            Modifiers = new TerrainFeatureModifiers();
        }

        public virtual string printTerrainFeature(Record record)
        {
            return "The deity " + Creator.Name + " created " + this.Name + " in " + record.Year + "\n";
        }
    }

    [Serializable]
    public class TerrainFeatureModifiers
    {
        public int NaturalDefenceValue { get; set; }
        public int FortificationDefenceValue { get; set; }

        public TerrainFeatureModifiers()
        {
            NaturalDefenceValue = 0;
            FortificationDefenceValue = 0;
        }
    }

    [Serializable]
    enum BiomeType
    {
        TropicalRainforest,
        TropicalDryForest,
        TropicalGrassland,
        HotDesert,
        ColdDesert,
        TemperateGrassland,
        Scrubland,
        TemperateDeciduousForest,
        BorealForest,
        Tundra,
        PermanentFreshWaterLake,
        PermanentRiver,
        Subterranean,
        PolarDesert,
        Ocean,
    }
}
