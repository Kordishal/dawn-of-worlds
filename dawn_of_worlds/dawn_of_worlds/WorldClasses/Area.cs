using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.WorldClasses
{
    class Area
    {
        public string Name { get; set; }

        public SystemCoordinates Coordinates { get; set; }

        public Region RegionArea { get; set; }
        public List<Tile> Tiles { get; set; }

        public Area(Region region)
        {
            Name = Constants.Names.GetName("area");
            RegionArea = region;

            Tiles = new List<Tile>();   
        }

        public override string ToString()
        {
            return "Area: " + Name;
        }
    }
}
