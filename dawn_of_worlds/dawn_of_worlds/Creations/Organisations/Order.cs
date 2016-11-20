using dawn_of_worlds.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Creations.Organisations
{
    class Order : Creation 
    {
        public OrderType Type { get; set; }
        public OrderPurpose Purpose { get; set; }

        public Order(string name, Deity creator, OrderType type, OrderPurpose purpose) : base(name, creator)
        {
            Type = type;
            Purpose = purpose;
        }
    }

    public enum OrderType
    {
        Religion,
    }

    public enum OrderPurpose
    {
        WorshipFounder,
    }
}
