﻿using dawn_of_worlds.Actors;
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
        public Province Province { get; set; }

        public City City { get; set; }
        public BiomeType BiomeType { get; set; }

        public TerrainFeatures(string name, Province location, Deity creator) : base(name, creator)
        {
            Province = location;
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
