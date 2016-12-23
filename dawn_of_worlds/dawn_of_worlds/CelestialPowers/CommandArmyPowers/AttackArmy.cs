using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Creations.Diplomacy;
using dawn_of_worlds.Creations.Conflict;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.CelestialPowers.CommandArmyPowers
{
    class AttackArmy : CommandArmy
    {

        private List<Army> candidate_armies { get; set; }

        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

            if (creator.Domains.Contains(Domain.Battle))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            if (creator.Domains.Contains(Domain.War))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            if (creator.Domains.Contains(Domain.Peace))
                weight -= Constants.WEIGHT_STANDARD_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        public override bool Precondition(Deity creator)
        {
            if (isObsolete)
                return false;

            if (!_commanded_army.Owner.isAtWar)
                return false;

            possible_candidate_armies();

            if (candidate_armies.Count == 0)
                return false;

            return true;
        }

        private void possible_candidate_armies()
        {
            candidate_armies.Clear();

            foreach (War on_going_war in Program.World.OngoingWars)
            {
                if (on_going_war.isInWar(_commanded_army.Owner))
                {
                    if (!on_going_war.hasEnded)
                    {
                        if (on_going_war.Attackers.Contains(_commanded_army.Owner))
                        {
                            foreach (Nation defender in on_going_war.Defenders)
                            {
                                candidate_armies.AddRange(defender.Armies);
                            }
                        }
                        else
                        {
                            foreach (Nation attacker in on_going_war.Attackers)
                            {
                                candidate_armies.AddRange(attacker.Armies);
                            }
                        }
                    }
                }              
            }
        }


        public override void Effect(Deity creator)
        {
            possible_candidate_armies();

            Army target_army = candidate_armies[Constants.Random.Next(candidate_armies.Count)];

            // Move the armies into the same terrain.
            if (!target_army.Location.Equals(_commanded_army.Location))
            {
                _commanded_army.Location = target_army.Location;
            }

            Battle battle = new Battle(_commanded_army.Name + " vs. " + target_army.Name, creator, _commanded_army, target_army);
            battle.Fight();

            creator.LastCreation = null;
        }


        public AttackArmy(Army commanded_army) : base(commanded_army)
        {
            Name = "Attack Army: " + _commanded_army.Name;
            candidate_armies = new List<Army>();
        }
    }
}
