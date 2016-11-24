using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.CelestialPowers.CommandNationPowers;
using dawn_of_worlds.CelestialPowers.CommandCityPowers;
using dawn_of_worlds.CelestialPowers.CreateAvatarPowers;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.CelestialPowers.CreateOrderPowers
{
    class CreateOrder : Power
    {
        private bool isCreated { get; set; }

        private OrderType _type { get; set; }
        private OrderPurpose _purpose { get; set; }

        private Nation _nation { get; set; }
        private Race _race { get; set; }

        public override bool isObsolete
        {
            get
            {
                return isCreated;
            }
        }

        public override int Cost(int current_age)
        {
            switch (current_age)
            {
                case 1:
                    return 8;
                case 2:
                    return 6;
                case 3:
                    return 4;
                default:
                    return 2;
            }
        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            Order created_order = new Order("PlaceHolder", creator, _type, _purpose);
            creator.CreatedOrders.Add(created_order);
            current_world.Orders.Add(created_order);


            if (_nation != null)
                created_order.OrderNation = _nation;

            if (_race != null)
                created_order.OrderRace = _race;


            // The founder/creator religion. People will worship their creator.
            if (created_order.Type == OrderType.Church && created_order.Purpose == OrderPurpose.FounderWorship)
            {
                if (created_order.isNationalOrder)
                {
                    _nation.NationalOrders.Add(created_order);

                    creator.Powers.Add(new CreateCity(_nation));
                    creator.Powers.Add(new FormAlliance(_nation));
                    creator.Powers.Add(new DeclareWar(_nation));

                    foreach (City city in _nation.Cities)
                    {
                        creator.Powers.Add(new ExpandCityInfluence(city));
                        creator.Powers.Add(new RaiseArmy(city));
                    }

                    isCreated = true;

                    creator.Powers.Add(new CreateAvatar(AvatarType.HighPriest, created_order.hasRaceRestriction ? created_order.OrderRace : created_order.OrderNation.FoundingRace, created_order.OrderNation, created_order));
                }
            }

            //created_order.Name = Constants.Names.GetReligionName(creator, created_order.OrderRace);
           
            creator.LastCreation = created_order;
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
                    weight += Constants.WEIGHT_STANDARD_MEDIUM;
                    break;
                case 3:
                    weight += Constants.WEIGHT_STANDARD_HIGH;
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

        public CreateOrder(OrderType type, OrderPurpose purpose, Nation nation, Race race)
        {
            Name = "Create Order: " + Enum.GetName(typeof(OrderType), type) + "|" + Enum.GetName(typeof(OrderPurpose), purpose);
            _type = type;
            _purpose = purpose;
            _nation = nation;
            _race = race;
            isCreated = false;
        }
    }
}
