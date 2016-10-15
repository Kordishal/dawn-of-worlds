using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Creations.Geography;

namespace dawn_of_worlds.CelestialPowers.CommandRacePowers
{
    class FoundNation : CommandRace
    {
        public override void Effect(World current_world, Deity creator, int current_age)
        {
            Area location = null;
            bool not_found_valid_area = true;

            while (not_found_valid_area)
            {
                location = current_world.AreaGrid[Main.MainLoop.RND.Next(Main.MainLoop.AREA_GRID_X), Main.MainLoop.RND.Next(Main.MainLoop.AREA_GRID_Y)];

                if (location.AreaRegion.Landmass && location.Inhabitants.Contains(_commanded_race))
                {
                    not_found_valid_area = false;
                }
            }

            Nation founded_nation = new Nation("Nation of " + _commanded_race.Name, creator);

            founded_nation.FoundingRace = _commanded_race;

            List<GeographcialCreation> possible_territories = new List<GeographcialCreation>();
            possible_territories.AddRange(location.Forests);
            possible_territories.AddRange(location.Lakes);
            if (location.MountainRanges != null)
                possible_territories.AddRange(location.MountainRanges.Mountains);
            possible_territories.AddRange(location.Rivers);

            bool not_found_valid_founding_territory = true;
            int counter = 0;
            while (not_found_valid_founding_territory)
            {
                GeographcialCreation current = possible_territories[Main.MainLoop.RND.Next(possible_territories.Count)];
                if (current.Owner == null)
                {
                    founded_nation.Territory.Add(current);
                    current.Owner = founded_nation;
                }
                counter += 1;

                if (counter >= 100)
                {
                    Console.WriteLine("Could not find a valid founding territory for " + founded_nation.Name + " in " + location.Name + ".");
                    break;
                }
            }

            creator.FoundedNations.Add(founded_nation);

        }

        public FoundNation (Race command_race) : base(command_race)
        {
            Name = "Found Nation: " + command_race.Name;
        }
    }
}
