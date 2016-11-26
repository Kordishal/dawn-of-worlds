using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Organisations;
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
        public Nation Owner { get; set; }

        public City City { get; set; }
        public City SphereOfInfluenceCity { get; set; }

        public Terrain Location { get; set; }

        public BiomeType BiomeType { get; set; }

        public TerrainFeatures(string name, Terrain location, Deity creator) : base(name, creator)
        {
            Location = location;
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
        MediterraneanScrubland,
        TemperateDeciduousForest,
        BorealForest,
        Tundra,
        PermanentFreshWaterLake,
        PermanentRiver,
        Subterranean,
    }
}
