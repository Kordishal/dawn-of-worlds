using System.Collections.Generic;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Creations.Diplomacy;
using dawn_of_worlds.Main;
using dawn_of_worlds.Log;
using dawn_of_worlds.Effects;
using dawn_of_worlds.Creations.Civilisations;

namespace dawn_of_worlds.CelestialPowers.CommandNationPowers
{
    class DeclareWar : CommandNation
    {
        private List<Civilisation> candidate_nations { get; set; }

        protected override void initialize()
        {
            base.initialize();
            Name = "Declare War: " + _commanded_nation.Name;
            Tags = new List<CreationTag>() { CreationTag.War };
        }

        public override bool Precondition(Deity creator)
        {
            base.Precondition(creator);

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



        public override int Effect(Deity creator)
        {
            Civilisation war_target = candidate_nations[Constants.Random.Next(candidate_nations.Count)];

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

                // determine the weights of each war goal. Certain types of polities can only take certain types of war goals.
                foreach (WeightedObjects<WarGoal> weighted_war_goal in war_goals[i])
                {
                    Civilisation taker, target;
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
            
                    if (taker.isNomadic)
                    {
                        if (target.isNomadic)
                            if (weighted_war_goal.Object.Type == WarGoalType.Conquest)
                                weighted_war_goal.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                        else
                            if (weighted_war_goal.Object.Type == WarGoalType.VassalizeCity)
                                weighted_war_goal.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                    }
                    else
                    {
                        if (target.isNomadic)
                        {
                            if (weighted_war_goal.Object.Type == WarGoalType.RemoveNomadicPresence)
                                weighted_war_goal.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                        }
                        else
                        {
                            if (weighted_war_goal.Object.Type == WarGoalType.CityConquest)
                                weighted_war_goal.Weight += Constants.WEIGHT_STANDARD_CHANGE * 2;
                            if (weighted_war_goal.Object.Type == WarGoalType.Conquest)
                                weighted_war_goal.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                        }
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
            foreach (Civilisation attacker in declared_war.Attackers)
            {
                creator.Powers.Add(new WhitePeace(attacker, declared_war));

                foreach (Civilisation defender in declared_war.Defenders)
                    creator.Powers.Add(new AttackNation(attacker, defender, declared_war));
            }


            // defender related
            declared_war.Defenders[0].Creator.Powers.Add(new SurrenderWar(declared_war.Defenders[0], declared_war));
            foreach (Civilisation defender in declared_war.Defenders)
            {
                defender.Creator.Powers.Add(new WhitePeace(defender, declared_war));

                foreach (Civilisation attacker in declared_war.Defenders)
                    creator.Powers.Add(new AttackNation(defender, attacker, declared_war));
            }
            creator.LastCreation = declared_war;

            Program.WorldHistory.AddRecord(RecordType.WarReport, declared_war, War.printWar);

            return 0;
        }

        public DeclareWar(Civilisation commanded_nation) : base(commanded_nation)
        {
            initialize();
        }

        private void compile_candidate_nations()
        {
            candidate_nations = new List<Civilisation>();

            foreach (Relations relation in _commanded_nation.Relationships)
            {
                if (relation.Status == RelationStatus.Known && relation.Target.PossibleWarGoals.Count > 0)
                    candidate_nations.Add(relation.Target);
            }

        }
    }
}
