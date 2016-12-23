using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;

namespace dawn_of_worlds.Creations.Geography
{
    class HillRange : TerrainFeatures
    {

        public List<Hill> Hills { get; set; }

        public HillRange(string name, Province location, Deity creator) : base(name, location, creator)
        {
            Hills = new List<Hill>();
        }
    }
}
