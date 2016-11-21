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

namespace dawn_of_worlds.CelestialPowers.CommandNationPowers
{
    class SurrenderWar : CommandNation
    {

        public override bool isObsolete
        {
            get
            {
                return _surrendered_war.hasEnded;
            }
        }

        private bool _is_attacker { get; set; }
        private War _surrendered_war { get; set; }

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            // If nation no longer exists.
            if (isObsolete)
                return false;

            return true;
        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            // The war goal which will change hands.
            WarGoal war_goal;
            if (_is_attacker)
                war_goal = _surrendered_war.WarGoalDefenders;
            else
                war_goal = _surrendered_war.WarGoalAttackers;

            // Remove war from war lists
            current_world.OngoingWars.Remove(_surrendered_war);
            _commanded_nation.Wars.Remove(_surrendered_war);
            war_goal.Winner.Wars.Remove(_surrendered_war);

            // Give city to victor
            war_goal.WarGoalCity.Owner = war_goal.Winner;
            war_goal.Winner.Cities.Add(war_goal.WarGoalCity);
            // Assign territory to victor
            foreach (GeographicalFeature gc in war_goal.WarGoalCity.CitySphereOfÌnfluence)
            {
                gc.Owner = war_goal.Winner;
            }


            // remove the city from the loser.
            _commanded_nation.Cities.Remove(war_goal.WarGoalCity);

            // if last city has been removed from a nation the nation is destroyed.
            if (_commanded_nation.Cities.Count == 0)
            {
                _commanded_nation.DestroyNation();
            }

            _surrendered_war.hasEnded = true;

            creator.LastCreation = null;
        }


        public override int Weight(World current_world, Deity creator, int current_age)
        {
            int weight = 0;
            switch (current_age)
            {
                case 1:
                    weight += 10;
                    break;
                case 2:
                    weight += 40;
                    break;
                case 3:
                    weight += 60;
                    break;
                default:
                    weight += 100;
                    break;
            }

            int army_count_attacker = 0;
            int army_count_defender = 0;

            foreach (Nation attacker in _surrendered_war.Attackers)
            {
                for (int i = 0; i < attacker.Armies.Count; i++)
                {
                    if (!attacker.Armies[i].isScattered)
                        army_count_attacker += 1;
                }
            }
            foreach (Nation defender in _surrendered_war.Defenders)
            {
                for (int i = 0; i < defender.Armies.Count; i++)
                {
                    if (!defender.Armies[i].isScattered)
                        army_count_defender += 1;
                }
            }

            // depending on the amount of armies on one side or the other the weight adjusted.
            if (_is_attacker)
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

            
            return weight;
        }

        public SurrenderWar(Nation surrendering_nation, War surrendered_war) : base(surrendering_nation)
        {
            Name = "Surrender War (" + surrendered_war.Name + "): " + surrendering_nation;

            _surrendered_war = surrendered_war;

            if (surrendering_nation == surrendered_war.Attackers[0])
                _is_attacker = true;
            else
                _is_attacker = false;
        }
    }
}
