using dawn_of_worlds.CelestialPowers;
using dawn_of_worlds.CelestialPowers.CreateRacePowers;
using dawn_of_worlds.CelestialPowers.ShapeClimatePowers;
using dawn_of_worlds.CelestialPowers.ShapeLandPowers;
using dawn_of_worlds.Creations;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Creations.Organisations;
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

        public List<Creation> Creations { get; set; }
        public List<Race> CreatedRaces { get; set; }
        public List<Order> CreatedOrders { get; set; }
        public List<Nation> FoundedNations { get; set; }
        public List<City> FoundedCities { get; set; }

        public List<string> ActionLog { get; set; }


        public Deity()
        {
            PowerPoints = 0;
            Powers = new List<Power>();
            Creations = new List<Creation>();
            CreatedRaces = new List<Race>();
            CreatedOrders = new List<Order>();
            FoundedNations = new List<Nation>();
            FoundedCities = new List<City>();

            ActionLog = new List<string>();

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

            Race Humans = new Race("Human", this);
            Race Elves = new Race("Elves", this);
            Race Giants = new Race("Giants", this);
            Race Dragons = new Race("Dragons", this);
            Race Dwarves = new Race("Dwarves", this);

            // Create Races
            Powers.Add(new CreateRace(Humans));
            Powers.Add(new CreateRace(Elves));
            Powers.Add(new CreateRace(Giants));
            Powers.Add(new CreateRace(Dragons));
            Powers.Add(new CreateRace(Dwarves));

            Race ColdHumans = new Race("Cold Humans", this);
            Humans.PossibleSubRaces.Add(ColdHumans);

            Race FireDragons = new Race("Fire Dragons", this);
            Dragons.PossibleSubRaces.Add(FireDragons);

            Race DarkElves = new Race("Dark Elves", this);
            Elves.PossibleSubRaces.Add(DarkElves);

            Race DeepDwarves = new Race("Deep Dwarves", this);
            Dwarves.PossibleSubRaces.Add(DeepDwarves);

            Race HillGiants = new Race("Hill Giants", this);
            Giants.PossibleSubRaces.Add(HillGiants);
        }


        public void AddPowerPoints()
        {
            Console.WriteLine("PowerPoints before new Turn: " + PowerPoints);

            if (PowerPoints < 5)
                PowerPoints = PowerPoints + (5 - PowerPoints);

            PowerPoints = PowerPoints + Main.MainLoop.RND.Next(12);

            Console.WriteLine("PowerPoints after adding turn gain: " + PowerPoints);
        }

        public void Turn(World current_world, int current_age)
        {
            List<Power> current_powers = new List<Power>(Powers);
            List<Power> possible_powers = new List<Power>();
            int total_weight = 0;

            foreach (Power p in current_powers)
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
