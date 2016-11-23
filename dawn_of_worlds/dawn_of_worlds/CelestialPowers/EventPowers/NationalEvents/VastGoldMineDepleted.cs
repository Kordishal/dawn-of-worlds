using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Actors;

namespace dawn_of_worlds.CelestialPowers.EventPowers.NationalEvents
{
    class VastGoldMineDepleted : NationalEvent
    {
        public VastGoldMineDepleted(Nation nation) : base(nation)
        {
            Name = "National Event: Vast Gold Vein Found";
        }

        public override int Weight(World current_world, Deity creator, int current_age)
        {
            int weight = base.Weight(current_world, creator, current_age);

            return weight >= 0 ? weight : 0;
        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            _nation.Tags.Remove(NationalTags.VeryRich);
            _nation.Tags.Remove(NationalTags.GoldMine);

            creator.LastCreation = _nation;
        }
    }
}
