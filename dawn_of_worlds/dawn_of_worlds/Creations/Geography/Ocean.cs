using System;
using System.Collections.Generic;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Main;
using dawn_of_worlds.WorldModel;

namespace dawn_of_worlds.Creations.Geography
{
    class Ocean : TerrainFeatures
    {
        public Ocean(string name, Province location, Deity creator) : base(name, location, creator)
        {
            BiomeType = BiomeType.Ocean;
        }
    }
}
