using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Creations.Diplomacy;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;
using dawn_of_worlds.Effects;
using dawn_of_worlds.Creations.Civilisations;

namespace dawn_of_worlds.CelestialPowers.CommandNationPowers
{
    class SurrenderWar : CommandNation
    {
        private War _surrendered_war { get; set; }

        protected override void initialize()
        {
            base.initialize();
            Name = "Surrender War (" + _commanded_nation.Name + ")";
            Tags = new List<CreationTag>() { CreationTag.Peace, CreationTag.Diplomacy };
        }

        public override bool isObsolete
        {
            get
            {
                return _surrendered_war.hasEnded;
            }
        }

        public override bool Precondition(Deity creator)
        {
            base.Precondition(creator);

            if (!_commanded_nation.hasDiplomacy)
                return false;

            return true;
        }

        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

            int army_count_attacker = 0;
            int army_count_defender = 0;

            foreach (Civilisation attacker in _surrendered_war.Attackers)
            {
                for (int i = 0; i < attacker.Armies.Count; i++)
                {
                    if (!attacker.Armies[i].isScattered)
                        army_count_attacker += 1;
                }
            }
            foreach (Civilisation defender in _surrendered_war.Defenders)
            {
                for (int i = 0; i < defender.Armies.Count; i++)
                {
                    if (!defender.Armies[i].isScattered)
                        army_count_defender += 1;
                }
            }

            // depending on the amount of armies on one side or the other the weight adjusted.
            if (_surrendered_war.isAttacker(_commanded_nation))
            {
                // whithout armies the attacker is very likely to surrender.
                if (army_count_attacker == 0)
                {
                    weight += 10000;
                }
                else
                {
                    if (army_count_defender - army_count_attacker > 5)
                    {
                        weight += 10;
                    }
                    else if (army_count_defender - army_count_attacker > 10)
                    {
                        weight += 20;
                    }
                    else if (army_count_defender - army_count_attacker > 20)
                    {
                        weight += 50;
                    }
                }
            }
            else
            {
                // without armies the defender is very likely to surrender.
                if (army_count_defender == 0)
                {
                    weight += 10000;
                }
                else
                {
                    if (army_count_attacker - army_count_defender > 5)
                    {
                        weight += 10;
                    }
                    else if (army_count_attacker - army_count_defender > 10)
                    {
                        weight += 20;
                    }
                    else if (army_count_attacker - army_count_defender > 20)
                    {
                        weight += 50;
                    }
                }
            }


            return weight >= 0 ? weight : 0;
        }

        public override void Effect(Deity creator)
        {
            // The war goal which will change hands.
            WarGoal war_goal;
            if (_surrendered_war.isAttacker(_commanded_nation))
                war_goal = _surrendered_war.WarGoalDefenders;
            else
                war_goal = _surrendered_war.WarGoalAttackers;

            // Remove war from war lists
            Program.World.OngoingWars.Remove(_surrendered_war);

            switch (war_goal.Type)
            {
                case WarGoalType.Conquest:
                    war_goal.Territory.changeOwnership(war_goal.Winner);
                    if (_commanded_nation.Territory.Count == 0)
                        _commanded_nation.DestroyNation();
                    break;
                case WarGoalType.RemoveNomadicPresence:
                    break;
                case WarGoalType.VassalizeCity:
                    break;
            }
            _surrendered_war.hasEnded = true;
            _surrendered_war.End = Simulation.Time.Shuffle;

            creator.LastCreation = _surrendered_war;
        }

        public SurrenderWar(Civilisation surrendering_nation, War surrendered_war) : base(surrendering_nation)
        {
            _surrendered_war = surrendered_war;
            initialize();
        }



    
    }
}
