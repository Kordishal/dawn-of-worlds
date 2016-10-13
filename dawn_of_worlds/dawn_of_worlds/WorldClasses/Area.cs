using dawn_of_worlds.Creations.Geography;
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

        public List<Forest> Forests { get; set; }

        public List<Lake> Lakes { get; set; }
        public List<River> Rivers { get; set; }

        public MountainRange MountainRanges { get; set; }

        public Area(Region region)
        {
            Name = id.ToString();
            id += 1;
            AreaRegion = region;
            Forests = new List<Forest>();
            Lakes = new List<Lake>();
            Rivers = new List<River>();
        }


        public override string ToString()
        {
            return "Area: " + Name;
        }
    }
}
