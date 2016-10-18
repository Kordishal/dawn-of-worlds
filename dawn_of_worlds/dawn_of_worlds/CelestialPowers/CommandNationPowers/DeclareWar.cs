using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Organisations;

namespace dawn_of_worlds.CelestialPowers.CommandNationPowers
{
    class DeclareWar : CommandNation
    {

        public override void Effect(World current_world, Deity creator, int current_age)
        {


        }

        public DeclareWar(Nation commanded_nation) : base(commanded_nation)
        {

        }
    }
}
