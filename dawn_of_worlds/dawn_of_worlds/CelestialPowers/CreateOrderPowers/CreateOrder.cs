using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Organisations;

namespace dawn_of_worlds.CelestialPowers.CreateOrderPowers
{
    class CreateOrder : Power
    {

        private OrderType _type { get; set; }
        private OrderPurpose _purpose { get; set; }

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
            Order created_order = new Order("Order of " + Enum.GetName(typeof(OrderPurpose), _purpose), creator, _type, _purpose);
            creator.CreatedOrders.Add(created_order);
            current_world.Orders.Add(created_order);

            switch (_type)
            {
                case OrderType.Religion:
                    

                    break;
            }





        }

        public override int Weight(World current_world, Deity creator, int current_age)
        {
            switch (current_age)
            {
                case 1:
                    return 10;
                case 2:
                    return 50;
                case 3:
                    return 80;
                default:
                    return 100;
            }
        }

        public CreateOrder(OrderType type, OrderPurpose purpose)
        {
            Name = "Create Order: " + Enum.GetName(typeof(OrderType), type) + "|" + Enum.GetName(typeof(OrderPurpose), purpose);
            _type = type;
            _purpose = purpose;
        }
    }
}
