using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Creations.Diplomacy;

namespace dawn_of_worlds.CelestialPowers.CommandNationPowers
{
    class DeclareWar : CommandNation
    {

        private List<Nation> candidate_nations { get; set; }

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            // A nation cannot declare a war while at war. (but can be called into one as an ally)
            if (_commanded_nation.Wars.Count > 0)
                return false;

            // Any attacker needs at least one army.
            if (_commanded_nation.Armies.Count == 0)
                return false;

            compile_candidate_nations();

            // needs a war target
            if (candidate_nations.Count > 0)
                return true;

            return false;
        }

        private void compile_candidate_nations()
        {
            candidate_nations.Clear();

            List<Area> considered_areas = new List<Area>();
            foreach (Area settled_area in _commanded_nation.TerritoryAreas)
            {
                considered_areas.Add(settled_area);
                foreach (Area neighbour_area in settled_area.Neighbours)
                {
                    if (neighbour_area != null && !_commanded_nation.TerritoryAreas.Contains(neighbour_area))
                    {
                        considered_areas.Add(neighbour_area);
                    }
                }
            }

            foreach (Area area in considered_areas)
            {
                bool is_at_war = false;
                foreach (Nation nation in area.Nations)
                {
                    // Checks whether the nation is already in an alliance with the commanded nation.
                    is_at_war = false;
                    foreach (War war in _commanded_nation.Wars)
                    {
                        if (war.Attackers.Contains(nation) || war.Defenders.Contains(nation))
                        {
                            is_at_war = true;
                        }
                    }

                    if (!is_at_war)
                        candidate_nations.Add(nation);

                }
            }

        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            Nation war_target = null;

            while (war_target == null)
            {
                war_target = candidate_nations[Main.MainLoop.RND.Next(candidate_nations.Count)];
            }

            War declared_war = new War("War of " + _commanded_nation.Name + " vs. " + war_target.Name, creator);
            declared_war.Attackers.Add(_commanded_nation);
            declared_war.Defenders.Add(war_target);

            _commanded_nation.Wars.Add(declared_war);
            war_target.Wars.Add(declared_war);

            declared_war.Attackers.AddRange(add_allies(_commanded_nation));
            declared_war.Defenders.AddRange(add_allies(war_target));
            

            if (war_target.Cities.Count == 0)
            {
                declared_war.WarGoalAttackers = new WarGoal(null, war_target.Territory);
            }
            else
            {
                declared_war.WarGoalAttackers = new WarGoal(war_target.Cities[Main.MainLoop.RND.Next(war_target.Cities.Count)], null);
            }

            if (_commanded_nation.Cities.Count == 0)
            {
                declared_war.WarGoalDefenders = new WarGoal(null, _commanded_nation.Territory);
            }
            else
            {
                declared_war.WarGoalDefenders = new WarGoal(_commanded_nation.Cities[Main.MainLoop.RND.Next(_commanded_nation.Cities.Count)], null);
            }

            bool has_no_armies = true;
            foreach (Nation n in declared_war.Defenders)
            {
                if (n.Armies.Count > 0)
                {
                    has_no_armies = false;
                }
            }

            // if the defenders have no armies the attacker wins by default.
            if (has_no_armies)
            {

            }

        }


        private List<Nation> add_allies(Nation nation)
        {
            List<Nation> allies = new List<Nation>();

            foreach (Alliance alliance in nation.Alliances)
            {
                foreach (Nation n in alliance.Members)
                {
                    if (!n.Equals(nation))
                    {
                        allies.Add(n);
                    }
                }
            }

            return allies;
        }

        public DeclareWar(Nation commanded_nation) : base(commanded_nation)
        {

        }
    }
}
