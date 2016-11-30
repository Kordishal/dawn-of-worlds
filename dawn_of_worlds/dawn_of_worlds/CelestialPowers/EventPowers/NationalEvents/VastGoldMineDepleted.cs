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

        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

            return weight >= 0 ? weight : 0;
        }

        public override void Effect(Deity creator)
        {
            _nation.Tags.Remove(NationalTags.VeryRich);
            _nation.Tags.Remove(NationalTags.GoldMine);

            creator.LastCreation = _nation;
        }
    }
}
