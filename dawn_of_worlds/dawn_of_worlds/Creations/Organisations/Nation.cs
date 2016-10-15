using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Creations.Organisations
{
    class Nation : Creation
    {

        public Race FoundingRace { get; set; }

        public List<GeographcialCreation> Territory { get; set; }



        public Nation(string name, Deity creator) :base(name, creator)
        {
            Territory = new List<GeographcialCreation>();
        }
    }
}
