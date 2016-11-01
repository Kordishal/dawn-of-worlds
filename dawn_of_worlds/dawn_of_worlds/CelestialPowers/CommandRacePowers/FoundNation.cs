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

            // Mark territory as claimed.
            location.UnclaimedTerritory.Remove(territory);

            // A nation can only be founded by creating a city. Which will be the capital.
            founded_nation.Cities.Add(new City("Capital City of " + founded_nation.Name, creator));

            founded_nation.CapitalCity.CityLocation = territory;
            founded_nation.CapitalCity.Owner = founded_nation;

            // Add territory to founded Nation.
            founded_nation.TerritoryAreas.Add(location);

            // Tell territory by whom it is ownd and if there is a city.
            territory.Owner = founded_nation;
            territory.SphereOfInfluenceCity = founded_nation.CapitalCity;
            territory.City = founded_nation.CapitalCity;


            // Add nation to the creator and Powers related to this nation.
            creator.FoundedNations.Add(founded_nation);
            creator.Powers.Add(new CreateCity(founded_nation));
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
