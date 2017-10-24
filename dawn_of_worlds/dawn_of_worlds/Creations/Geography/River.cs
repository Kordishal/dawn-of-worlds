using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Actors;
using Newtonsoft.Json;

namespace dawn_of_worlds.Creations.Geography
{
    class River : TerrainFeatures
    {
        [JsonIgnore]
        public MountainRange Spring { get; set; }

        [JsonIgnore]
        public List<Province> Riverbed { get; set; }

        [JsonIgnore]
        public Province Destination { get; set; }

        [JsonIgnore]
        public Lake DestinationLake { get; set; }

        [JsonIgnore]
        public River DestinationRiver { get; set; }

        [JsonIgnore]
        public List<Lake> ConnectedLakes { get; set; }

        [JsonIgnore]
        public List<River> SourceRivers { get; set; }

        public River(string name, Province location, Deity creator) : base(name, location, creator)
        {
            Riverbed = new List<Province>();
            ConnectedLakes = new List<Lake>();
            SourceRivers = new List<River>();
        }
    }
}
