using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Creations.Organisations;

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

                    Organisation creator_worhip_order = new Organisation("Dwarves Creator Worshippers", creator, OrganisationType.ReligiousOrder, OrganisationPurpose.WorshipCreator);

                    Race dwarves = new Race("Dwarves", creator, location, creator_worhip_order);
                    location.Inhabitants.Add(dwarves);
                    notCreatedDwarves = false;

                    creator.CreatedRaces.Add(dwarves);
                    creator.CreatedOrganisations.Add(creator_worhip_order);
                }
            }
        }

        public CreateDwarves()
        {
            Name = "Create Dwarves";
        }
    }
}
