using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.WorldClasses;

namespace dawn_of_worlds.CelestialPowers.EventPowers.NationalEvents
{
    class VastGoldVeinFound : NationalEvent
    {
        public VastGoldVeinFound(Nation nation) : base(nation)
        {
            Name = "National Event: Vast Gold Vein Found";
        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            _nation.Tags.Add(NationalTags.VeryRich);

            creator.LastCreation = _nation;
        }
    }
}
