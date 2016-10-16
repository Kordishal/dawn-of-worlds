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
    class CreateMountain : ShapeLand
    {

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            // Needs at least one Mountainrange it can be added to.
            foreach (Area a in current_world.AreaGrid)
            {
                if (a.MountainRanges != null)
                {
                    return true;
                }
            }

            return false;
        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            bool not_found_valid_area = true;

            while (not_found_valid_area)
            {
                Area location = current_world.AreaGrid[Main.MainLoop.RND.Next(Main.MainLoop.AREA_GRID_X), Main.MainLoop.RND.Next(Main.MainLoop.AREA_GRID_Y)];

                if (location.AreaRegion.Landmass && location.MountainRanges != null)
                {
                    not_found_valid_area = false;
                    Mountain mountain = new Mountain("Mount Special", location, creator);
                    location.MountainRanges.Mountains.Add(mountain);
                    location.GeographicalFeatures.Add(mountain);
                    location.UnclaimedTerritory.Add(mountain);
                    mountain.Range = location.MountainRanges;

                    creator.Creations.Add(mountain);
                }
            }
        }


        public CreateMountain()
        {
            Name = "Create Mountain";
        }
    }
}
