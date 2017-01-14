using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Creations.Civilisations;

namespace dawn_of_worlds.Creations.Inhabitants
{
    class Avatar : Creation
    {
        public AvatarType Type { get; set; }

        public Race AvatarRace { get; set; }

        public Civilisation MasterNation { get; set; }

        public Order OrderMembership { get; set; }

        public Avatar(string name, Deity creator) : base(name, creator)
        {

        }
    }


    enum AvatarType
    {
        LegendaryBeast,
        RoyalDynasty,
        HighPriest,
        Champion,
        General,
    }
}
