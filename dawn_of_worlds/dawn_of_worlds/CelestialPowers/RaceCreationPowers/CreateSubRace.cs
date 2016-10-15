using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.CelestialPowers.CommandRacePowers;

namespace dawn_of_worlds.CelestialPowers.RaceCreationPowers
{
    class CreateSubRace : Power
    {
        private Race _created_race;
        private Race _main_race;

        public override int Cost(int current_age)
        {
            switch (current_age)
            {
                case 1:
                    return 12;
                case 2:
                    return 4;
                case 3:
                    return 10;
                default:
                    return 40;
            }
        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            bool not_found_valid_area = true;

            while (not_found_valid_area)
            {
                Area location = current_world.AreaGrid[Main.MainLoop.RND.Next(Main.MainLoop.AREA_GRID_X), Main.MainLoop.RND.Next(Main.MainLoop.AREA_GRID_Y)];

                if (location.AreaRegion.Landmass && neighbourAreaHasMainRace(location.Neighbours))
                {
                    not_found_valid_area = false;

                    Organisation creator_worhip_order = new Organisation(_created_race.Name + "Creator Worshippers", creator, OrganisationType.ReligiousOrder, OrganisationPurpose.WorshipCreator);

                    location.Inhabitants.Add(_created_race);

                    _created_race.HomeArea = location;
                    _created_race.SettledAreas.Add(location);
                    _created_race.isSubRace = true;
                    _created_race.MainRace = _main_race;

                    _main_race.SubRaces.Add(_created_race);

                    creator.CreatedRaces.Add(_created_race);
                    creator.CreatedOrganisations.Add(creator_worhip_order);

                    creator.Powers.Add(new SettleArea(_created_race));
                    creator.Powers.Add(new FoundNation(_created_race));

                    foreach (Deity d in current_world.Deities)
                    {
                        d.Powers.Remove(this);
                    }
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
                        if (r.Equals(_main_race))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public override int Weight(World current_world, Deity creator, int current_age)
        {
            switch (current_age)
            {
                case 1:
                    return 10;
                case 2:
                    return 200;
                case 3:
                    return 50;
                default:
                    return 100;
            }
        }


        public CreateSubRace(Race created_race, Race main_race)
        {
            Name = "Create Subrace: " + created_race.Name;
            _created_race = created_race;
            _main_race = main_race;
        }
    }
}
