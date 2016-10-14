using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Creations.Organisations;

namespace dawn_of_worlds.CelestialPowers.RaceCreationPowers.SubRaceCreationPowers
{
    class CreateDeepDwarves : CreateSubRace
    {
        public static bool notCreatedDeepDwarves = true;

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            if (notCreatedDeepDwarves && !CreateDwarves.notCreatedDwarves)
                return true;
            else
                return false;
        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            bool not_found_valid_area = true;

            while (not_found_valid_area)
            {
                Area location = current_world.AreaGrid[Main.MainLoop.RND.Next(Main.MainLoop.AREA_GRID_X), Main.MainLoop.RND.Next(Main.MainLoop.AREA_GRID_Y)];

                if (location.AreaRegion.Landmass && neighbourAreaHasMainRace(location.Neighbours) && location.MountainRanges != null)
                {
                    not_found_valid_area = false;

                    Organisation creator_worhip_order = new Organisation("Deep Dwarves Creator Worshippers", creator, OrganisationType.ReligiousOrder, OrganisationPurpose.WorshipCreator);

                    Race deep_dwarves = new Race("Deep Dwarves", creator, location, creator_worhip_order);
                    location.Inhabitants.Add(deep_dwarves);
                    deep_dwarves.isSubRace = true;

                    foreach (Area a in current_world.AreaGrid)
                    {
                        foreach (Race r in a.Inhabitants)
                        {
                            if (r.Name == "Dwarves")
                            {
                                deep_dwarves.MainRace = r;
                                r.SubRaces.Add(deep_dwarves);
                            }
                        }
                    }

                    creator.CreatedRaces.Add(deep_dwarves);
                    creator.CreatedOrganisations.Add(creator_worhip_order);

                    notCreatedDeepDwarves = false;

                }
            }
        }

        private bool neighbourAreaHasMainRace(Area[] neighbours)
        {
            foreach (Area a in neighbours)
            {
                if (a != null)
                {
                    foreach (Race r in a.Inhabitants)
                    {
                        if (r.Name == "Dwarves")
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public CreateDeepDwarves()
        {
            Name = "Create Deep Dwarves";
        }
    }
}
