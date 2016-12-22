using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Log;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Creations.Geography
{
    class TerrainFeatures : Creation
    {
        public Nation NationalTerritory { get; set; }
        public Nation HuntingGround { get; set; }
        public Nation TraveledArea { get; set; }

        public void changeOwnership(Nation to)
        {
            switch (to.Type)
            {
                case NationTypes.FeudalNation:
                case NationTypes.TribalNation:
                case NationTypes.LairTerritory:
                    NationalTerritory.Territory.Remove(this);
                    NationalTerritory = to;
                    break;
                case NationTypes.NomadicTribe:
                    TraveledArea.Territory.Remove(this);
                    TraveledArea = to;
                    break;
                case NationTypes.HuntingGrounds:
                    HuntingGround.Territory.Remove(this);
                    HuntingGround = to;
                    break;
            }
        }

        public City City { get; set; }

        public Tile Location { get; set; }

        public BiomeType BiomeType { get; set; }

        public TerrainFeatures(string name, Tile location, Deity creator) : base(name, creator)
        {
            Location = location;
        }

        public virtual string printTerrainFeature(Record record)
        {
            return "The deity " + Creator.Name + " created " + this.Name + " in " + record.Year + "\n";
        }
    }

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
