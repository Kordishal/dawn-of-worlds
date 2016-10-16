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
    class CreateMountainRange : ShapeLand
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
                    if (location.MountainRanges != null)
                    {
                        Mountain mountain = new Mountain("Mountain First", location, creator);
                        location.MountainRanges.Mountains.Add(mountain);
                        location.GeographicalFeatures.Add(mountain);
                        location.UnclaimedTerritory.Add(mountain);
                        mountain.Range = location.MountainRanges;

                        creator.Creations.Add(mountain);
                    }
                    else
                    {
                        MountainRange mountain_range = new MountainRange("Mountain Range Halleluja", location, creator);
                        location.MountainRanges = mountain_range;

                        Mountain mountain = new Mountain("Mountain First", location, creator);
                        mountain_range.Mountains.Add(mountain);
                        mountain.Range = mountain_range;

                        creator.Creations.Add(mountain_range);
                        creator.Creations.Add(mountain);
                    }
                }
            }
        }

        public CreateMountainRange()
        {
            Name = "Create Mountain Range";
        }
    }
}
