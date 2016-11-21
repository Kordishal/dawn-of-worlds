using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Inhabitants;

namespace dawn_of_worlds.CelestialPowers.CommandAvatarPowers
{
    abstract class CommandAvatar : Power
    {
        protected Avatar _avatar { get; set; }
        public CommandAvatar(Avatar avatar)
        {
            _avatar = avatar;
        }


        public override int Cost(int current_age)
        {
            switch (current_age)
            {
                case 1:
                    return 2;
                case 2:
                    return 1;
                case 3:
                    return 1;
                default:
                    return 1;
            }
        }

        public override int Weight(World current_world, Deity creator, int current_age)
        {
            switch (current_age)
            {
                case 1:
                    return 10;
                case 2:
                    return 40;
                case 3:
                    return 60;
                default:
                    return 100;
            }
        }
    }
}
