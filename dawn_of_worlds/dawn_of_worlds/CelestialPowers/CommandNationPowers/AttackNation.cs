using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Civilisations;
using dawn_of_worlds.Creations.Conflict;
using dawn_of_worlds.Creations.Diplomacy;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.CelestialPowers.CommandNationPowers
{
    class AttackNation : CommandNation
    {
        private Civilisation _attacked_nation { get; set; }
        private War _war { get; set; }
        private List<WeightedObjects<Army>> _possible_attackers { get; set; }
        private List<WeightedObjects<Army>> _possible_targets { get; set; }

        public override bool Precondition(Deity creator)
        {
            base.Precondition(creator);

            possible_target_armies();

            if (_possible_targets.Count == 0)
                return false;

            if (_possible_attackers.Count == 0)
                return false;

            return true;
        }

        public override int Effect(Deity creator)
        {
            possible_target_armies();

            Army target_army = WeightedObjects<Army>.ChooseRandomObject(_possible_targets);
            Army attacker_army = WeightedObjects<Army>.ChooseRandomObject(_possible_attackers);

            // Move the armies into the same terrain.
            if (!target_army.Location.Equals(attacker_army.Location))
            {
                attacker_army.Location = target_army.Location;
            }

            Battle battle = new Battle(attacker_army.Name + " vs. " + target_army.Name, creator, attacker_army, target_army, target_army.Location, _war);
            battle.Fight();

            creator.LastCreation = null;

            return 0;
        }


        public AttackNation(Civilisation commanded_nation, Civilisation target_nation, War war) : base(commanded_nation)
        {
            _attacked_nation = target_nation;
            _war = war;
            initialize();
        }

        private void possible_target_armies()
        {
            _possible_targets = new List<WeightedObjects<Army>>();

            foreach (Army army in _attacked_nation.Armies)
            {
                if (!army.isScattered)
                {
                    WeightedObjects<Army> temp = new WeightedObjects<Army>(army);
                    temp.Weight = 10;
                    temp.Weight += temp.Object.getTotalModifier() * 10;
                    _possible_targets.Add(temp);
                }
            }

            _possible_attackers = new List<WeightedObjects<Army>>();

            foreach (Army army in _commanded_nation.Armies)
            {
                if (!army.isScattered)
                {
                    WeightedObjects<Army> temp = new WeightedObjects<Army>(army);
                    temp.Weight = 10;
                    temp.Weight += temp.Object.getTotalModifier() * 10;
                    _possible_attackers.Add(temp);
                }
            }

        }

    }
}
