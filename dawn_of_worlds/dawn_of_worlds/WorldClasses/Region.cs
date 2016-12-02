using dawn_of_worlds.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.WorldClasses
{
    class Region
    {
        public string Name { get; set; }

        public bool Landmass { get; set; }

        public World RegionWorld { get; set; }
        public List<Area> RegionAreas { get; set; }


        public Region(World world, int num_areas)
        {
            Name = Constants.Names.GetName("region");
            RegionWorld = world;
            RegionAreas = new List<Area>();
            Landmass = Main.Constants.Random.Next(20) < 15 ? true : false;

            for (int j = 0; j < num_areas; j++)
            {
                RegionAreas.Add(new Area(this));
            }


        }

    }
}
