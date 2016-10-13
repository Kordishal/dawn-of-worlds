using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Actors;

namespace dawn_of_worlds.Creations.Geography
{
    class MountainRange : GeographcialCreation
    {

        public List<Mountain> Mountains { get; set; }

        public MountainRange(string name, Area location, Deity creator) : base(name, location, creator)
        {
            Mountains = new List<Mountain>();

        }
    }
}
