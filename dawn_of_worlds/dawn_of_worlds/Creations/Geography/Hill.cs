using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using Newtonsoft.Json;

namespace dawn_of_worlds.Creations.Geography
{
    class Hill : TerrainFeatures
    {
        [JsonIgnore]
        public HillRange Range { get; set; }

        public Hill(string name, Province location, Deity creator) : base(name, location, creator)
        {
        }
    }
}
