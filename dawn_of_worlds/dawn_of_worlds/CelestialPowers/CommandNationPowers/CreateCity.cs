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
using dawn_of_worlds.Creations.Diplomacy;
using dawn_of_worlds.Creations.Objects;

namespace dawn_of_worlds.CelestialPowers.CommandNationPowers
{
    class CreateCity : CommandNation
    {

        private List<TerrainFeatures> _valid_city_terrains { get; set; }

        public override int Cost()
        {
            int cost = base.Cost();

            if (_commanded_nation.Tags.Contains(NationalTags.VeryRich))
                cost -= 2;

            return cost;
        }

        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

            if (creator.Domains.Contains(Domain.Creation))
                weight += Constants.WEIGHT_MANY_CHANGE;

            if (creator.Domains.Contains(Domain.Architecture))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            if (_commanded_nation.Tags.Contains(NationalTags.VeryRich))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        public override bool Precondition(Deity creator)
        {
            if (isObsolete)
                return false;

            if (!_commanded_nation.hasCities)
                return false;

            _valid_city_terrains = validTerrainFeatures();

            if (_valid_city_terrains.Count > 0)
                return true;

            return false;
        }

        private List<TerrainFeatures> validTerrainFeatures()
        {
            List<TerrainFeatures> terrain_features = new List<TerrainFeatures>();

            foreach (Province province in _commanded_nation.Territory)
            {
                if (province.PrimaryTerrainFeature.City == null)
                    terrain_features.Add(province.PrimaryTerrainFeature);

                foreach (TerrainFeatures terrain in province.SecondaryTerrainFeatures)
                {
                    if (terrain.City == null)
                        terrain_features.Add(terrain);
                }
            }

            return terrain_features;
        }


        public override void Effect(Deity creator)
        {
            // Choose the city location at random.
            TerrainFeatures construction_site = _valid_city_terrains[Constants.Random.Next(_valid_city_terrains.Count)];
            
            // The city is created and placed in the world. The nation is defined as the city owner.
            City founded_city = new City("PlaceHolder", creator);
            founded_city.TerrainFeature = construction_site;
            founded_city.Owner = _commanded_nation;

            // Tell the location, that it now has a city.
            construction_site.City = founded_city;

            // add the city to the list of cities owned by the nation.
            _commanded_nation.Cities.Add(founded_city);            

            //New War Goal to conquer this city.
            _commanded_nation.PossibleWarGoals.Add(new WarGoal(WarGoalType.CityConquest));
            _commanded_nation.PossibleWarGoals.Last().City = founded_city;

            founded_city.Name.Singular = Constants.Names.GetName("cities");

            // Add city related powers and the creator
            creator.FoundedCities.Add(founded_city);
            creator.Powers.Add(new RaiseArmy(founded_city));
            creator.Powers.Add(new ConstructBuilding(founded_city, BuildingType.CityWall));
            creator.Powers.Add(new ConstructBuilding(founded_city, BuildingType.Temple));
            creator.Powers.Add(new ConstructBuilding(founded_city, BuildingType.Shrine));

            creator.LastCreation = founded_city;

            //Program.WorldHistory.AddRecord(founded_city);
        }


        public CreateCity(Nation commanded_nation) : base(commanded_nation)
        {
            Name = "Create City: " + commanded_nation.Name + " in Area " + commanded_nation.Territory[0].Name;
        }

    }
}
