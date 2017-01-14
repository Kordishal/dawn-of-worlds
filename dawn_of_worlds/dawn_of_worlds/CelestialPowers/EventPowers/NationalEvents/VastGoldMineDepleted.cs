using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Modifiers;
using dawn_of_worlds.Creations.Civilisations;

namespace dawn_of_worlds.CelestialPowers.EventPowers.NationalEvents
{
    class VastGoldMineDepleted : CivilisationEvents
    {
        protected override void initialize()
        {
            base.initialize();
            Name = "National Event: Vast Gold Mine Depleted (" + _nation.Name + ")";
            Tags = new List<CreationTag>() { CreationTag.Subterranean, CreationTag.Depletion };
        }


        public override bool Precondition(Deity creator)
        {
            base.Precondition(creator);

            if (!_nation.LocalTags.Contains(CivilisationTags.GoldMine))
                return false;

            return true;
        }

        public override void Effect(Deity creator)
        {
            _nation.LocalTags.Remove(CivilisationTags.VeryRich);
            _nation.LocalTags.Remove(CivilisationTags.GoldMine);

            creator.LastCreation = _nation;
        }


        public VastGoldMineDepleted(Civilisation nation) : base(nation) { }
    }
}
