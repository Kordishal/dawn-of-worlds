using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Geography;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    class CreateLake : ShapeLand
    {
        public override void Effect(World current_world, Deity creator, int current_age)
        {
            bool not_found_valid_area = true;

            while (not_found_valid_area)
            {
                Area location = current_world.AreaGrid[Main.MainLoop.RND.Next(Main.MainLoop.AREA_GRID_X), Main.MainLoop.RND.Next(Main.MainLoop.AREA_GRID_Y)];

                if (location.AreaRegion.Landmass)
                {
                    not_found_valid_area = false;
                    if (location.Lakes.Count > 0)
                    {
                        if (Main.MainLoop.RND.Next(100) < 50)
                        {
                            Lake lake = location.Lakes[Main.MainLoop.RND.Next(location.Lakes.Count)];
                            lake.Size += Main.MainLoop.RND.Next(5);
                        }
                        else
                        {
                            Lake lake = new Lake("Lake Titicaca", location, creator);
                            location.Lakes.Add(lake);
                        }
                    }
                    else
                    {
                        Lake lake = new Lake("Lake Victoria", location, creator);
                        location.Lakes.Add(lake);
                    }
                }
            }
        }
    }
}
