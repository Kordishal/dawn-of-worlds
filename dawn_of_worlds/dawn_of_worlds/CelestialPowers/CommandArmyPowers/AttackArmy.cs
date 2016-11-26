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

        public override int Weight(World current_world, Deity creator, int current_age)
        {
            int weight = base.Weight(current_world, creator, current_age);

            if (creator.Domains.Contains(Domain.Battle))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            if (creator.Domains.Contains(Domain.War))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            if (creator.Domains.Contains(Domain.Peace))
                weight -= Constants.WEIGHT_STANDARD_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            if (isObsolete)
                return false;

            if (_commanded_army.Owner.Wars.Count == 0)
                return false;

            possible_candidate_armies();

            if (candidate_armies.Count == 0)
                return false;

            return true;
        }

        private void possible_candidate_armies()
        {
            candidate_armies.Clear();

            foreach (War on_going_war in _commanded_army.Owner.Wars)
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


        public override void Effect(World current_world, Deity creator, int current_age)
        {

            Army target_army = candidate_armies[Main.Constants.RND.Next(candidate_armies.Count)];

            // Move the armies into the same area.
            if (!target_army.ArmyLocation.Equals(_commanded_army.ArmyLocation))
            {
                _commanded_army.ArmyLocation = target_army.ArmyLocation;
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
