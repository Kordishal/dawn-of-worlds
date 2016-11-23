using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;

namespace dawn_of_worlds.Creations.Geography
{
    class Grassland : Terrain
    {
        public Grassland(string name, Area location, Deity creator) : base(name, location, creator)
        {
        }
    }
}
