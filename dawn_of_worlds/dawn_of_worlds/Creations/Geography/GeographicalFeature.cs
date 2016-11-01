using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Creations.Geography
{
    class GeographicalFeature : Creation
    {

        public Nation Owner { get; set; }

        public City City { get; set; }
        public City SphereOfInfluenceCity { get; set; }

        public Area Location { get; set; }

        public GeographicalFeature(string name, Area location, Deity creator) : base(name, creator)
        {
            Location = location;
        }
    }
}
