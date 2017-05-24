using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.CelestialPowers.CommandAvatarPowers
{
    class UsePower : CommandAvatar
    {
        private Power _power;

        protected override void initialize()
        {
            Name = "Use Power: " + _power.Name;
        }

        public override bool isObsolete
        {
            get
            {
                return _power.isObsolete;
            }
        }

        public override bool Precondition(Deity creator)
        {
            return _power.Precondition(creator);
        }
        public override int Cost(Deity creator)
        {
            return _power.Cost(creator);
        }
        public override int Weight(Deity creator)
        {
            return _power.Weight(creator);
        }

        public override int Effect(Deity creator)
        {
            return _power.Effect(creator);
        }

        public UsePower(Avatar avatar, Power power) : base(avatar)
        {
            _power = power;
            initialize();
        }
    }
}
