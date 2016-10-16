using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Geography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Creations.Organisations
{
    class City : Creation
    {

        public Nation Owner { get; set; }

        public GeographicalFeature CityLocation { get; set; }

        // A city can only raise one army per turn.
        public bool not_hasRaisedArmy { get; set; }


        public City(string name, Deity creator): base(name, creator)
        {
            not_hasRaisedArmy = true;
        }
    }
}
