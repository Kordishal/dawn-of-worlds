using dawn_of_worlds.CelestialPowers;
using dawn_of_worlds.CelestialPowers.CreateRacePowers;
using dawn_of_worlds.CelestialPowers.ShapeClimatePowers;
using dawn_of_worlds.CelestialPowers.ShapeLandPowers;
using dawn_of_worlds.Creations;
using dawn_of_worlds.Creations.Civilisations;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Main;
using dawn_of_worlds.Effects;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Actors
{
    /// <summary>
    /// The deities are the only actors in dawn of worlds. 
    /// Everything which happens is because of divine intervention. 
    /// 
    /// Deities affect their changes to the world with powers. Each powers costs power points.
    /// Each deity gets a random amount of power points per turn. 
    /// 
    /// The deities can use several powers per turn. 
    /// </summary>
    class Deity
    {

        /// <summary>
        /// Name of the deity. Randomly assigned. Used to name things created by this deity.
        /// </summary>
        public string Name { get; set; }

        // Resource to use powers.
        public int PowerPoints { get; set; }


        /// <summary>
        /// Each deity has a set amount of domains. They influence what powers the deity uses.
        /// </summary>
        public Modifier[] Domains { get; set; }
        private int _num_of_domains = 5;


        /// <summary>
        /// A list of all powers the deity can currently use.
        /// </summary>
        public List<Power> Powers { get; set; }

        /// <summary>
        /// Modifiers applied to power point generation each turn.
        /// </summary>
        public DeityModifiers Modifiers { get; set; }


        /// <summary>
        /// Stores all the terrain features created by this deity.
        /// </summary>
        public List<TerrainFeatures> TerrainFeatures { get; set; }
        public List<Race> CreatedRaces { get; set; }
        public List<Order> CreatedOrders { get; set; }
        public List<Avatar> CreatedAvatars { get; set; }
        public List<Civilisation> FoundedNations { get; set; }
        public List<City> FoundedCities { get; set; }


        /// <summary>
        /// The last used power of this deity. Currently not used.
        /// </summary>
        public Power LastUsedPower { get; set; }


        /// <summary>
        /// Last creation of the deity.
        /// </summary>
        public Creation LastCreation { get; set; }

        /// <summary>
        /// Creates a deity with a name, x domains and the powers useable in the beginning.
        /// </summary>
        /// <param name="name"></param>
        public Deity()
        {
            // takes a name from the deities name set.
            Name = Program.GenerateNames.GetName("deity_names");

            // starts with no power points and no modifiers.
            PowerPoints = 0;
            Modifiers = new DeityModifiers();

            Powers = new List<Power>();

            Domains = new Modifier[_num_of_domains];
  
            // choose x random domains without duplicates and no opposits.
            List<ModifierTag> domain_tags = new List<ModifierTag>();
            Array modifier_tags = Enum.GetValues(typeof(ModifierTag));
            for (int i = (int)ModifierTag.DomainsBegin + 1; i < (int)ModifierTag.DomainsEnd; i++)
                domain_tags.Add((ModifierTag)modifier_tags.GetValue(i));
            for (int i = 0; i < _num_of_domains; i++)
            {
                while (Domains[i] == null)
                {
                    bool is_valid_domain = true;
                    ModifierTag domain = domain_tags[Constants.Random.Next(domain_tags.Count)];

                    // Checks whether there is an incompatible domain and whether there is the same domain already in.
                    for (int j = 0; j < _num_of_domains; j++)
                        if (Domains[j] != null && (Domains[j].Excludes != null && Domains[j].Excludes.Contains(domain) || Domains[j].Tag == domain))
                            is_valid_domain = false;

                    if (is_valid_domain)
                        Domains[i] = new Modifier(ModifierCategory.Domain, domain);
                }
            }
            

            TerrainFeatures = new List<TerrainFeatures>();
            CreatedRaces = new List<Race>();
            CreatedOrders = new List<Order>();
            CreatedAvatars = new List<Avatar>();
            FoundedNations = new List<Civilisation>();
            FoundedCities = new List<City>();

            // Shape Land Powers
            Powers.Add(new CreateForest());
            Powers.Add(new CreateGrassland());
            Powers.Add(new CreateDesert());
            Powers.Add(new CreateCave());
            Powers.Add(new CreateLake());
            Powers.Add(new CreateRiver());
            Powers.Add(new CreateMountainRange());
            Powers.Add(new CreateMountain());
            Powers.Add(new CreateHillRange());
            Powers.Add(new CreateHill());
            // Shape Climate Powers
            Powers.Add(new MakeClimateWarmer());
            Powers.Add(new MakeClimateColder());
            Powers.Add(new AddClimateModifier(ClimateModifier.MagicInfused));
            Powers.Add(new CreateSpecialClimate(Climate.Inferno));


            // Create Races Powers
            foreach (Race race in DefinedRaces.DefinedRacesList)
            {
                foreach (Province province in Program.World.ProvinceGrid)
                {
                    Powers.Add(new CreateRace(race, province));
                }              
            }
        }


        private int _low_point_turn_bonus { get; set; }

        /// <summary>
        /// Add power points for a new turn.
        /// </summary>
        public void addPowerPoints()
        {
            int gain = 0;

            // Any deity with less than 5 points gets +1 point gain. This gain is cummulative to a max of +3
            // this is to encourage action.
            if (PowerPoints <= 5 && _low_point_turn_bonus < 3)
                 _low_point_turn_bonus += 1;
            else if (PowerPoints > 5)
                _low_point_turn_bonus = 0;

            gain += _low_point_turn_bonus;
            gain += Constants.Random.Next(Constants.DEITY_BASE_POWERPOINT_MIN_GAIN, Constants.DEITY_BASE_POWERPOINT_MAX_GAIN);
            gain += Modifiers.BonusPowerPoints;
            gain += (int)Math.Floor(gain * Modifiers.PowerPointModifier);

            PowerPoints = PowerPoints + gain;
        }

        /// <summary>
        /// What a deity does when they take a turn.
        /// </summary>
        public void Turn()
        {
            List<Power> current_powers = new List<Power>(Powers);
            List<Power> possible_powers = new List<Power>();

            // take actions as long as there are actions to be taken.
            do
            {
                possible_powers = new List<Power>();

                int total_weight = 0;

                foreach (Power p in current_powers)
                {
                    // no powers that are too expensive.
                    if (PowerPoints - p.Cost(this) >= 0)
                    {
                        // no powers where the precondition is not met.
                        if (p.Precondition(this))
                        {
                            possible_powers.Add(p);
                            total_weight += p.Weight(this);
                        }
                    }
                }

                // leave this loop when there are no possible powers.
                if (possible_powers.Count == 0)
                    break;

                int chance = Constants.Random.Next(total_weight);
                int prev_weight = 0, current_weight = 0;
                foreach (Power p in possible_powers)
                {
                    current_weight += p.Weight(this);
                    if (prev_weight <= chance && chance < current_weight)
                    {
                        int return_value = p.Effect(this);
                        if (return_value == 0)
                        {
                            PowerPoints = PowerPoints - p.Cost(this);
                            // For the Action Log entry.
                            _total_power_points_used += p.Cost(this);
                            LastUsedPower = p;
                            break;
                        }
                        else
                        {
                            break;
                        }

                    }


                    prev_weight += p.Weight(this);
                }

            } while (possible_powers.Count != 0);
            
        }

        /// <summary>
        /// A local property which stores the total number of power points used by the deity.
        /// </summary>
        private int _total_power_points_used { get; set; }

        /// <summary>
        /// Puts all the attributes of the deity into a string.
        /// </summary>
        /// <returns>Description of the deity.</returns>
        public string printDeity()
        {
            int counter = 0;
            string result = "";
            result += "Name: " + Name + "\n";
            result += "Domains: ";
            foreach (Modifier domain in Domains)
                result += domain.ToString() + ", ";
            result += "\n";
            result += "Total PowerPoints Used: " + _total_power_points_used + "\n";
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
            foreach (Civilisation nation in FoundedNations)
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

    /// <summary>
    /// Modifies the amount of power points the deity gets each turn. 
    /// </summary>
    public class DeityModifiers
    {
        public int BonusPowerPoints { get; set; }
        public double PowerPointModifier { get; set; }

        public DeityModifiers()
        {
            BonusPowerPoints = 0;
            PowerPointModifier = 1.0;
        }
    }

}
