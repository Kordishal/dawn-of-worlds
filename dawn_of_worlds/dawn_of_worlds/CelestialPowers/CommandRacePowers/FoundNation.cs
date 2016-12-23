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

namespace dawn_of_worlds.CelestialPowers.CommandRacePowers
{
    class FoundNation : CommandRace
    {
        private NationTypes _type { get; set; }

        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

            if (creator.Domains.Contains(Domain.Creation))
                weight += Constants.WEIGHT_MANY_CHANGE;

            if (creator.Domains.Contains(Domain.Community))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            if (_commanded_race.Tags.Contains(RaceTags.RacialEpidemic))
                weight -= Constants.WEIGHT_STANDARD_CHANGE;


            // Some social & cultural practices have an effect on the type of nation created.
            foreach (SocialCulturalCharacteristic social_cultural_characteristic in _commanded_race.SocialCulturalCharacteristics)
            {
                switch (social_cultural_characteristic)
                {
                    case SocialCulturalCharacteristic.Communal:
                        break;
                    case SocialCulturalCharacteristic.Nomadic:
                        if (_type == NationTypes.NomadicTribe)
                            weight += Constants.WEIGHT_STANDARD_CHANGE;
                        break;
                    case SocialCulturalCharacteristic.Sedentary:
                        if (_type == NationTypes.NomadicTribe)
                            weight -= Constants.WEIGHT_STANDARD_CHANGE;
                        break;
                    case SocialCulturalCharacteristic.Tribal:
                        if (_type == NationTypes.TribalNation)
                            weight += Constants.WEIGHT_STANDARD_CHANGE;
                        if (_type == NationTypes.NomadicTribe)
                            weight += Constants.WEIGHT_STANDARD_CHANGE;
                        break;
                    case SocialCulturalCharacteristic.Territorial:
                        if (_type == NationTypes.LairTerritory)
                            weight += Constants.WEIGHT_STANDARD_CHANGE;
                        if (_type == NationTypes.HuntingGrounds)
                            weight += Constants.WEIGHT_STANDARD_CHANGE;
                        break;
                    case SocialCulturalCharacteristic.Elitist:
                        if (_type == NationTypes.FeudalNation)
                            weight += Constants.WEIGHT_STANDARD_CHANGE;
                        break;
                }              
            }
            return weight >= 0 ? weight : 0;
        }

        public override bool Precondition(Deity creator)
        {
            if (isObsolete)
                return false;


            switch (_commanded_race.Type)
            {
                case SpeciesType.Humanoid:
                    if (_type == NationTypes.HuntingGrounds)
                        return false;
                    if (_type == NationTypes.LairTerritory)
                        return false;
                    break;
                case SpeciesType.Dragonoid:
                    if (_type == NationTypes.HuntingGrounds)
                        return false;
                    if (_type == NationTypes.NomadicTribe)
                        return false;
                    if (_type == NationTypes.TribalNation)
                        return false;
                    if (_type == NationTypes.FeudalNation)
                        return false;
                    break;
                case SpeciesType.Beasts:
                    if (_type == NationTypes.LairTerritory)
                        return false;
                    if (_type == NationTypes.NomadicTribe)
                        return false;
                    if (_type == NationTypes.TribalNation)
                        return false;
                    if (_type == NationTypes.FeudalNation)
                        return false;
                    break;
            }

            switch (_type)
            {
                case NationTypes.FeudalNation:
                case NationTypes.TribalNation:
                case NationTypes.LairTerritory:
                    foreach (Province province in _commanded_race.SettledProvinces)
                    {
                        if (province.Owner == null)
                        {
                            return true;
                        }
                    }
                    break;
                case NationTypes.HuntingGrounds:
                case NationTypes.NomadicTribe:
                    return true;
            }

            return false;
        }

        public override void Effect(Deity creator)
        {
            List<WeightedObjects<Province>> possible_locations = new List<WeightedObjects<Province>>();

            foreach (Province province in _commanded_race.SettledProvinces)
            {
                switch (_type)
                {
                    case NationTypes.FeudalNation:
                    case NationTypes.TribalNation:
                    case NationTypes.LairTerritory:
                        if (province.Owner == null)
                            possible_locations.Add(new WeightedObjects<Province>(province));
                        break;
                    case NationTypes.HuntingGrounds:
                    case NationTypes.NomadicTribe:
                        possible_locations.Add(new WeightedObjects<Province>(province));
                        break;
                }
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
            
            Nation founded_nation = new Nation("Nation of " + _commanded_race.Name, creator);
            founded_nation.Type = _type;
            founded_nation.InhabitantRaces.Add(_commanded_race);
            founded_nation.Territory.Add(location);

            // Diplomacy
            if (_commanded_race.Type == SpeciesType.Beasts)
            {
                founded_nation.hasDiplomacy = false;
            }
            else
            {
                founded_nation.hasDiplomacy = true;
                foreach (Nation nation in Program.World.Nations)
                {
                    nation.Relationships.Add(new Relations(founded_nation));
                    founded_nation.Relationships.Add(new Relations(nation));
                }
            }

            // Cities
            if (founded_nation.Type == NationTypes.HuntingGrounds || founded_nation.Type == NationTypes.NomadicTribe)
            {
                founded_nation.hasCities = false;
            }
            else
            {
                // Nations with cities get their capital city.
                founded_nation.hasCities = true;
                founded_nation.Cities.Add(new City("Capital City of " + founded_nation.Name, creator));
                founded_nation.CapitalCity.TerrainFeature = location.PrimaryTerrainFeature;
                founded_nation.CapitalCity.Owner = founded_nation;

                location.PrimaryTerrainFeature.City = founded_nation.CapitalCity;
            }

            // Territory
            founded_nation.Territory.Add(location);
            switch (founded_nation.Type)
            {
                case NationTypes.FeudalNation:
                case NationTypes.TribalNation:
                case NationTypes.LairTerritory:
                    location.Owner = founded_nation;
                    break;
                case NationTypes.HuntingGrounds:
                    location.HuntingGrounds.Add(founded_nation);
                    break;
                case NationTypes.NomadicTribe:
                    location.NomadicPresence.Add(founded_nation);
                    break;
            }


            // Add origin order -> church. This church is needed to be able to command this nation.
            Order founder_origin_order = new Order(Constants.Names.GetReligionName(creator, _commanded_race), creator, OrderType.Church, OrderPurpose.FounderWorship);
            founder_origin_order.OrderNation = founded_nation;
            founder_origin_order.OrderRace = null;

            founded_nation.NationalOrders.Add(founder_origin_order);
            creator.CreatedOrders.Add(founder_origin_order);

            // Possible War Goals
            WarGoal war_goal;
            List<WarGoal> war_goals = new List<WarGoal>();
            switch (founded_nation.Type)
            {
                case NationTypes.FeudalNation:
                case NationTypes.TribalNation:
                case NationTypes.LairTerritory:
                    war_goal = new WarGoal(WarGoalType.Conquest);
                    war_goal.Territory = founded_nation.Territory[0];
                    war_goals.Add(war_goal);
                    break;
                case NationTypes.HuntingGrounds:
                    break;
                case NationTypes.NomadicTribe:
                    war_goal = new WarGoal(WarGoalType.RemoveNomadicPresence);
                    war_goals.Add(war_goal);
                    break;
            }

            founded_nation.PossibleWarGoals.AddRange(war_goals);

            // Add nation to the creator and Powers related to this nation.
            creator.FoundedNations.Add(founded_nation);
            Program.World.Nations.Add(founded_nation);
            creator.CreatedOrders.Add(founded_nation.OriginOrder);

            if (founded_nation.hasCities)
            {
                creator.Powers.Add(new CreateCity(founded_nation));
                creator.FoundedCities.Add(founded_nation.CapitalCity);
                Program.World.Cities.Add(founded_nation.CapitalCity);
            }
            
            if (founded_nation.hasDiplomacy)
            {
                creator.Powers.Add(new ExpandTerritory(founded_nation));
                creator.Powers.Add(new EstablishContact(founded_nation));
                creator.Powers.Add(new FormAlliance(founded_nation));
                creator.Powers.Add(new DeclareWar(founded_nation));
            }


            foreach (Deity deity in Program.World.Deities)
            {
                // Add avatars
                foreach (AvatarType type in Enum.GetValues(typeof(AvatarType)))
                {
                    deity.Powers.Add(new CreateAvatar(type, founded_nation.FoundingRace, founded_nation, null));
                }

                // Add Events
                deity.Powers.Add(new VastGoldMineEstablised(founded_nation));
                deity.Powers.Add(new VastGoldMineDepleted(founded_nation));
            }

            foreach (Deity deity in Program.World.Deities)
            {
                if (!(deity == creator))
                    deity.Powers.Add(new CreateOrder(OrderType.Church, OrderPurpose.FounderWorship, founded_nation, null));
            }

            creator.LastCreation = founded_nation;
        }

        public FoundNation (Race command_race, NationTypes type) : base(command_race)
        {
            Name = "Found Nation: " + command_race.Name + " " + type.ToString();
            _type = type;
        }
    }
}
