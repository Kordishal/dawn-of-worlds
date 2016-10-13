using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Creations.Geography
{
    class GeographcialCreation : Creation
    {
        public Area Location { get; set; }

        public GeographcialCreation(string name, Area location, Deity creator) : base(name, creator)
        {
            Location = location;
        }
    }
}
