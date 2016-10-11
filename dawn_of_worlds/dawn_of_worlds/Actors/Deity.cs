using dawn_of_worlds.CelestialPowers;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Actors
{
    class Deity
    {

        public int PowerPoints { get; set; }

        public List<Power> Powers { get; set; }

        public Deity()
        {
            PowerPoints = 0;
            Powers = new List<Power>();
        }

        public void Turn(World current_world, int current_age)
        {
            if (PowerPoints < 5)
                PowerPoints += PowerPoints + (5 - PowerPoints);

            PowerPoints += PowerPoints + Main.MainLoop.RND.Next(12);

            List<Power> possible_powers = new List<Power>();
            int total_weight = 0;

            foreach (Power p in Powers)
            {
                if (p.Cost(current_age) < PowerPoints)
                {
                    if (p.Precondition(current_world, this, current_age))
                    {
                        possible_powers.Add(p);
                        total_weight += p.Weight(current_world, this, current_age);
                    }
                }                    
            }

            int chance = Main.MainLoop.RND.Next(total_weight);
            int prev_weight = 0, current_weight = 0;
            foreach (Power p in possible_powers)
            {
                current_weight = p.Weight(current_world, this, current_age);
                if (prev_weight <= chance && chance <= current_weight)
                {
                    p.Effect(current_world, this, current_age);
                    continue;
                }

                prev_weight = current_weight;
            }
        }
    }
}
