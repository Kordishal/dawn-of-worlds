using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldModel;

namespace dawn_of_worlds.Creations.Geography
{
    class Desert : TerrainFeatures
    {
        public Desert(string name, Province location, Deity creator) : base(name, location, creator)
        {
        }
    }
}
