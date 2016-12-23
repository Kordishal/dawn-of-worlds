using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Actors;

namespace dawn_of_worlds.Creations.Geography
{
    class Lake : TerrainFeatures
    {

        public River OutGoingRiver { get; set; }

        public List<River> SourceRivers { get; set; }

        public Lake(string name, Province location, Deity creator) : base(name, location, creator)
        {
            SourceRivers = new List<River>();
        }
    }
}
