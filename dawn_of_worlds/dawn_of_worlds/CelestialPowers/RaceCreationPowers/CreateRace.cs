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
    class CreateRace : Power
    {
        private Race _created_race;

        public override int Cost(int current_age)
        {
            switch (current_age)
            {
                case 1:
                    return 22;
                case 2:
                    return 6;
                case 3:
                    return 15;
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

                if (location.AreaRegion.Landmass)
                {
                    not_found_valid_area = false;
                    
                    // Each race has an order dedicated to worship their creator.
                    Organisation creator_worhip_order = new Organisation(_created_race.Name + " Worship Order", creator, OrganisationType.ReligiousOrder, OrganisationPurpose.WorshipCreator);
                    _created_race.OriginOrder = creator_worhip_order;

                    // The created race is settled 
                    _created_race.HomeArea = location;
                    _created_race.SettledAreas.Add(location);
                    
                    // Tells the Area that someone is living here.
                    location.Inhabitants.Add(_created_race);

                    // Tells the creator what they have created and adds the powers to command this race.
                    creator.CreatedRaces.Add(_created_race);
                    creator.CreatedOrganisations.Add(creator_worhip_order);

                    creator.Powers.Add(new SettleArea(_created_race));
                    creator.Powers.Add(new FoundNation(_created_race));

                   
                    // Remove this power from all deities, as every race can only be created once.
                    foreach (Deity d in current_world.Deities)
                    {
                        d.Powers.Remove(this);
                        // add the powers to create all the subraces of this main race to all the deities.
                        foreach (Race r in _created_race.PossibleSubRaces)
                        {
                            d.Powers.Add(new CreateSubRace(r, _created_race));
                        }
                    }

                }
            }
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

        public CreateRace(Race created_race)
        {
            Name = "Create Race: " + created_race.Name;
            _created_race = created_race;
        }
    }
}
