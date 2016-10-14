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
    class CreateGiants : CreateRace
    {
        public static bool notCreatedGiants = true;

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            return notCreatedGiants;
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

                    Organisation creator_worhip_order = new Organisation("Giants Creator Worshippers", creator, OrganisationType.ReligiousOrder, OrganisationPurpose.WorshipCreator);

                    Race giants = new Race("Giants", creator, location, creator_worhip_order);
                    location.Inhabitants.Add(giants);
                    notCreatedGiants = false;

                    creator.CreatedRaces.Add(giants);
                    creator.CreatedOrganisations.Add(creator_worhip_order);
                }
            }
        }

        public CreateGiants()
        {
            Name = "Create Giants";
        }
    }
}
