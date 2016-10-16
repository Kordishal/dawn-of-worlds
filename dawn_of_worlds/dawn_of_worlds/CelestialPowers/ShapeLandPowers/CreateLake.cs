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

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            // Needs at least one River it can be connected to.
            foreach (Area a in current_world.AreaGrid)
            {
                if (a.Rivers.Count > 0)
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

                if (location.AreaRegion.Landmass && location.Rivers.Count > 0)
                {
                    Lake lake = new Lake("Lake Titicaca", location, creator);
                    not_found_valid_area = false;

                    River river = location.Rivers[Main.MainLoop.RND.Next(location.Rivers.Count)];
                    river.ConnectedLakes.Add(lake);
                    lake.SourceRivers.Add(river);
                    lake.OutGoingRiver = river;
                    location.Lakes.Add(lake);
                    location.GeographicalFeatures.Add(lake);
                    location.UnclaimedTerritory.Add(lake);
                    creator.Creations.Add(lake);
                }
            }
        }


        public CreateLake()
        {
            Name = "Create Lake";
        }
    }
}
