using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.WorldModel;
using dawn_of_worlds.Actors;

namespace dawn_of_worlds.Creations.Geography
{
    class Mountain : TerrainFeatures
    {
        public MountainRange Range { get; set; }

        public Mountain(string name, Province location, Deity creator) : base(name, location, creator)
        {
        }
    }
}
