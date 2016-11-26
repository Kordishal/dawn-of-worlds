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
        public List<Terrain> TerrainArea { get; set; }

        public Climate ClimateArea { get; set; }


        public Area(Region region)
        {
            Name = Constants.Names.GetName("area");
            RegionArea = region;

            ClimateArea = new Climate();           
        }

        public override string ToString()
        {
            return "Area: " + Name;
        }

        public string printArea()
        {
            string result = "";
            result += "Name: " + Name + "\n";
            result += "Region: " + RegionArea.Name + "\n";
            result += "Ocean: " + (RegionArea.Landmass ? "no" : "yes") + "\n";
            result += "Climate: " + ClimateArea.ToString() + "\n";



            return result;
        }
    }
}
