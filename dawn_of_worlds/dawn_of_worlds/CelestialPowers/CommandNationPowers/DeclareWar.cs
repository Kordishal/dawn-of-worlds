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

namespace dawn_of_worlds.CelestialPowers.CommandNationPowers
{
    class DeclareWar : CommandNation
    {

        private List<Nation> candidate_nations { get; set; }


        public override int Weight(World current_world, Deity creator, int current_age)
        {
            int weight = base.Weight(current_world, creator, current_age);

            if (creator.Domains.Contains(Domain.War))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            if (creator.Domains.Contains(Domain.Battle))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            if (creator.Domains.Contains(Domain.Peace))
                weight -= Constants.WEIGHT_STANDARD_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            // If nation no longer exists.
            if (isObsolete)
                return false;

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
                bool is_in_alliance = false;
                foreach (Nation nation in area.Nations)
                {
                    // Checks whether the nation is already in a War with the commanded nation.
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

                    // Checks whether the nation is in an alliance with the commanded nation
                    is_in_alliance = false;
                    foreach (Nation allied_nation in _commanded_nation.AlliedNations)
                    {
                        if (allied_nation.Equals(nation))
                        {
                            is_in_alliance = true;
                        }
                    }

                    if (!is_in_alliance)
                        candidate_nations.Add(nation);

                }
            }

        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            Nation war_target = null;

            while (war_target == null)
            {
                war_target = candidate_nations[Main.Constants.RND.Next(candidate_nations.Count)];
            }

            // The war to be declared.
            War declared_war = new War("War of " + _commanded_nation.Name + " vs. " + war_target.Name, creator);
            declared_war.Attackers.Add(_commanded_nation);
            declared_war.Defenders.Add(war_target);

            // Add allies to the war. The order is important as all nations which are allied to both nations will side with the defender.
            declared_war.Defenders.AddRange(war_target.AlliedNations);

            foreach (Nation n in _commanded_nation.AlliedNations)
            {
                if (!declared_war.Defenders.Contains(n))
                    declared_war.Attackers.Add(n);
            }

            // Define war goals
            declared_war.WarGoalAttackers = new WarGoal(_commanded_nation, war_target.Cities[Main.Constants.RND.Next(war_target.Cities.Count)]);
            declared_war.WarGoalDefenders = new WarGoal(war_target, _commanded_nation.Cities[Main.Constants.RND.Next(_commanded_nation.Cities.Count)]);

            // Add war to each nation
            foreach (Nation n in declared_war.Attackers)
            {
                n.Wars.Add(declared_war);
            }
            foreach (Nation n in declared_war.Defenders)
            {
                n.Wars.Add(declared_war);
            }

            // Add war to the list of ongoing conflicts.
            current_world.OngoingWars.Add(declared_war);

            // Add powers related to the war to connected deities.
            // attacker related
            creator.Powers.Add(new SurrenderWar(_commanded_nation, declared_war));

            // defender related
            declared_war.Defenders[0].Creator.Powers.Add(new SurrenderWar(declared_war.Defenders[0], declared_war));

            creator.LastCreation = declared_war;
        }

        public DeclareWar(Nation commanded_nation) : base(commanded_nation)
        {
            Name = "Declare War: " + commanded_nation.Name;
            candidate_nations = new List<Nation>();
        }
    }
}
