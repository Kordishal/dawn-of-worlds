using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Actors;

namespace dawn_of_worlds.Creations.Geography
{
    class River : TerrainFeatures
    {
        public MountainRange Spring { get; set; }

        public List<Province> Riverbed { get; set; }
        public Province Destination { get; set; }

        public Lake DestinationLake { get; set; }
        public River DestinationRiver { get; set; }

        public List<Lake> ConnectedLakes { get; set; }
        public List<River> SourceRivers { get; set; }

        public River(string name, Province location, Deity creator) : base(name, location, creator)
        {
            Riverbed = new List<Province>();
            ConnectedLakes = new List<Lake>();
            SourceRivers = new List<River>();
        }
    }
}
