﻿using dawn_of_worlds.Actors;
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
    abstract class Creation
    {
        private static int id = 0;

        private int _identifier { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public Deity Creator { get; set; }

        public List<CreationTag> Tags { get; set; }

        public Creation(string name, Deity creator)
        {
            _identifier = id;
            id++;

            Name = name;
            Creator = creator;
            Tags = new List<CreationTag>();
        }



        public override string ToString()
        {
            return Name;
        }
    }
}
