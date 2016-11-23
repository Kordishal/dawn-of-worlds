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

namespace dawn_of_worlds.CelestialPowers.CommandRacePowers
{
    class FoundNation : CommandRace
    {

        public override int Weight(World current_world, Deity creator, int current_age)
        {
            int weight = base.Weight(current_world, creator, current_age);

            if (creator.Domains.Contains(Domain.Creation))
                weight += Constants.WEIGHT_MANY_CHANGE;

            if (creator.Domains.Contains(Domain.Community))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            if (_commanded_race.Tags.Contains(RaceTags.RacialEpidemic))
                weight -= Constants.WEIGHT_STANDARD_CHANGE;

            return weight >= 0 ? weight : 0;
        }

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
                location = _commanded_race.SettledAreas[Main.Constants.RND.Next(_commanded_race.SettledAreas.Count)];
                // At least one unclaimed territory necessary to found a nation.
                if (location.UnclaimedTerritory.Count == 0)
                    location = null;
            }

            // The nation to be founded.
            Nation founded_nation = new Nation("Nation of " + _commanded_race.Name, creator);
            founded_nation.FoundingRace = _commanded_race;

            // Decide on the territory it should be settled on. Currently random.
            Terrain territory = null;
            while (territory == null)
            {
                territory = location.UnclaimedTerritory[Main.Constants.RND.Next(location.UnclaimedTerritory.Count)];      
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

            // Add origin order -> church. This church is needed to be able to command this nation.
            Order founder_origin_order = new Order("Church of " + founded_nation.Name, creator, OrderType.Church, OrderPurpose.WorshipFounder);
            founder_origin_order.OrderNation = founded_nation;
            founder_origin_order.OrderRace = null;

            founded_nation.NationalOrders.Add(founder_origin_order);
            creator.CreatedOrders.Add(founder_origin_order);


            // Add nation to the creator and Powers related to this nation.
            creator.FoundedNations.Add(founded_nation);
            creator.Powers.Add(new CreateCity(founded_nation));
            creator.Powers.Add(new FormAlliance(founded_nation));
            creator.Powers.Add(new DeclareWar(founded_nation));

            foreach (Deity deity in current_world.Deities)
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

            foreach (Deity deity in current_world.Deities)
            {
                if (!(deity == creator))
                    deity.Powers.Add(new CreateOrder(OrderType.Church, OrderPurpose.WorshipFounder, founded_nation, null));
            }

            // Add nation to world overview
            current_world.Nations.Add(founded_nation);
            location.Nations.Add(founded_nation);

            creator.LastCreation = founded_nation;
        }

        public FoundNation (Race command_race) : base(command_race)
        {
            Name = "Found Nation: " + command_race.Name;
        }
    }
}
