using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Civilisations;
using dawn_of_worlds.Creations.Diplomacy;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Main;
using dawn_of_worlds.Effects;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.CelestialPowers.CommandNationPowers
{
    class EstablishContact : CommandNation
    {

        private List<Civilisation> candidate_nations { get; set; }

        protected override void initialize()
        {
            base.initialize();
            Name = "Establish Contact (" + _commanded_nation.Name + ")";
            Tags = new List<CreationTag>() { CreationTag.Diplomacy };
        }

        public override bool Precondition(Deity creator)
        {
            base.Precondition(creator);

            if (!_commanded_nation.hasDiplomacy)
                return false;

            compile_candidate_nations();

            // a nation needs to be close by or known to an ally.
            if (candidate_nations.Count > 0)
                return true;

            return false;
        }

        public override void Effect(Deity creator)
        {
            compile_candidate_nations();

            // The new contact will be chosen amongst the possible contacts at random.
            Civilisation new_contact = candidate_nations[Constants.Random.Next(candidate_nations.Count)];

            _commanded_nation.Relationships.Find(x => x.Target.Equals(new_contact)).Status = RelationStatus.Known;
            new_contact.Relationships.Find(x => x.Target.Equals(_commanded_nation)).Status = RelationStatus.Known;
            creator.LastCreation = null;
        }


        public EstablishContact(Civilisation commanded_nation): base(commanded_nation)
        {
            initialize();
        }

        private void compile_candidate_nations()
        {
            candidate_nations = new List<Civilisation>();

            foreach (Relations relation in _commanded_nation.Relationships)
            {
                // Unknown nations can become known when they have territory in the same terrain. 
                if (relation.Status == RelationStatus.Unknown)
                {
                    foreach (Province province in relation.Target.Territory)
                    {
                        if (_commanded_nation.Territory.Contains(province))
                        {
                            candidate_nations.Add(relation.Target);
                        }
                    }
                }
                // Add all contacts of allies as well. This can lead to a single nation being added several times making them more likely to be discovered.
                else if (relation.Status == RelationStatus.Allied)
                {
                    foreach (Relations ally_relation in relation.Target.Relationships)
                    {
                        if (!(ally_relation.Status == RelationStatus.Unknown) && !(ally_relation.Target.Equals(_commanded_nation)))
                        {
                            candidate_nations.Add(ally_relation.Target);
                        }
                    }
                }
            }

        }
    }
}
