using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using dawn_of_worlds.Actors;

namespace dawn_of_worlds.Generators
{
    class DeityGenerator
    {
        public Random rnd { get; set; }

        public int MinNumberDeities { get; set; }
        public int MaxNumberDeities { get; set; }

        public DeityGenerator(int seed, int min_number_of_deities=4, int max_number_of_deities=5)
        {
            rnd = new Random(seed);
            MinNumberDeities = min_number_of_deities;
            MaxNumberDeities = max_number_of_deities;
        }

        public void BasicGeneration()
        {
            for (int i = 0; i < rnd.Next(MinNumberDeities, MaxNumberDeities); i++)
                Program.State.Deities.Add(new Deity());
        }


    }
}
