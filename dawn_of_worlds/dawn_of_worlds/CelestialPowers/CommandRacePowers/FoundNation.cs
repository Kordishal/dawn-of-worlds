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
using dawn_of_worlds.CelestialPowers.CommandNationPowers;

namespace dawn_of_worlds.CelestialPowers.CommandRacePowers
{
    class FoundNation : CommandRace
    {

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            foreach (Area a in _commanded_race.SettledAreas)
            {
                if (a.UnclaimedTerritory.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {

            // Find an area with unclaimed space of the settled areas. 
            Area location = null;
            while (location == null)
            {
                location = _commanded_race.SettledAreas[Main.MainLoop.RND.Next(_commanded_race.SettledAreas.Count)];
                // At least one unclaimed territory necessary to found a nation.
                if (location.UnclaimedTerritory.Count == 0)
                    location = null;
            }

            // The nation to be founded.
            Nation founded_nation = new Nation("Nation of " + _commanded_race.Name, creator);
            founded_nation.FoundingRace = _commanded_race;

            // Decide on the territory it should be settled on. Currently random.
            GeographicalFeature territory = null;
            while (territory == null)
            {
                territory = location.UnclaimedTerritory[Main.MainLoop.RND.Next(location.UnclaimedTerritory.Count)];      
            }
            founded_nation.Territory.Add(territory);
            founded_nation.TerritoryAreas.Add(location);
            location.UnclaimedTerritory.Remove(territory);
            territory.Owner = founded_nation;


            // Add nation to the creator and Powers related to this nation.
            creator.FoundedNations.Add(founded_nation);
            creator.Powers.Add(new ExpandTerritory(founded_nation));
            creator.Powers.Add(new CreateCity(founded_nation));
            creator.Powers.Add(new RaiseArmy(founded_nation));
            creator.Powers.Add(new FormAlliance(founded_nation));

            // Add nation to world overview
            current_world.Nations.Add(founded_nation);
            location.Nations.Add(founded_nation);
        }

        public FoundNation (Race command_race) : base(command_race)
        {
            Name = "Found Nation: " + command_race.Name;
        }
    }
}
