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

        private bool _is_attacker { get; set; }
        private War _surrendered_war { get; set; }

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

            } 


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
