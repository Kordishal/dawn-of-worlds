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
using dawn_of_worlds.Log;

namespace dawn_of_worlds.CelestialPowers.CommandNationPowers
{
    class DeclareWar : CommandNation
    {
        private List<Nation> candidate_nations { get; set; }

        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

            if (creator.Domains.Contains(Domain.War))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            if (creator.Domains.Contains(Domain.Battle))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            if (creator.Domains.Contains(Domain.Peace))
                weight -= Constants.WEIGHT_STANDARD_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        public override bool Precondition(Deity creator)
        {
            // If nation no longer exists.
            if (isObsolete)
                return false;

            if (!_commanded_nation.hasDiplomacy)
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

        public override void Effect(Deity creator)
        {
            Nation war_target = candidate_nations[Constants.Random.Next(candidate_nations.Count)];

            // The war to be declared.
            War declared_war = new War("War of " + _commanded_nation.Name + " vs. " + war_target.Name, creator);
            declared_war.Attackers.Add(_commanded_nation);
            declared_war.Defenders.Add(war_target);

            // Add allies to the war. The order is important as all nations which are allied to both nations will side with the defender.
            foreach (Relations relation in war_target.Relationships)
                if (relation.Status == RelationStatus.Allied)
                    declared_war.Defenders.Add(relation.Target);

            foreach (Relations relation in _commanded_nation.Relationships)
                if (relation.Status == RelationStatus.Allied)
                    if (!declared_war.Defenders.Contains(relation.Target))
                        declared_war.Attackers.Add(relation.Target);
            

            List<WeightedObjects<WarGoal>>[] war_goals = new List<WeightedObjects<WarGoal>>[2];
            for (int i = 0; i < 2; i++)
            {
                war_goals[i] = new List<WeightedObjects<WarGoal>>();
                foreach (WarGoal war_goal in war_target.PossibleWarGoals)
                    war_goals[i].Add(new WeightedObjects<WarGoal>(war_goal));

                foreach (WeightedObjects<WarGoal> weighted_war_goal in war_goals[i])
                {
                    Nation taker, target;
                    if (i == 0)
                    {
                        taker = _commanded_nation;
                        target = war_target;
                    }
                    else
                    {
                        taker = war_target;
                        target = _commanded_nation;
                    }

                    weighted_war_goal.Object.Winner = taker;

                    switch (taker.Type)
                    {
                        case NationTypes.TribalNation:
                        case NationTypes.LairTerritory:
                        case NationTypes.FeudalNation:
                            switch (target.Type)
                            {
                                case NationTypes.LairTerritory:
                                case NationTypes.TribalNation:
                                case NationTypes.FeudalNation:
                                    if (weighted_war_goal.Object.Type == WarGoalType.CityConquest)
                                        weighted_war_goal.Weight += Constants.WEIGHT_STANDARD_CHANGE * 2;
                                    if (weighted_war_goal.Object.Type == WarGoalType.Conquest)
                                        weighted_war_goal.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                                    break;
                                case NationTypes.NomadicTribe:
                                    if (weighted_war_goal.Object.Type == WarGoalType.RemoveNomadicPresence)
                                        weighted_war_goal.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                                    break;
                            }
                            break;
                        case NationTypes.NomadicTribe:
                            switch (target.Type)
                            {
                                case NationTypes.FeudalNation:
                                case NationTypes.TribalNation:
                                case NationTypes.LairTerritory:
                                    if (weighted_war_goal.Object.Type == WarGoalType.VassalizeCity)
                                        weighted_war_goal.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                                    break;
                                case NationTypes.NomadicTribe:
                                    if (weighted_war_goal.Object.Type == WarGoalType.TravelAreaConquest)
                                        weighted_war_goal.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                                    break;
                            }
                            break;
                    }

                }
            }

            List<WarGoal> weighted_war_goals = WeightedObjects<WarGoal>.ChooseHeaviestObjects(war_goals[0]);
            declared_war.WarGoalAttackers = weighted_war_goals[Constants.Random.Next(weighted_war_goals.Count)];

            weighted_war_goals = WeightedObjects<WarGoal>.ChooseHeaviestObjects(war_goals[1]);
            declared_war.WarGoalDefenders = weighted_war_goals[Constants.Random.Next(weighted_war_goals.Count)];


            // Add war to the list of ongoing conflicts.
            Program.World.OngoingWars.Add(declared_war);

            declared_war.Begin = Simulation.Time.Shuffle;

            // Add powers related to the war to connected deities.
            // attacker related
            // Only the war leader can surrender as only he stands to lose anything, all other participants can only white peace.
            creator.Powers.Add(new SurrenderWar(_commanded_nation, declared_war));
            foreach (Nation attacker in declared_war.Attackers)
            {
                creator.Powers.Add(new WhitePeace(attacker, declared_war));

                foreach (Nation defender in declared_war.Defenders)
                    creator.Powers.Add(new AttackNation(attacker, defender, declared_war));
            }


            // defender related
            declared_war.Defenders[0].Creator.Powers.Add(new SurrenderWar(declared_war.Defenders[0], declared_war));
            foreach (Nation defender in declared_war.Defenders)
            {
                defender.Creator.Powers.Add(new WhitePeace(defender, declared_war));

                foreach (Nation attacker in declared_war.Defenders)
                    creator.Powers.Add(new AttackNation(defender, attacker, declared_war));
            }
            creator.LastCreation = declared_war;

            Program.WorldHistory.AddRecord(RecordType.WarReport, declared_war, War.printWar);
        }

        public DeclareWar(Nation commanded_nation) : base(commanded_nation)
        {
            Name = "Declare War: " + commanded_nation.Name;
            candidate_nations = new List<Nation>();
        }
    }
}
