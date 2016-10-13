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
    class CreateRiver : ShapeLand
    {
        public override void Effect(World current_world, Deity creator, int current_age)
        {
            bool not_found_valid_area = true;

            while (not_found_valid_area)
            {
                Area location = current_world.AreaGrid[Main.MainLoop.RND.Next(Main.MainLoop.AREA_GRID_X), Main.MainLoop.RND.Next(Main.MainLoop.AREA_GRID_Y)];

                if (location.MountainRanges != null)
                {
                    not_found_valid_area = false;
                    River river = new River("The River", location, creator);
                    location.Rivers.Add(river);
                    river.Spring = location.MountainRanges;


                }
            }
        }
    }
}
