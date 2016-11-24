using dawn_of_worlds.Actors;
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

        public Deity Creator { get; set; }


        public Creation(string name, Deity creator)
        {
            _identifier = id;
            id++;

            Name = name;
            Creator = creator;
        }



        public override string ToString()
        {
            return Name;
        }
    }
}
