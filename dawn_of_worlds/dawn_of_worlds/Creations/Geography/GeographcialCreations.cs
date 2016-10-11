using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Creations.Geography
{
    class GeographcialCreations : Creation
    {
        public Area Location { get; set; }

        public GeographcialCreations(string name, Area location) : base(name)
        {
            Location = location;
        }
    }
}
