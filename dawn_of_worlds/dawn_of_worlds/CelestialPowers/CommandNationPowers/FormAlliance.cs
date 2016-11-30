using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Creations.Diplomacy;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.CelestialPowers.CommandNationPowers
{
    class FormAlliance : CommandNation
    {

        private List<Nation> candidate_nations { get; set; }

        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

            if (creator.Domains.Contains(Domain.War))
                weight -= Constants.WEIGHT_STANDARD_CHANGE;

            if (creator.Domains.Contains(Domain.Peace))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        public override bool Precondition(Deity creator)
        {
            // If nation no longer exists.
            if (isObsolete)
                return false;

            if (!_commanded_nation.hasDiplomacy)
                return false;

            // cannot make new alliances while at war.
            if (_commanded_nation.isAtWar)
                return false;

            compile_candidate_nations();

            // needs a nation it can ally with.
            if (candidate_nations.Count > 0)
                return true;

            return false;
        }

        private void compile_candidate_nations()
        {
            candidate_nations.Clear();

            foreach(Relations relation in _commanded_nation.Relationships)
            {
                if (relation.Status == RelationStatus.Known)
                {
                    candidate_nations.Add(relation.Target);
                }
            }

        }

        public override void Effect(Deity creator)
        {
            compile_candidate_nations();

            // The new ally will be chosen amongst the possible allies at random.
            Nation new_ally = candidate_nations[Constants.RND.Next(candidate_nations.Count)];

            _commanded_nation.Relationships.Find(x => x.Target == new_ally).Status = RelationStatus.Allied;
            new_ally.Relationships.Find(x => x.Target == _commanded_nation).Status = RelationStatus.Allied;
            creator.LastCreation = null;
        }


        public FormAlliance(Nation commanded_nation): base(commanded_nation)
        {
            Name = "Form Alliance: " + commanded_nation.Name;
            candidate_nations = new List<Nation>();
            compile_candidate_nations();
        }
    }
}
