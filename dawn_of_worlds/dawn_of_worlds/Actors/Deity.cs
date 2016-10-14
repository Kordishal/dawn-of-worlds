using dawn_of_worlds.CelestialPowers;
using dawn_of_worlds.CelestialPowers.RaceCreationPowers;
using dawn_of_worlds.CelestialPowers.RaceCreationPowers.SubRaceCreationPowers;
using dawn_of_worlds.CelestialPowers.ShapeClimatePowers;
using dawn_of_worlds.CelestialPowers.ShapeLandPowers;
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

            // Shape Land
            Powers.Add(new CreateForest());
            Powers.Add(new CreateLake());
            Powers.Add(new CreateRiver());
            Powers.Add(new CreateMountainRange());
            Powers.Add(new CreateMountain());

            // Shape Climate
            Powers.Add(new IncreaseTemperature());
            Powers.Add(new DecreaseTemperature());
            Powers.Add(new IncreaseHumidity());
            Powers.Add(new DecreaseHumidity());

            // Create Races
            Powers.Add(new CreateHumans());
            Powers.Add(new CreateElves());
            Powers.Add(new CreateDwarves());
            Powers.Add(new CreateDragons());
            Powers.Add(new CreateGiants());

            // Create Subraces
            Powers.Add(new CreateColdHumans());
            Powers.Add(new CreateHillGiants());
            Powers.Add(new CreateFireDragons());
            Powers.Add(new CreateDeepDwarves());
            Powers.Add(new CreateDarkElves());
        }


        public void AddPower()
        {
            Console.WriteLine("PowerPoints before new Turn: " + PowerPoints);

            if (PowerPoints < 5)
                PowerPoints = PowerPoints + (5 - PowerPoints);

            PowerPoints = PowerPoints + Main.MainLoop.RND.Next(12);

            Console.WriteLine("PowerPoints after adding turn gain: " + PowerPoints);
        }

        public void Turn(World current_world, int current_age)
        {
            List<Power> possible_powers = new List<Power>();
            int total_weight = 0;

            foreach (Power p in Powers)
            {
                if (PowerPoints - p.Cost(current_age) >= 0)
                {
                    if (p.Precondition(current_world, this, current_age))
                    {
                        possible_powers.Add(p);
                        total_weight += p.Weight(current_world, this, current_age);
                    }
                }                    
            }

            Console.WriteLine("Possible Actions Count: " + possible_powers.Count);

            int chance = Main.MainLoop.RND.Next(total_weight);
            int prev_weight = 0, current_weight = 0;
            foreach (Power p in possible_powers)
            {
                current_weight += p.Weight(current_world, this, current_age);
                if (prev_weight <= chance && chance < current_weight)
                {
                    Console.WriteLine("TAKE ACTION");
                    Console.WriteLine("Action: " + p);
                    Console.WriteLine("Cost: " + p.Cost(current_age));
                    Console.WriteLine("PowerPoints: " + PowerPoints);
                    p.Effect(current_world, this, current_age);
                    PowerPoints = PowerPoints - p.Cost(current_age);
                    break;
                }

                prev_weight += p.Weight(current_world, this, current_age);
            }
        }
    }
}
