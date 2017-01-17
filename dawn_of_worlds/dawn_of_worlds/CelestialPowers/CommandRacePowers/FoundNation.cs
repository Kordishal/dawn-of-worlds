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
using dawn_of_worlds.CelestialPowers.CreateOrderPowers;
using dawn_of_worlds.CelestialPowers.CreateAvatarPowers;
using dawn_of_worlds.CelestialPowers.EventPowers.NationalEvents;
using dawn_of_worlds.Main;
using dawn_of_worlds.Creations.Diplomacy;
using dawn_of_worlds.Effects;
using dawn_of_worlds.Creations.Civilisations;

namespace dawn_of_worlds.CelestialPowers.CommandRacePowers
{
    class FoundNation : CommandRace
    {
        private Polity _polity { get; set; }

        protected override void initialize()
        {
            base.initialize();
            Name = "Found Nation (" + _polity.ToString() + ")";
            Tags = _polity.Tags;
        }

        public override bool Precondition(Deity creator)
        {
            base.Precondition(creator);

            return true;
        }

        public override void Effect(Deity creator)
        {
            List<WeightedObjects<Province>> possible_locations = new List<WeightedObjects<Province>>();

            foreach (Province province in _commanded_race.SettledProvinces)
            {
                if (!province.hasOwner)
                    possible_locations.Add(new WeightedObjects<Province>(province));
            }
                

            foreach (WeightedObjects<Province> weighted_feature in possible_locations)
            {
                weighted_feature.Weight += 5;
                // considers the biome of the terrain feature.
                switch (weighted_feature.Object.PrimaryTerrainFeature.BiomeType)
                {
                    case BiomeType.BorealForest:
                    case BiomeType.TemperateDeciduousForest:
                    case BiomeType.TropicalDryForest:
                    case BiomeType.TropicalRainforest:
                        if (_commanded_race.PreferredTerrain.Exists(x => x == RacialPreferredHabitatTerrain.ForestDwellers))
                            weighted_feature.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                        break;
                    case BiomeType.ColdDesert:
                    case BiomeType.HotDesert:
                        if (_commanded_race.PreferredTerrain.Exists(x => x == RacialPreferredHabitatTerrain.DesertDwellers))
                            weighted_feature.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                        break;
                    case BiomeType.TemperateGrassland:
                    case BiomeType.Tundra:
                    case BiomeType.TropicalGrassland:
                        if (_commanded_race.PreferredTerrain.Exists(x => x == RacialPreferredHabitatTerrain.GrasslandDwellers))
                            weighted_feature.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                        break;
                    case BiomeType.Subterranean:
                        if (_commanded_race.PreferredTerrain.Exists(x => x == RacialPreferredHabitatTerrain.CaveDwellers))
                            weighted_feature.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                        break;
                }
                // Considers the generall type of the terrain.
                switch (weighted_feature.Object.Type)
                {
                    case TerrainType.HillRange:
                        if (_commanded_race.PreferredTerrain.Exists(x => x == RacialPreferredHabitatTerrain.HillDwellers))
                            weighted_feature.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                        break;
                    case TerrainType.MountainRange:
                        if (_commanded_race.PreferredTerrain.Exists(x => x == RacialPreferredHabitatTerrain.MountainDwellers))
                            weighted_feature.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                        break;
                    case TerrainType.Plain:
                        if (_commanded_race.PreferredTerrain.Exists(x => x == RacialPreferredHabitatTerrain.PlainDwellers))
                            weighted_feature.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                        break;
                }
            }

            Province location = WeightedObjects<Province>.ChooseRandomObject(possible_locations);
            
            Civilisation founded_civilisation = new Civilisation("Nation of " + _commanded_race.Name, creator);
            founded_civilisation.PoliticalOrganisation = _polity;
            founded_civilisation.InhabitantRaces.Add(_commanded_race);

            // Nomadic?
            switch (founded_civilisation.PoliticalOrganisation.Organisation)
            {
                case SocialOrganisation.BandSociety:
                    switch (founded_civilisation.PoliticalOrganisation.Form)
                    {
                        case PolityForm.Band:
                        case PolityForm.Herd:
                        case PolityForm.Pack:
                            founded_civilisation.isNomadic = true;
                            break;
                        case PolityForm.Brood:
                            founded_civilisation.isNomadic = false;
                            break;
                    }
                    break;
                case SocialOrganisation.TribalSociety:
                    int rnd = Constants.Random.Next(50);
                    if (rnd < 25)
                        founded_civilisation.isNomadic = true;
                    else
                        founded_civilisation.isNomadic = false;
                    break;
            }

            // Diplomacy
            if (_commanded_race.Type == SpeciesType.Beasts)
            {
                founded_civilisation.hasDiplomacy = false;
            }
            else
            {
                founded_civilisation.hasDiplomacy = true;
                foreach (Civilisation nation in Program.World.Nations)
                {
                    nation.Relationships.Add(new Relations(founded_civilisation));
                    founded_civilisation.Relationships.Add(new Relations(nation));
                }
            }

            // Cities
            // Nomadic civilisations do not have cities
            if (founded_civilisation.isNomadic)
                founded_civilisation.hasCities = false;
            else
            {
                founded_civilisation.hasCities = true;
                founded_civilisation.Cities.Add(new City("Capital City of " + founded_civilisation.Name, creator));
                founded_civilisation.CapitalCity.TerrainFeature = location.PrimaryTerrainFeature;
                founded_civilisation.CapitalCity.Owner = founded_civilisation;

                location.PrimaryTerrainFeature.City = founded_civilisation.CapitalCity;
            }

            // Territory
            founded_civilisation.Territory.Add(location);
            if (founded_civilisation.isNomadic)
                location.NomadicPresence.Add(founded_civilisation);
            else
                location.Owner = founded_civilisation;


            // Add origin order -> church. This church is needed to be able to command this nation.
            Order founder_origin_order = new Order(Constants.Names.GetReligionName(creator, _commanded_race), creator, OrderType.Church, OrderPurpose.FounderWorship);
            founder_origin_order.OrderNation = founded_civilisation;
            founder_origin_order.OrderRace = null;

            founded_civilisation.NationalOrders.Add(founder_origin_order);
            creator.CreatedOrders.Add(founder_origin_order);

            // Possible War Goals
            WarGoal war_goal;
            List<WarGoal> war_goals = new List<WarGoal>();
            switch (founded_civilisation.PoliticalOrganisation.Organisation)
            {
                case SocialOrganisation.BandSociety:
                    break;
                case SocialOrganisation.TribalSociety:
                    break;
                case SocialOrganisation.Chiefdom:
                    break;
                case SocialOrganisation.State:
                    break;
            }

            founded_civilisation.PossibleWarGoals.AddRange(war_goals);

            // Add nation to the creator and Powers related to this nation.
            creator.FoundedNations.Add(founded_civilisation);
            Program.World.Nations.Add(founded_civilisation);
            creator.CreatedOrders.Add(founded_civilisation.OriginOrder);

            if (founded_civilisation.hasCities)
            {
                creator.Powers.Add(new CreateCity(founded_civilisation));
                creator.FoundedCities.Add(founded_civilisation.CapitalCity);
                Program.World.Cities.Add(founded_civilisation.CapitalCity);
            }
            
            if (founded_civilisation.hasDiplomacy)
            {
                creator.Powers.Add(new ExpandTerritory(founded_civilisation));
                creator.Powers.Add(new EstablishContact(founded_civilisation));
                creator.Powers.Add(new FormAlliance(founded_civilisation));
                creator.Powers.Add(new DeclareWar(founded_civilisation));
            }


            foreach (Deity deity in Program.World.Deities)
            {
                // Add avatars
                foreach (AvatarType type in Enum.GetValues(typeof(AvatarType)))
                {
                    deity.Powers.Add(new CreateAvatar(type, founded_civilisation.FoundingRace, founded_civilisation, null));
                }

                // Add Events
                deity.Powers.Add(new VastGoldMineEstablised(founded_civilisation));
                deity.Powers.Add(new VastGoldMineDepleted(founded_civilisation));
            }

            foreach (Deity deity in Program.World.Deities)
            {
                if (!(deity == creator))
                    deity.Powers.Add(new CreateOrder(OrderType.Church, OrderPurpose.FounderWorship, founded_civilisation, null));
            }

            creator.LastCreation = founded_civilisation;
        }

        public FoundNation (Race command_race, Polity polity) : base(command_race)
        {
            _polity = polity;
            initialize();
        }
    }
}
