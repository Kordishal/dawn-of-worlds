using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using dawn_of_worlds.Actors;
using dawn_of_worlds.CelestialPowers;
using dawn_of_worlds.Effects;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.CelestialPowers.ShapeClimatePowers;
using dawn_of_worlds.CelestialPowers.CreateRacePowers;
using dawn_of_worlds.WorldModel;
using dawn_of_worlds.CelestialPowers.ShapeLandPowers;

namespace dawn_of_worlds.Generators
{
    // TODO: Make deity generation more complex. Allow for more ways to influence and limit the generation. 
    // TODO: Move generation code from the deity base class into this class.
    // TODO: Document & Test deity generation

    class DeityGenerator
    {
        public Random rnd { get; set; }

        public int MinNumberDeities { get; set; }
        public int MaxNumberDeities { get; set; }

        public int MinNumberDomains { get; set; }
        public int MaxNumberDomains { get; set; }

        public List<Deity> GeneratedDeities { get; set; }

        public DeityGenerator(int seed, int min_nr_domains=2, int max_nr_domains=5, int min_nr_deities=4, int max_nr_deities=5)
        {
            rnd = new Random(seed);
            MinNumberDeities = min_nr_deities;
            MaxNumberDeities = max_nr_deities;

            MinNumberDomains = min_nr_domains;
            MaxNumberDomains = max_nr_domains;

            GeneratedDeities = new List<Deity>();
        }

        public void BasicGeneration()
        {
            for (int i = 0; i < rnd.Next(MinNumberDeities, MaxNumberDeities); i++)
            {
                var deity = new Deity();

                deity.Name = Program.GenerateNames.GetName("deity_names");

                deity.PowerPoints = 0;
                deity.Modifiers = new DeityModifiers();

                deity.Powers = new List<Power>();

                int nr_domains = rnd.Next(MinNumberDomains, MaxNumberDomains);
                deity.Domains = new Modifier[nr_domains];

                List<ModifierTag> domain_tags = new List<ModifierTag>();
                Array modifier_tags = Enum.GetValues(typeof(ModifierTag));

                for (int j = (int)ModifierTag.DomainsBegin + 1; j < (int)ModifierTag.DomainsEnd; j++)
                    domain_tags.Add((ModifierTag)modifier_tags.GetValue(j));

                for (int k = 0; k < nr_domains; k++)
                {
                    while (deity.Domains[k] == null)
                    {
                        bool is_valid_domain = true;
                        ModifierTag domain = domain_tags[Constants.Random.Next(domain_tags.Count)];

                        // Checks whether there is an incompatible domain and whether there is the same domain already in.
                        for (int l = 0; l < nr_domains; l++)
                            if (deity.Domains[l] != null && (deity.Domains[l].Excludes != null && 
                                deity.Domains[l].Excludes.Contains(domain) || deity.Domains[l].Tag == domain))
                                is_valid_domain = false;

                        if (is_valid_domain)
                            deity.Domains[k] = new Modifier(ModifierCategory.Domain, domain);
                    }
                }

                // Shape Land Powers
                deity.Powers.Add(new CreateForest());
                deity.Powers.Add(new CreateGrassland());
                deity.Powers.Add(new CreateDesert());
                deity.Powers.Add(new CreateCave());
                deity.Powers.Add(new CreateLake());
                deity.Powers.Add(new CreateRiver());
                deity.Powers.Add(new CreateMountainRange());
                deity.Powers.Add(new CreateMountain());
                deity.Powers.Add(new CreateHillRange());
                deity.Powers.Add(new CreateHill());
                // Shape Climate Powers
                deity.Powers.Add(new MakeClimateWarmer());
                deity.Powers.Add(new MakeClimateColder());
                deity.Powers.Add(new AddClimateModifier(ClimateModifier.MagicInfused));
                deity.Powers.Add(new CreateSpecialClimate(Climate.Inferno));


                // Create Races Powers
                foreach (var race in DefinedRaces.DefinedRacesList)
                {
                    foreach (var province in Program.State.ProvinceGrid)
                    {
                        deity.Powers.Add(new CreateRace(race, province));
                    }
                }



                GeneratedDeities.Add(deity);
            }
        }


    }
}
