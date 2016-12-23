using dawn_of_worlds.CelestialPowers;
using dawn_of_worlds.CelestialPowers.CreateRacePowers;
using dawn_of_worlds.CelestialPowers.ShapeClimatePowers;
using dawn_of_worlds.CelestialPowers.ShapeLandPowers;
using dawn_of_worlds.Creations;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Main;
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
        public string Name { get; set; }
        public int PowerPoints { get; set; }

        public Power LastUsedPower { get; set; }
        public List<Power> Powers { get; set; }

        public List<Domain> Domains { get; set; }

        public Creation LastCreation { get; set; }

        public List<TerrainFeatures> TerrainFeatures { get; set; }
        public List<Race> CreatedRaces { get; set; }
        public List<Order> CreatedOrders { get; set; }
        public List<Avatar> CreatedAvatars { get; set; }
        public List<Nation> FoundedNations { get; set; }
        public List<City> FoundedCities { get; set; }

        public List<string> ActionLog { get; set; }


        public Deity(string name, World current_world)
        {
            Name = name;
            PowerPoints = 0;
            Powers = new List<Power>();
            Domains = new List<Domain>();
            TerrainFeatures = new List<TerrainFeatures>();
            CreatedRaces = new List<Race>();
            CreatedOrders = new List<Order>();
            CreatedAvatars = new List<Avatar>();
            FoundedNations = new List<Nation>();
            FoundedCities = new List<City>();

            ActionLog = new List<string>();

            foreach (Area area in current_world.AreaGrid)
            {
                // Shape Land
                Powers.Add(new CreateForest(area));
                Powers.Add(new CreateGrassland(area));
                Powers.Add(new CreateDesert(area));
                Powers.Add(new CreateCave(area));
                Powers.Add(new CreateLake(area));
                Powers.Add(new CreateRiver(area));
                Powers.Add(new CreateMountainRange(area));
                Powers.Add(new CreateMountain(area));
                Powers.Add(new CreateHillRange(area));
                Powers.Add(new CreateHill(area));
                // Shape Climate
                Powers.Add(new MakeClimateWarmer(area));
                Powers.Add(new MakeClimateColder(area));
                Powers.Add(new AddClimateModifier(area, ClimateModifier.MagicInfused));
                Powers.Add(new CreateSpecialClimate(area, Climate.Inferno));
            }

            // Create Races
            foreach (Race race in DefinedRaces.DefinedRacesList)
            {
                foreach (Province province in current_world.TileGrid)
                {
                    Powers.Add(new CreateRace(race, province));
                }              
            }
        }


        public void AddPowerPoints()
        {
            //Console.WriteLine("PowerPoints before new Turn: " + PowerPoints);

            if (PowerPoints < 5)
                PowerPoints = PowerPoints + (5 - PowerPoints);

            PowerPoints = PowerPoints + Constants.Random.Next(Constants.DEITY_BASE_POWERPOINT_MIN_GAIN, Constants.DEITY_BASE_POWERPOINT_MAX_GAIN);

            //Console.WriteLine("PowerPoints after adding turn gain: " + PowerPoints);
        }

        public void Turn()
        {
            List<Power> current_powers = new List<Power>(Powers);
            List<Power> possible_powers = new List<Power>();
            int total_weight = 0;

            foreach (Power p in current_powers)
            {
                if (PowerPoints - p.Cost() >= 0)
                {
                    if (p.Precondition(this))
                    {
                        possible_powers.Add(p);
                        total_weight += p.Weight(this);
                    }
                }                    
            }

            //Console.WriteLine("Possible Actions Count: " + possible_powers.Count);

            int chance = Constants.Random.Next(total_weight);
            int prev_weight = 0, current_weight = 0;
            foreach (Power p in possible_powers)
            {
                current_weight += p.Weight(this);
                if (prev_weight <= chance && chance < current_weight)
                {
                    //Console.WriteLine("TAKE ACTION");
                    //Console.WriteLine("Action: " + p);
                    //Console.WriteLine("Cost: " + p.Cost(current_age));
                    //Console.WriteLine("PowerPoints: " + PowerPoints);
                    p.Effect(this);
                    PowerPoints = PowerPoints - p.Cost();
                    // For the Action Log entry.
                    _total_power_points += p.Cost();
                    LastUsedPower = p;
                    break;
                }


                prev_weight += p.Weight(this);
            }
        }

        private int _total_power_points;

        public string printDeity()
        {
            int counter = 0;
            string result = "";
            result += "Name: " + Name + "\n";
            result += "Domains: ";
            foreach (Domain domain in Domains)
                result += domain.ToString() + ", ";
            result += "\n";
            result += "Total PowerPoints Used: " + _total_power_points + "\n";
            result += "CreationsCount: " + TerrainFeatures.Count.ToString() + "\n";
            result += "Creations: \n";
            foreach (Creation creation in TerrainFeatures)
            {
                result += creation.Name;
                counter++;
                if (counter % 10 == 0)
                    result += "\n";
                else
                    result += ", ";
            }
            counter = 0;
            result += "\n\n";
            result += "RacesCount: " + CreatedRaces.Count.ToString() + "\n";
            result += "Races: \n";
            foreach (Race race in CreatedRaces)
            {
                result += race.Name;
                counter++;
                if (counter % 10 == 0)
                    result += "\n";
                else
                    result += ", ";
            }
            counter = 0;
            result += "\n\n";
            result += "OrdersCount: " + CreatedOrders.Count.ToString() + "\n";
            result += "Orders: \n";
            foreach (Order order in CreatedOrders)
            {
                result += order.Name;
                counter++;
                if (counter % 10 == 0)
                    result += "\n";
                else
                    result += ", ";
            }
            counter = 0;
            result += "\n\n";
            result += "NationsCount: " + FoundedNations.Count.ToString() + "\n";
            result += "Nations: \n";
            foreach (Nation nation in FoundedNations)
            {
                result += nation.Name;
                counter++;
                if (counter % 10 == 0)
                    result += "\n";
                else
                    result += ", ";
            }
            counter = 0;
            result += "\n\n";
            result += "CitiesCount: " + FoundedCities.Count.ToString() + "\n";
            result += "Cities: \n";
            foreach (City city in FoundedCities)
            {
                result += city.Name;
                counter++;
                if (counter % 10 == 0)
                    result += "\n";
                else
                    result += ", ";
            }
            counter = 0;
            result += "\n\n";
            result += "AvatarsCount: " + CreatedAvatars.Count.ToString() + "\n";
            result += "Avatars: \n";
            foreach (Avatar avatar in CreatedAvatars)
            {
                result += avatar.Name;
                counter++;
                if (counter % 10 == 0)
                    result += "\n";
                else
                    result += ", ";
            }
            counter = 0;
            result += "\n\n";
            return result;
        }
    }
}
