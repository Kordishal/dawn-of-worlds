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
    class CreateForest : ShapeLand
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
                                 
                    Forest forest = new Forest("Dark Wood 01", location, creator);
                    location.Forests.Add(forest);
                    location.GeographicalFeatures.Add(forest);
                    location.UnclaimedTerritory.Add(forest);
                    creator.Creations.Add(forest);
                }
            }
        }

        public override int Weight(World current_world, Deity creator, int current_age)
        {
            return base.Weight(current_world, creator, current_age) + 20;
        }

        public CreateForest()
        {
            Name = "Create Forest";
        }
    }
}
