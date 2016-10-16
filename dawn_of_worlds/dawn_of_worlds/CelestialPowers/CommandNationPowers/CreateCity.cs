using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Creations.Geography;

namespace dawn_of_worlds.CelestialPowers.CommandNationPowers
{
    class CreateCity : CommandNation
    {
        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            foreach (GeographicalFeature gc in _commanded_nation.Territory)
            {
                if (gc.City == null)
                {
                    return true;
                }
            }

            return false;
        }


        public override void Effect(World current_world, Deity creator, int current_age)
        {
            GeographicalFeature location = null;

            // Search for a valid city location. Each geographical feature can have one city.
            while (location == null)
            {
                location = _commanded_nation.Territory[Main.MainLoop.RND.Next(_commanded_nation.Territory.Count)];

                if (location.City != null)
                    location = null;
            }

            // The city is created and placed in the world. The nation is defined as the city owner.
            City founded_city = new City("THE ONE CITY", creator);
            founded_city.CityLocation = location;
            founded_city.Owner = _commanded_nation;

            // The city is stored where it is placed, by whom it is owned and created.
            location.City = founded_city;
            _commanded_nation.Cities.Add(founded_city);
            creator.FoundedCities.Add(founded_city);
        }


        public CreateCity(Nation commanded_nation) : base(commanded_nation)
        {
            Name = "Create City: " + commanded_nation.Name;
        }

    }
}
