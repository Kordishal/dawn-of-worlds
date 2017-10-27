using dawn_of_worlds.Actors;
using dawn_of_worlds.Effects;
using dawn_of_worlds.Names;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Creations
{

    // TODO: Make identifiers public and make sure each creation type can be converted to json to be 
    // importet into elasticsearch.
    abstract class Creation
    {
        private static int id = 0;

        public int Identifier { get; set; }

        public Random rnd { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public Deity Creator { get; set; }

        public List<string> Tags { get; set; }

        public Creation(string name, Deity creator)
        {
            Identifier = id;
            id++;

            Name = name;
            Creator = creator;
            Tags = new List<string>();
            rnd = new Random(Identifier);
        }



        public override string ToString()
        {
            return Name;
        }
    }
}
