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
    class Terrain : Creation
    {
        public Nation Owner { get; set; }

        public City City { get; set; }
        public City SphereOfInfluenceCity { get; set; }

        public Area Location { get; set; }

        public BiomeType BiomeType { get; set; }

        public Terrain(string name, Area location, Deity creator) : base(name, creator)
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
