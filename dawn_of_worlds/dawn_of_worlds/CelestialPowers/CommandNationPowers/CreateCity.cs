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
using dawn_of_worlds.Main;

namespace dawn_of_worlds.CelestialPowers.CommandNationPowers
{
    class CreateCity : CommandNation
    {

        public override int Cost(int current_age)
        {
            int cost = base.Cost(current_age);

            if (_commanded_nation.Tags.Contains(NationalTags.VeryRich))
                cost -= 2;

            return cost;
        }

        public override int Weight(World current_world, Deity creator, int current_age)
        {
            int weight = base.Weight(current_world, creator, current_age);

            if (creator.Domains.Contains(Domain.Creation))
                weight += Constants.WEIGHT_MANY_CHANGE;

            if (creator.Domains.Contains(Domain.Architecture))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            if (_commanded_nation.Tags.Contains(NationalTags.VeryRich))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            if (isObsolete)
                return false;

            foreach (TerrainFeatures terrain_features in _commanded_nation.Territory)
            {
                if (terrain_features.City == null)
                    return true;
            }

            return false;
        }


        public override void Effect(World current_world, Deity creator, int current_age)
        {
            TerrainFeatures terrain_features = null;
            List<TerrainFeatures> undeveloped_terrain_features = _commanded_nation.Territory.FindAll(x => x.City == null);

            // Choose the city location at random.
            terrain_features = undeveloped_terrain_features[Constants.RND.Next(undeveloped_terrain_features.Count)];
            

            // The city is created and placed in the world. The nation is defined as the city owner.
            City founded_city = new City("City of the " + _commanded_nation.FoundingRace + " in Area " + terrain_features.Location.Name, creator);
            founded_city.CityLocation = terrain_features;
            founded_city.CitySphereOfÌnfluence.Add(terrain_features);
            founded_city.Owner = _commanded_nation;

            // Tell the location, that it now has a city.
            terrain_features.City = founded_city;
            terrain_features.Location.UnclaimedTerritory.Remove(terrain_features);

            // add the city to the list of cities owned by the nation.
            _commanded_nation.Cities.Add(founded_city);

            // Add city related powers and the creator
            creator.FoundedCities.Add(founded_city);
            creator.Powers.Add(new RaiseArmy(founded_city));

            creator.LastCreation = founded_city;
        }


        public CreateCity(Nation commanded_nation) : base(commanded_nation)
        {
            Name = "Create City: " + commanded_nation.Name + " in Area " + commanded_nation.Territory[0].Name;
        }

    }
}
