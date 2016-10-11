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

        public List<Area> RegionAreas { get; set; }


        public Region(int min_areas, int max_areas)
        {
            RegionAreas = new List<Area>();
            Landmass = Main.MainLoop.RND.Next(10) < 5 ? true : false;

            for (int j = 0; j < Main.MainLoop.RND.Next(min_areas, max_areas); j++)
            {
                RegionAreas.Add(new Area());
            }


        }

    }
}
