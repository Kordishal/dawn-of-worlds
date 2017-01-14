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
using dawn_of_worlds.Modifiers;
using dawn_of_worlds.Creations.Civilisations;

namespace dawn_of_worlds.CelestialPowers.CommandNationPowers
{
    class FormAlliance : CommandNation
    {
        private List<Civilisation> candidate_nations { get; set; }

        protected override void initialize()
        {
            base.initialize();
            Name = "Form Alliance: " + _commanded_nation.Name;
            Tags = new List<CreationTag>() { CreationTag.Alliance, CreationTag.Diplomacy };
        }

        public override bool Precondition(Deity creator)
        {
            base.Precondition(creator);

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



        public override void Effect(Deity creator)
        {
            compile_candidate_nations();

            // The new ally will be chosen amongst the possible allies at random.
            Civilisation new_ally = candidate_nations[Constants.Random.Next(candidate_nations.Count)];

            _commanded_nation.Relationships.Find(x => x.Target == new_ally).Status = RelationStatus.Allied;
            new_ally.Relationships.Find(x => x.Target == _commanded_nation).Status = RelationStatus.Allied;
            creator.LastCreation = null;
        }


        public FormAlliance(Civilisation commanded_nation): base(commanded_nation)
        {
            initialize();
        }

        private void compile_candidate_nations()
        {
            candidate_nations = new List<Civilisation>();

            foreach (Relations relation in _commanded_nation.Relationships)
            {
                if (relation.Status == RelationStatus.Known)
                {
                    candidate_nations.Add(relation.Target);
                }
            }

        }

    }
}
