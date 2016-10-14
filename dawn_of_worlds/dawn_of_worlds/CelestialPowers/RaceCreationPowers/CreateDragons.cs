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
    class CreateDragons : CreateRace
    {
        public static bool notCreatedDragons = true;

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            return notCreatedDragons;
        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            bool not_found_valid_area = true;

            while (not_found_valid_area)
            {
                Area location = current_world.AreaGrid[Main.MainLoop.RND.Next(Main.MainLoop.AREA_GRID_X), Main.MainLoop.RND.Next(Main.MainLoop.AREA_GRID_Y)];

                if (location.AreaRegion.Landmass)
                {
                    not_found_valid_area = false;

                    Race dragons = new Race("Dragons", creator, location);
                    location.Inhabitants.Add(dragons);
                    notCreatedDragons = false;

                }
            }
        }

        public CreateDragons()
        {
            Name = "Create Dragons";
        }
    }
}
