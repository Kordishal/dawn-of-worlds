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
using dawn_of_worlds.CelestialPowers.CreateAvatarPowers;
using dawn_of_worlds.CelestialPowers.EventPowers.RacialEvents;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.CelestialPowers.CreateRacePowers
{
    class CreateRace : Power
    {
        private Race _created_race { get; set; }
        private List<WeightedArea> _possible_areas { get; set; }
        private void determinePossibleAreas(World current_world)
        {
            _possible_areas = new List<WeightedArea>();

            foreach (Area area in current_world.AreaGrid)
            {
                // if this is a subrace
                // Exclude all areas where the main race is not present or neighbouring
                if (_created_race.isSubRace)
                    if (!area.Inhabitants.Contains(_created_race.MainRace) && !neighbourAreaHasMainRace(area.Neighbours))
                        continue;

                // Aquatic, exclude all areas, which do not have water to live in.
                if (_created_race.Habitat == RacialHabitat.Aquatic)
                    if (area.AreaRegion.Landmass && area.Rivers.Count == 0 && area.Lakes.Count == 0)
                        continue;

                // Subterranean, exlude all areas, which do not have an underworld or caves.
                if (_created_race.Habitat == RacialHabitat.Subterranean)
                    if (area.Caves.Count == 0)
                        continue;

                // Terranean, exclude all areas which do not include a landmass to live on
                if (_created_race.Habitat == RacialHabitat.Terranean)
                    if (!area.AreaRegion.Landmass)
                        continue;

                // Add the remaining areas and weight them with preferred terrain & climate traits
                WeightedArea weighted_area = new WeightedArea(area);
                weighted_area.Weight += 5;

                foreach (RacialPreferredHabitatTerrain terrain in _created_race.PreferredTerrain)
                {
                    switch (terrain)
                    {
                        case RacialPreferredHabitatTerrain.CaveDwellers:
                            if (area.Caves.Count > 0)
                                weighted_area.Weight += area.Caves.Count * 10;
                            break;
                        case RacialPreferredHabitatTerrain.DesertDwellers:
                            if (area.Deserts.Count > 0)
                                weighted_area.Weight += area.Deserts.Count * 10;
                            break;
                        case RacialPreferredHabitatTerrain.ForestDwellers:
                            if (area.Forests.Count > 0)
                                weighted_area.Weight += area.Forests.Count * 10;
                            break;
                        case RacialPreferredHabitatTerrain.HillDwellers:
                            if (area.HillRanges != null)
                                weighted_area.Weight += area.HillRanges.Hills.Count * 10;
                            break;
                        case RacialPreferredHabitatTerrain.MountainDwellers:
                            if (area.MountainRanges != null)
                                weighted_area.Weight += area.MountainRanges.Mountains.Count * 10;
                            break;
                        case RacialPreferredHabitatTerrain.PlainDwellers:
                            if (area.Grasslands.Count > 0)
                                weighted_area.Weight += area.Grasslands.Count * 10;
                            break;
                    }
                }

                foreach (RacialPreferredHabitatClimate climate in _created_race.PreferredClimate)
                {
                    switch (climate)
                    {
                        case RacialPreferredHabitatClimate.ColdAcclimated:
                            if (area.AreaClimate == Climate.Arctic || area.AreaClimate == Climate.SubArctic)
                                weighted_area.Weight += 200;
                            break;
                        case RacialPreferredHabitatClimate.HeatAcclimated:
                            if (area.AreaClimate == Climate.Tropical || area.AreaClimate == Climate.SubTropical)
                                weighted_area.Weight += 200;
                            break;
                        case RacialPreferredHabitatClimate.TemperateAcclimated:
                            if (area.AreaClimate == Climate.Temperate)
                                weighted_area.Weight += 200;
                            break;
                    }
                }

                _possible_areas.Add(weighted_area);
            }
        }
        private bool neighbourAreaHasMainRace(Area[] neighbours)
        {
            foreach (Area area in neighbours)
            {
                if (area != null)
                {
                    if (area.Inhabitants.Contains(_created_race.MainRace))
                        return true;
                }
            }
            return false;
        }
        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            // No longer valid once unsed.
            if (isObsolete)
                return false;

            if (_created_race.isSubRace && _created_race.MainRace.Creator == null)
                return false;

            determinePossibleAreas(current_world);

            if (_possible_areas.Count == 0)
                return false;

            return true;
        }

        public override int Cost(int current_age)
        {
            int cost = 0;

            switch (current_age)
            {
                case 1:
                    cost += 22;
                    break;
                case 2:
                    cost += 6;
                    break;
                case 3:
                    cost += 15;
                    break;
                default:
                    cost += 0;
                    break;
            }

            return cost;
        }

        // Once the race is created and has a creator it becomes obsolete and can no longer be used.
        public override bool isObsolete
        {
            get
            {
                return _created_race.Creator != null;
            }
        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            determinePossibleAreas(current_world);

            int total_weight = 0;
            foreach (WeightedArea area in _possible_areas)
            {
                total_weight += area.Weight;
            }

            Area location = null;
            int chance = Constants.RND.Next(total_weight);
            int prev_weight = 0, current_weight = 0;
            foreach (WeightedArea area in _possible_areas)
            {
                current_weight += area.Weight;
                if (prev_weight <= chance && chance < current_weight)
                {
                    location = area.Area;
                }
                prev_weight = current_weight;
            }
                    
            // Each race has an order dedicated to worship their creator.
            Order creator_worhip_order = new Order(_created_race.Name + " Creation Church", creator, OrderType.Church, OrderPurpose.FounderWorship);
            creator_worhip_order.OrderRace = _created_race;
            creator_worhip_order.OrderNation = null;

            _created_race.Creator = creator;
            _created_race.OriginOrder = creator_worhip_order;

            // The created race is settled 
            _created_race.HomeArea = location;
            _created_race.SettledAreas.Add(location);
                    
            // Tells the Area that someone is living here.
            location.Inhabitants.Add(_created_race);

            // Tells the creator what they have created and adds the powers to command this race.
            creator.CreatedRaces.Add(_created_race);
            creator.CreatedOrders.Add(creator_worhip_order);

            creator.Powers.Add(new SettleArea(_created_race));
            creator.Powers.Add(new FoundNation(_created_race));

            foreach (Deity deity in current_world.Deities)
            {
                // Add racial events.
                deity.Powers.Add(new RacialEpidemic(_created_race));
                deity.Powers.Add(new EndRacialEpidemic(_created_race));
                // Add avatars for this race.
                foreach (AvatarType type in Enum.GetValues(typeof(AvatarType)))
                {
                    deity.Powers.Add(new CreateAvatar(type, _created_race, null, null));
                }
            }

            // If this is a subrace some extra steps are required
            if (_created_race.isSubRace)
            {
                _created_race.MainRace.SubRaces.Add(_created_race);
            }

            current_world.Races.Add(_created_race);
            creator.LastCreation = _created_race;
        }

        public override int Weight(World current_world, Deity creator, int current_age)
        {
            int weight = 0;

            switch (current_age)
            {
                case 1:
                    weight += Constants.WEIGHT_STANDARD_LOW;
                    break;
                case 2:
                    weight += Constants.WEIGHT_STANDARD_HIGH;
                    break;
                case 3:
                    weight += Constants.WEIGHT_STANDARD_MEDIUM;
                    break;
                default:
                    weight += 0;
                    break;
            }

            int cost = Cost(current_age);
            if (cost > Constants.WEIGHT_COST_DEVIATION_MEDIUM)
                weight += cost * Constants.WEIGHT_STANDARD_COST_DEVIATION;
            else
                weight -= cost * Constants.WEIGHT_STANDARD_COST_DEVIATION;


            if (creator.Domains.Contains(Domain.Creation))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        public CreateRace(Race created_race)
        {
            if (created_race.isSubRace)
                Name = "Create SubRace: " + created_race.Name;
            else
                Name = "Create Race: " + created_race.Name;
            _created_race = created_race;
        }
    }
}
