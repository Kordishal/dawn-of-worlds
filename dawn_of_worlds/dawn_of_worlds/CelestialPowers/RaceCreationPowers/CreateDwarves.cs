using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Inhabitants;

namespace dawn_of_worlds.CelestialPowers.RaceCreationPowers
{
    class CreateDwarves : CreateRace
    {
        public static bool notCreatedDwarves = true;

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            return notCreatedDwarves;
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

                    Race dwarves = new Race("Dwarves", creator, location);
                    location.Inhabitants.Add(dwarves);
                    notCreatedDwarves = false;

                }
            }
        }

        public CreateDwarves()
        {
            Name = "Create Dwarves";
        }
    }
}
