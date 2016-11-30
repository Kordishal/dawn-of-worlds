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
using dawn_of_worlds.Creations.Geography;

namespace dawn_of_worlds.CelestialPowers.CreateRacePowers
{
    class CreateRace : Power
    {
        private Race _created_race { get; set; }
        private Terrain _terrain { get; set; }
        private bool neighbourTerrainHasMainRace()
        {
            for (int i = 0; i < 8; i++)
            {
                SystemCoordinates coords = _terrain.Coordinates.GetNeighbour(i);

                if (coords.X >= 0 && coords.Y >= 0 && coords.X < Constants.TERRAIN_GRID_X && coords.Y < Constants.TERRAIN_GRID_Y)
                {
                    if (Program.World.TerrainGrid[coords.X, coords.Y].SettledRaces.Contains(_created_race.MainRace))
                        return true;
                }
            }
            return false;
        }

        public override bool Precondition(Deity creator)
        {
            // No longer valid once used.
            if (isObsolete)
                return false;

            if (_created_race.isSubRace && _created_race.MainRace.Creator == null)
                return false;

            // if this is a subrace
            // Exclude all areas where the main race is not present or neighbouring
            if (_created_race.isSubRace)
                if (!_terrain.SettledRaces.Contains(_created_race.MainRace) || !neighbourTerrainHasMainRace())
                    return false;

            // Aquatic, exclude all areas, which do not have water to live in.
            if (_created_race.Habitat == RacialHabitat.Aquatic)
                if (!(_terrain.Type == TerrainType.Ocean) && !(_terrain.SecondaryTerrainFeatures.Exists(x => x.GetType() == typeof(Lake))))
                    return false;

            // Subterranean, exlude all areas, which do not have an underworld or caves.
            if (_created_race.Habitat == RacialHabitat.Subterranean)
                if (!_terrain.SecondaryTerrainFeatures.Exists(x => x.GetType() == typeof(Cave)))
                    return false;

            // Terranean, exclude all areas which do not include a landmass to live on
            if (_created_race.Habitat == RacialHabitat.Terranean)
                if (_terrain.Type == TerrainType.Ocean)
                    return false;

            return true;
        }

        public override int Cost()
        {
            int cost = 0;
            switch (Simulation.Time.CurrentAge)
            {
                case Age.Creation:
                    cost += 22;
                    break;
                case Age.Races:
                    cost += 6;
                    break;
                case Age.Relations:
                    cost += 15;
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

        public override void Effect(Deity creator)
        {                
            // Each race has an order dedicated to worship their creator.
            Order creator_worhip_order = new Order("PlaceHolder", creator, OrderType.Church, OrderPurpose.FounderWorship);
            creator_worhip_order.OrderRace = _created_race;
            creator_worhip_order.OrderNation = null;
            creator_worhip_order.Name = Constants.Names.GetReligionName(creator, creator_worhip_order.OrderRace);

            _created_race.Creator = creator;
            _created_race.OriginOrder = creator_worhip_order;

            // The created race is settled 
            _created_race.HomeTerrain = _terrain;
            _created_race.SettledTerrains.Add(_terrain);
                    
            // Tells the Area that someone is living here.
            _terrain.SettledRaces.Add(_created_race);

            // Tells the creator what they have created and adds the powers to command this race.
            creator.CreatedRaces.Add(_created_race);
            creator.CreatedOrders.Add(creator_worhip_order);

            foreach (Terrain terrain in Program.World.TerrainGrid)
                creator.Powers.Add(new SettleTerrain(_created_race, terrain));
            foreach (NationTypes type in Enum.GetValues(typeof(NationTypes)))
                creator.Powers.Add(new FoundNation(_created_race, type));

            foreach (Deity deity in Program.World.Deities)
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

            // Add the race to the world overview.
            Program.World.Races.Add(_created_race);
            creator.LastCreation = _created_race;
        }

        public override int Weight(Deity creator)
        {
            int weight = 0;

            switch (Simulation.Time.CurrentAge)
            {
                case Age.Creation:
                    weight += Constants.WEIGHT_STANDARD_LOW;
                    break;
                case Age.Races:
                    weight += Constants.WEIGHT_STANDARD_HIGH;
                    break;
                case Age.Relations:
                    weight += Constants.WEIGHT_STANDARD_MEDIUM;
                    break;
                default:
                    weight += 0;
                    break;
            }

            int cost = Cost();
            if (cost > Constants.WEIGHT_COST_DEVIATION_MEDIUM)
                weight += cost * Constants.WEIGHT_STANDARD_COST_DEVIATION;
            else
                weight -= cost * Constants.WEIGHT_STANDARD_COST_DEVIATION;


            if (creator.Domains.Contains(Domain.Creation))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            foreach (RacialPreferredHabitatTerrain terrain in _created_race.PreferredTerrain)
            {
                switch (terrain)
                {
                    case RacialPreferredHabitatTerrain.CaveDwellers:
                        if (_terrain.SecondaryTerrainFeatures.FindAll(x => x.GetType() == typeof(Cave)).Count > 0)
                            weight += _terrain.SecondaryTerrainFeatures.FindAll(x => x.GetType() == typeof(Cave)).Count * 10;
                        break;
                    case RacialPreferredHabitatTerrain.DesertDwellers:
                        if (_terrain.PrimaryTerrainFeature.GetType() == typeof(Desert))
                            weight += Constants.WEIGHT_STANDARD_CHANGE;
                        break;
                    case RacialPreferredHabitatTerrain.ForestDwellers:
                        if (_terrain.PrimaryTerrainFeature.GetType() == typeof(Forest))
                            weight += Constants.WEIGHT_STANDARD_CHANGE;
                        break;
                    case RacialPreferredHabitatTerrain.HillDwellers:
                        if (_terrain.Type == TerrainType.HillRange)
                            weight += Constants.WEIGHT_STANDARD_CHANGE;
                        break;
                    case RacialPreferredHabitatTerrain.MountainDwellers:
                        if (_terrain.Type == TerrainType.MountainRange)
                            weight += Constants.WEIGHT_STANDARD_CHANGE;
                        break;
                    case RacialPreferredHabitatTerrain.PlainDwellers:
                        if (_terrain.Type == TerrainType.Plain)
                            weight += Constants.WEIGHT_STANDARD_CHANGE;
                        break;
                }
            }

            foreach (RacialPreferredHabitatClimate climate in _created_race.PreferredClimate)
            {
                switch (climate)
                {
                    case RacialPreferredHabitatClimate.ColdAcclimated:
                        if (_terrain.Area.ClimateArea == Climate.Arctic || _terrain.Area.ClimateArea == Climate.SubArctic)
                            weight += Constants.WEIGHT_STANDARD_CHANGE * 2;
                        break;
                    case RacialPreferredHabitatClimate.HeatAcclimated:
                        if (_terrain.Area.ClimateArea == Climate.Tropical || _terrain.Area.ClimateArea == Climate.SubTropical)
                            weight += Constants.WEIGHT_STANDARD_CHANGE * 2;
                        break;
                    case RacialPreferredHabitatClimate.TemperateAcclimated:
                        if (_terrain.Area.ClimateArea == Climate.Temperate)
                            weight += Constants.WEIGHT_STANDARD_CHANGE * 2;
                        break;
                }
            }

            return weight >= 0 ? weight : 0;
        }

        public CreateRace(Race created_race, Terrain terrain)
        {
            if (created_race.isSubRace)
                Name = "Create SubRace: " + created_race.Name;
            else
                Name = "Create Race: " + created_race.Name;
            _created_race = created_race;
            _terrain = terrain;

        }
    }
}
