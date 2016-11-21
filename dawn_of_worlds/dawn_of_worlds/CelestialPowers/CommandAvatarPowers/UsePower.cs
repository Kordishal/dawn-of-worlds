using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.WorldClasses;

namespace dawn_of_worlds.CelestialPowers.CommandAvatarPowers
{
    class UsePower : CommandAvatar
    {
        private Power _power;

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            return _power.Precondition(current_world, creator, current_age);
        }

        public override bool isObsolete
        {
            get
            {
                return _power.isObsolete;
            }
        }

        public UsePower(Avatar avatar, Power power) : base(avatar)
        {
            Name = "Use Power: " + power.Name;
            _power = power;
        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            _power.Effect(current_world, creator, current_age);
        }
    }
}
