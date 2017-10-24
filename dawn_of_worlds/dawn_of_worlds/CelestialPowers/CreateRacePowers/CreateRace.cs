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
using dawn_of_worlds.Main;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Effects;
using dawn_of_worlds.Creations.Civilisations;

namespace dawn_of_worlds.CelestialPowers.CreateRacePowers
{
    class CreateRace : Power
    {
        private Race _created_race { get; set; }
        private Province _terrain { get; set; }

        protected override void initialize()
        {
            Name = "Create Race";
            BaseCost = new int[] { 22, 6, 15 };
            CostChange = Constants.COST_CHANGE_VALUE;

            BaseWeight = new int[] { Constants.WEIGHT_STANDARD_LOW, Constants.WEIGHT_STANDARD_HIGH, Constants.WEIGHT_STANDARD_MEDIUM };
            WeightChange = Constants.WEIGHT_STANDARD_CHANGE;

            Tags = new List<CreationTag>() { CreationTag.Creation };
        }

        public override bool Precondition(Deity creator)
        {
            base.Precondition(creator);

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

        // Once the race is created and has a creator it becomes obsolete and can no longer be used.
        public override bool isObsolete
        {
            get
            {
                return _created_race.Creator != null;
            }
        }

        public override int Effect(Deity creator)
        {
            _created_race.YearofCreation = Simulation.Time.Shuffle;
                
            // Each race has an order dedicated to worship their creator.
            Order creator_worhip_order = new Order("PlaceHolder", creator, OrderType.Church, OrderPurpose.FounderWorship);
            creator_worhip_order.OrderRace = _created_race;
            creator_worhip_order.OrderNation = null;
            creator_worhip_order.Name = Program.GenerateNames.GetName();

            _created_race.Creator = creator;
            _created_race.OriginOrder = creator_worhip_order;

            // The created race is settled 
            _created_race.SettledProvinces.Add(_terrain);
                    
            // Tells the Area that someone is living here.
            _terrain.SettledRaces.Add(_created_race);

            // Tells the creator what they have created and adds the powers to command this race.
            creator.CreatedRaces.Add(_created_race);
            creator.CreatedOrders.Add(creator_worhip_order);

            foreach (Area area in Program.World.AreaGrid)
                creator.Powers.Add(new SettleTile(_created_race, area));

            if (_created_race.Type == SpeciesType.Humanoid)
                creator.Powers.Add(new FoundNation(_created_race, PolityDefinitions.BandSociety));

            if (_created_race.Type == SpeciesType.Dragon)
                creator.Powers.Add(new FoundNation(_created_race, PolityDefinitions.DragonBrood));

            foreach (Deity deity in Program.World.Deities)
            {
                // Add avatars for this race.
                foreach (AvatarType type in Enum.GetValues(typeof(AvatarType)))
                {
                    deity.Powers.Add(new CreateAvatar(type, _created_race, null, null));
                }
            }

            // Add the race to the world overview.
            Program.World.Races.Add(_created_race);
            creator.LastCreation = _created_race;

            return 0;
        }

        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

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
                    case RacialPreferredHabitatClimate.Arctic:
                        if (_terrain.LocalClimate == Climate.Arctic || _terrain.LocalClimate == Climate.SubArctic)
                            weight += Constants.WEIGHT_STANDARD_CHANGE * 2;
                        break;
                    case RacialPreferredHabitatClimate.Tropical:
                        if (_terrain.LocalClimate == Climate.Tropical || _terrain.LocalClimate == Climate.SubTropical)
                            weight += Constants.WEIGHT_STANDARD_CHANGE * 2;
                        break;
                    case RacialPreferredHabitatClimate.Temperate:
                        if (_terrain.LocalClimate == Climate.Temperate)
                            weight += Constants.WEIGHT_STANDARD_CHANGE * 2;
                        break;
                }
            }

            return weight >= 0 ? weight : 0;
        }

        public CreateRace(Race created_race, Province terrain)
        {
            _created_race = created_race;
            _terrain = terrain;
            initialize();
        }
    }
}
