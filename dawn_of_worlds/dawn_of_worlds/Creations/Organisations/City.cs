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
        // Owner Nation
        public Nation Owner { get; set; }

        // Territory
        public Terrain CityLocation { get; set; }
        public List<Terrain> CitySphereOfÌnfluence { get; set; }

        // A city can only raise one army per turn.
        public bool not_hasRaisedArmy { get; set; }


        public City(string name, Deity creator): base(name, creator)
        {
            CitySphereOfÌnfluence = new List<Terrain>();
            not_hasRaisedArmy = true;
        }
    }
}
