using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.WorldClasses
{
    class Area
    {

        private static int id = 0;

        public string Name { get; set; }

        public Region AreaRegion { get; set; }

        public Area(Region region)
        {
            Name = id.ToString();
            id += 1;
            AreaRegion = region;
        }


        public override string ToString()
        {
            return "Area: " + Name;
        }
    }
}
