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
            if (_commanded_nation.isAtWar)
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

            foreach (Relations relation in _commanded_nation.Relationships)
            {
                if (relation.Status == RelationStatus.Known)
                    candidate_nations.Add(relation.Target);
            }

        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            Nation war_target = candidate_nations[Constants.RND.Next(candidate_nations.Count)];

            // The war to be declared.
            War declared_war = new War("War of " + _commanded_nation.Name + " vs. " + war_target.Name, creator);
            declared_war.Attackers.Add(_commanded_nation);
            declared_war.Defenders.Add(war_target);

            // Add allies to the war. The order is important as all nations which are allied to both nations will side with the defender.
            for (int i = 0; i < _commanded_nation.Relationships.Count; i++)
            {
                if (war_target.Relationships[i].Status == RelationStatus.Allied)
                    declared_war.Defenders.Add(war_target.Relationships[i].Target);

                if (_commanded_nation.Relationships[i].Status == RelationStatus.Allied)
                    if (!declared_war.Defenders.Contains(_commanded_nation.Relationships[i].Target))
                        declared_war.Attackers.Add(_commanded_nation.Relationships[i].Target);
            }

            // Define war goals
            declared_war.WarGoalAttackers = new WarGoal(_commanded_nation, war_target.Cities[Constants.RND.Next(war_target.Cities.Count)]);
            declared_war.WarGoalDefenders = new WarGoal(war_target, _commanded_nation.Cities[Constants.RND.Next(_commanded_nation.Cities.Count)]);

            // Add war to the list of ongoing conflicts.
            current_world.OngoingWars.Add(declared_war);

            // Add powers related to the war to connected deities.
            // attacker related
            creator.Powers.Add(new SurrenderWar(_commanded_nation, declared_war));
            foreach (Nation nation in declared_war.Attackers)
            {
                creator.Powers.Add(new WhitePeace(nation, declared_war));
            }


            // defender related
            declared_war.Defenders[0].Creator.Powers.Add(new SurrenderWar(declared_war.Defenders[0], declared_war));
            foreach (Nation nation in declared_war.Defenders)
            {
                nation.Creator.Powers.Add(new WhitePeace(nation, declared_war));
            }
            creator.LastCreation = declared_war;
        }

        public DeclareWar(Nation commanded_nation) : base(commanded_nation)
        {
            Name = "Declare War: " + commanded_nation.Name;
            candidate_nations = new List<Nation>();
        }
    }
}
