using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using dawn_of_worlds.Creations.Diplomacy;
using dawn_of_worlds.WorldModel;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Effects;
using dawn_of_worlds.Creations.Civilisations;

namespace dawn_of_worlds.CelestialPowers.CommandNationPowers
{
    class WhitePeace : CommandNation
    {

        protected override void initialize()
        {
            base.initialize();
            Name = "White Peace (" + _white_peaced_war.Name + ")";
            Tags = new List<CreationTag>() { CreationTag.Peace, CreationTag.Diplomacy };
        }

        public override bool isObsolete
        {
            get
            {
                return _white_peaced_war.hasEnded;
            }
        }

        private War _white_peaced_war { get; set; }

        public override bool Precondition(Deity creator)
        {
            base.Precondition(creator);

            if (!_commanded_nation.hasDiplomacy)
                return false;

            return true;
        }

        public override int Effect(Deity creator)
        {
            // if the nation calling for white peace is a war leader the war ends.
            if (_white_peaced_war.isWarLeader(_commanded_nation))
            {
                // Remove war from war lists
                Program.State.OngoingWars.Remove(_white_peaced_war);

                // reset all the relations statuses of each nation.
                foreach (Civilisation defender in _white_peaced_war.Defenders)
                {
                    foreach (Civilisation attacker in _white_peaced_war.Attackers)
                    {
                        Relations temp = defender.Relationships.Find(x => x.Target == attacker);
                        if (temp != null)
                            temp.Status = RelationStatus.Known;
                        else
                            throw new Exception();
                    }
                }

                foreach (Civilisation attacker in _white_peaced_war.Attackers)
                {
                    foreach (Civilisation defender in _white_peaced_war.Defenders)
                    {
                        Relations temp = attacker.Relationships.Find(x => x.Target == defender);
                        if (temp != null)
                            temp.Status = RelationStatus.Known;
                        else
                            throw new Exception();
                    }
                }
                _white_peaced_war.hasEnded = true;
                _white_peaced_war.End = Simulation.Time.Shuffle;
            }
            else
            {
                // When a nation calling for white peace is not a war leader it is simply removed from the war.
                if (_white_peaced_war.isAttacker(_commanded_nation))
                {
                    _white_peaced_war.Attackers.Remove(_commanded_nation);

                    foreach (Civilisation defender in _white_peaced_war.Defenders)
                    {
                        defender.Relationships.Find(x => x.Target == _commanded_nation).Status = RelationStatus.Known;
                        _commanded_nation.Relationships.Find(x => x.Target == defender).Status = RelationStatus.Known;
                    }
                }
                else
                {
                    _white_peaced_war.Defenders.Remove(_commanded_nation);

                    foreach (Civilisation attacker in _white_peaced_war.Attackers)
                    {
                        attacker.Relationships.Find(x => x.Target == _commanded_nation).Status = RelationStatus.Known;
                        _commanded_nation.Relationships.Find(x => x.Target == attacker).Status = RelationStatus.Known;
                    }
                }
            }
            
            creator.LastCreation = _white_peaced_war;

            return 0;
        }

        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

            int army_count_attacker = 0;
            int army_count_defender = 0;

            foreach (Civilisation attacker in _white_peaced_war.Attackers)
            {
                for (int i = 0; i < attacker.Armies.Count; i++)
                {
                    if (!attacker.Armies[i].isScattered)
                        army_count_attacker += 1;
                }
            }
            foreach (Civilisation defender in _white_peaced_war.Defenders)
            {
                for (int i = 0; i < defender.Armies.Count; i++)
                {
                    if (!defender.Armies[i].isScattered)
                        army_count_defender += 1;
                }
            }

            // depending on the amount of armies on one side or the other the weight adjusted.
            if (_white_peaced_war.isAttacker(_commanded_nation))
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


        public WhitePeace(Civilisation commanded_nation, War white_peace_war) : base(commanded_nation)
        {
            _white_peaced_war = white_peace_war;
            initialize();
        }
    }
}
