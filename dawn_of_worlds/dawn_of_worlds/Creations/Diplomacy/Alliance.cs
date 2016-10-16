using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Organisations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Creations.Diplomacy
{
    class Alliance : Creation
    {
        public List<Nation> Members { get; set; }

        public Alliance(string name, Deity creator) : base(name, creator)
        {
            Members = new List<Nation>();
        } 
    }
}
