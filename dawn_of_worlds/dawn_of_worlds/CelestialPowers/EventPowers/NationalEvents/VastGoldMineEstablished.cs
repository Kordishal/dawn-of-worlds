using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Main;
using dawn_of_worlds.Effects;
using dawn_of_worlds.Creations.Civilisations;

namespace dawn_of_worlds.CelestialPowers.EventPowers.NationalEvents
{
    class VastGoldMineEstablised : CivilisationEvents
    {

        protected override void initialize()
        {
            base.initialize();
            Name = "National Event: Vast Gold Vein Established (" + _nation.Name + ")";
            Tags = new List<CreationTag>() { CreationTag.Gold };
        }

        public override bool Precondition(Deity creator)
        {
            base.Precondition(creator);
            if (_nation.LocalTags.Contains(CivilisationTags.GoldMine))
                return false;

            return true;
        }

        public override int Effect(Deity creator)
        {
            _nation.LocalTags.Add(CivilisationTags.GoldMine);

            creator.LastCreation = _nation;
            return 0;
        }

        public VastGoldMineEstablised(Civilisation nation) : base(nation) { }
    }
}
