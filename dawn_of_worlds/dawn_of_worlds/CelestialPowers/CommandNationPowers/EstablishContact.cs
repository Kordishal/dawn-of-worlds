using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Diplomacy;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Main;
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

        private List<Nation> candidate_nations { get; set; }

        public override int Weight(World current_world, Deity creator, int current_age)
        {
            int weight = base.Weight(current_world, creator, current_age);

            return weight >= 0 ? weight : 0;
        }

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            // If nation no longer exists.
            if (isObsolete)
                return false;

            compile_candidate_nations();

            // a nation needs to be close by or known to an ally.
            if (candidate_nations.Count > 0)
                return true;

            return false;
        }


        private void compile_candidate_nations()
        {
            candidate_nations.Clear();

            foreach (Relations relation in _commanded_nation.Relationships)
            {
                // Unknown nations can become known when they have territory in the same terrain. 
                if (relation.Status == RelationStatus.Unknown)
                { 
                    foreach (Terrain terrain in relation.Target.TerrainTerritory)
                    {
                        if (_commanded_nation.TerrainTerritory.Contains(terrain))
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
                        if (!(ally_relation.Status == RelationStatus.Unknown))
                        {
                            candidate_nations.Add(ally_relation.Target);
                        }
                    }
                }
            }

        }


        public override void Effect(World current_world, Deity creator, int current_age)
        {
            compile_candidate_nations();

            // The new contact will be chosen amongst the possible contacts at random.
            Nation new_contact = candidate_nations[Constants.RND.Next(candidate_nations.Count)];

            _commanded_nation.Relationships.Find(x => x.Target == new_contact).Status = RelationStatus.Known;
            new_contact.Relationships.Find(x => x.Target == _commanded_nation).Status = RelationStatus.Known;
            creator.LastCreation = null;
        }


        public EstablishContact(Nation commanded_nation): base(commanded_nation)
        {
            Name = "Establish Contact: " + commanded_nation.Name;
            candidate_nations = new List<Nation>();
            compile_candidate_nations();
        }
    }
}
