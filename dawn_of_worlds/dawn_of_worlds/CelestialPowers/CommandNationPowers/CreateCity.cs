using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.CelestialPowers.CommandCityPowers;

namespace dawn_of_worlds.CelestialPowers.CommandNationPowers
{
    class CreateCity : CommandNation
    {
        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            if (isObsolete)
                return false;

            foreach (Area a in _commanded_nation.TerritoryAreas)
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
            GeographicalFeature location = null;
            List<GeographicalFeature> unclaimed_territory = new List<GeographicalFeature>();

            foreach (Area area in _commanded_nation.TerritoryAreas)
            {
                unclaimed_territory.AddRange(area.UnclaimedTerritory);
            }

            // Search for a valid city location. Each geographical feature can have one city.
            while (location == null)
            {
                location = unclaimed_territory[Main.MainLoop.RND.Next(unclaimed_territory.Count)];

                if (location.City != null)
                    location = null;
            }

            // The city is created and placed in the world. The nation is defined as the city owner.
            City founded_city = new City("THE ONE CITY", creator);
            founded_city.CityLocation = location;
            founded_city.CitySphereOfÌnfluence.Add(location);
            founded_city.Owner = _commanded_nation;

            // Tell the location, that it now has a city.
            location.City = founded_city;
            location.Location.UnclaimedTerritory.Remove(location);

            // add the city to the list of cities owned by the nation.
            _commanded_nation.Cities.Add(founded_city);

            // Add city related powers and the creator
            creator.FoundedCities.Add(founded_city);
            creator.Powers.Add(new ExpandCityInfluence(founded_city));
            creator.Powers.Add(new RaiseArmy(founded_city));
        }


        public CreateCity(Nation commanded_nation) : base(commanded_nation)
        {
            Name = "Create City: " + commanded_nation.Name;
        }

    }
}
