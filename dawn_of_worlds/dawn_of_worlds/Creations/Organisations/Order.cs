using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Inhabitants;
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

        public bool hasRaceRestriction { get { return OrderRace != null; } }
        public Race OrderRace { get; set; }

        public bool isNationalOrder { get { return OrderNation != null; } }
        public Nation OrderNation { get; set; }

        public Avatar Leader { get; set; }
        public List<Avatar> Members { get; set; }

        public Order(string name, Deity creator, OrderType type, OrderPurpose purpose) : base(name, creator)
        {
            Type = type;
            Purpose = purpose;
            Members = new List<Avatar>();
        }
    }

    public enum OrderType
    {
        Church,
    }

    public enum OrderPurpose
    {
        WorshipFounder,
    }
}
