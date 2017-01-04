using dawn_of_worlds.Actors;
using dawn_of_worlds.Main;
using dawn_of_worlds.Modifiers;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.CelestialPowers
{
    abstract class Power
    {
        public string Name { get; set; }

        protected List<CreationTag> Tags { get; set; }
        protected int[] BaseCost { get; set; }
        protected int CostChange { get; set; }

        protected int[] BaseWeight { get; set; }
        protected int WeightChange { get; set; }

        abstract protected void initialize();

        virtual public bool isObsolete { get { return false; } }

        virtual public bool Precondition(Deity creator)
        {
            foreach (Modifier domain in creator.Domains)
            {
                if (domain.Forbids != null)
                    for (int i = 0; i < domain.Forbids.Length; i++)
                        if (Tags.Contains(domain.Forbids[i]))
                            return false;
            }
            // if the power has become obsolete it should no longer be used and be removed at the end of the turn.
            if (isObsolete)
                return false;

            return true;
        }
        virtual public int Cost(Deity creator)
        {
            int cost = 0;
            switch (Simulation.Time.CurrentAge)
            {
                case Age.Creation:
                    cost += BaseCost[0];
                    break;
                case Age.Races:
                    cost += BaseCost[1];
                    break;
                case Age.Relations:
                    cost += BaseCost[2];
                    break;
            }

            // Domain cost reductions & increases
            foreach (Modifier domain in creator.Domains)
            {
                if (domain.IncreaseCost != null)
                    for (int i = 0; i < domain.IncreaseCost.Length; i++)
                        if (Tags.Contains(domain.IncreaseCost[i]))
                            cost += CostChange;
                if (domain.DecreaseCost != null)
                    for (int j = 0; j < domain.DecreaseCost.Length; j++)
                        if (Tags.Contains(domain.DecreaseCost[j]))
                            cost -= CostChange;
            }
               
            return cost >= 0 ? cost : 0;
        }
        virtual public int Weight(Deity creator)
        {
            int weight = 0;
            switch (Simulation.Time.CurrentAge)
            {
                case Age.Creation:
                    weight += BaseWeight[0];
                    break;
                case Age.Races:
                    weight += BaseWeight[1];
                    break;
                case Age.Relations:
                    weight += BaseWeight[2];
                    break;
                default:
                    weight += 0;
                    break;
            }

            // Domain cost reductions & increases
            foreach (Modifier domain in creator.Domains)
            {
                if (domain.IncreasesWeight != null)
                    for (int i = 0; i < domain.IncreasesWeight.Length; i++)
                        if (Tags.Contains(domain.IncreasesWeight[i]))
                            weight += WeightChange;
                if (domain.DecreasesWeight != null)
                    for (int j = 0; j < domain.DecreasesWeight.Length; j++)
                        if (Tags.Contains(domain.DecreasesWeight[j]))
                            weight -= WeightChange;
            }

            return weight >= 0 ? weight : 0;
        }

        abstract public void Effect(Deity creator);

        public Power()
        {
            Tags = new List<CreationTag>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
