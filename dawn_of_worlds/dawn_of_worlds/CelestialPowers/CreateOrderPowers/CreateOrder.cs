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
using dawn_of_worlds.Modifiers;

namespace dawn_of_worlds.CelestialPowers.CreateOrderPowers
{
    class CreateOrder : Power
    {
        private bool isCreated { get; set; }

        private OrderType _type { get; set; }
        private OrderPurpose _purpose { get; set; }

        private Civilisation _nation { get; set; }
        private Race _race { get; set; }

        protected override void initialize()
        {
            Name = "Create Order: " + Enum.GetName(typeof(OrderType), _type) + "|" + Enum.GetName(typeof(OrderPurpose), _purpose);
            BaseCost = new int[] { 8, 6, 4 };
            CostChange = Constants.COST_CHANGE_VALUE;

            BaseWeight = new int[] { Constants.WEIGHT_STANDARD_LOW, Constants.WEIGHT_STANDARD_MEDIUM, Constants.WEIGHT_STANDARD_HIGH };
            WeightChange = Constants.WEIGHT_STANDARD_CHANGE;

            Tags = new List<CreationTag>() { CreationTag.Creation, CreationTag.Community };
        }

        public override bool isObsolete
        {
            get
            {
                return isCreated;
            }
        }


        public override bool Precondition(Deity creator)
        {
            base.Precondition(creator);

            if (_nation != null && _nation.isDestroyed)
                return false;

            return true;
        }

        public override void Effect(Deity creator)
        {
            Order created_order = new Order("PlaceHolder", creator, _type, _purpose);
            creator.CreatedOrders.Add(created_order);
            Program.World.Orders.Add(created_order);


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
                    creator.Powers.Add(new ExpandTerritory(_nation));
                    creator.Powers.Add(new EstablishContact(_nation));
                    creator.Powers.Add(new FormAlliance(_nation));
                    creator.Powers.Add(new DeclareWar(_nation));

                    foreach (City city in _nation.Cities)
                    {
                        creator.Powers.Add(new RaiseArmy(city));
                    }

                    isCreated = true;

                    creator.Powers.Add(new CreateAvatar(AvatarType.HighPriest, created_order.hasRaceRestriction ? created_order.OrderRace : created_order.OrderNation.FoundingRace, created_order.OrderNation, created_order));
                }
            }

            //created_order.Name = Constants.Names.GetReligionName(creator, created_order.OrderRace);
           
            creator.LastCreation = created_order;
        }


        public CreateOrder(OrderType type, OrderPurpose purpose, Civilisation nation, Race race)
        {
            _type = type;
            _purpose = purpose;
            _nation = nation;
            _race = race;
            isCreated = false;
            initialize();
        }
    }
}
