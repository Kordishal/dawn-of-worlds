using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Diplomacy;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Creations.Organisations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.WorldClasses
{
    class World
    {
        public string Name { get; set; }

        public List<Deity> Deities { get; set; }
        public List<Race> Races { get; set; }
        public List<Nation> Nations { get; set; }
        public List<City> Cities { get; set; }
        public List<Order> Orders { get; set; }

        public List<War> OngoingWars { get; set; }

        public List<Region> WorldRegions { get; set; }

        public Area[,] AreaGrid { get; set; }

        public World(string world_name, int num_regions, int num_areas)
        {
            Races = new List<Race>();
            WorldRegions = new List<Region>();
            Deities = new List<Deity>();
            Nations = new List<Nation>();
            Cities = new List<City>();
            Orders = new List<Order>();
            OngoingWars = new List<War>();
            Name = world_name;

            generateWorldRegions(num_regions, num_areas);
            generateAreaGrid();
            generateAreaClimate();

            DefinedRaces.defineRaces();

            generateDeities();
        }

        private void generateWorldRegions(int num_regions, int num_areas)
        {
            for (int i = 0; i < num_regions; i++)
            {
                WorldRegions.Add(new Region(this, num_areas));
            }
        }
        private void generateAreaGrid()
        {
            int total_areas = 0;
            foreach (Region r in WorldRegions)
            {
                total_areas += r.RegionAreas.Count;
            }

            AreaGrid = new Area[Main.Constants.AREA_GRID_X, Main.Constants.AREA_GRID_Y];


            int x_length = AreaGrid.GetLength(0);
            int y_length = AreaGrid.GetLength(1);
            int x = 0, y = 0;
            bool has_valid_starter_coordinates;
            for (int i = 0; i < WorldRegions.Count; i++)
            {
                has_valid_starter_coordinates = false;
                while (!has_valid_starter_coordinates)
                {
                    x = Main.Constants.RND.Next(x_length);
                    y = Main.Constants.RND.Next(y_length);

                    if (AreaGrid[x, y] == null)
                    {
                        has_valid_starter_coordinates = true;
                    }
                }

                AreaGrid[x, y] = WorldRegions[i].RegionAreas[0];

                bool has_valid_neighbour = false;
                for (int j = 1; j < WorldRegions[i].RegionAreas.Count; j++)
                {
                    List<int> direction = new List<int>() { 0, 1, 2, 3 };
                    has_valid_neighbour = false;
                    while (!has_valid_neighbour)
                    {
                        switch (direction.Count > 0 ? direction[Main.Constants.RND.Next(direction.Count)] : 4)
                        {
                            case 0:
                                if (y + 1 < y_length)
                                {
                                    if (AreaGrid[x, y + 1] == null)
                                    {
                                        has_valid_neighbour = true;
                                        AreaGrid[x, y + 1] = WorldRegions[i].RegionAreas[j];
                                        y += 1;
                                    }
                                }
                                direction.Remove(0);
                                break;
                            case 1:
                                if (x + 1 < x_length)
                                {
                                    if (AreaGrid[x + 1, y] == null)
                                    {
                                        has_valid_neighbour = true;
                                        AreaGrid[x + 1, y] = WorldRegions[i].RegionAreas[j];
                                        x += 1;
                                    }
                                }
                                direction.Remove(1);
                                break;
                            case 2:
                                if (y - 1 >= 0)
                                {
                                    if (AreaGrid[x, y - 1] == null)
                                    {
                                        has_valid_neighbour = true;
                                        AreaGrid[x, y - 1] = WorldRegions[i].RegionAreas[j];
                                        y -= 1;
                                    }
                                }
                                direction.Remove(2);
                                break;
                            case 3:
                                if (x - 1 >= 0)
                                {
                                    if (AreaGrid[x - 1, y] == null)
                                    {
                                        has_valid_neighbour = true;
                                        AreaGrid[x - 1, y] = WorldRegions[i].RegionAreas[j];
                                        x -= 1;
                                    }
                                }
                                direction.Remove(3);
                                break;
                            default:
                                has_valid_starter_coordinates = false;
                                while (!has_valid_starter_coordinates)
                                {
                                    x = Main.Constants.RND.Next(x_length);
                                    y = Main.Constants.RND.Next(y_length);

                                    if (AreaGrid[x, y] == null)
                                    {
                                        has_valid_starter_coordinates = true;
                                    }
                                }

                                AreaGrid[x, y] = WorldRegions[i].RegionAreas[j];
                                has_valid_neighbour = true;
                                break;
                        }

                        if (x + 1 < x_length && AreaGrid[x + 1, y] != null)
                        {
                            AreaGrid[x, y].North = AreaGrid[x + 1, y];
                            AreaGrid[x + 1, y].South = AreaGrid[x, y];
                        }
                        if (x - 1 >= 0 && AreaGrid[x - 1, y] != null)
                        {
                            AreaGrid[x, y].South = AreaGrid[x - 1, y];
                            AreaGrid[x - 1, y].North = AreaGrid[x, y];
                        }
                        if (y + 1 < y_length && AreaGrid[x, y + 1] != null)
                        {
                            AreaGrid[x, y].East = AreaGrid[x, y + 1];
                            AreaGrid[x, y + 1].West = AreaGrid[x, y];
                        }
                        if (y - 1 >= 0 && AreaGrid[x, y - 1] != null)
                        {
                            AreaGrid[x, y].West = AreaGrid[x, y - 1];
                            AreaGrid[x, y - 1].East = AreaGrid[x, y];
                        }
                        if (x + 1 < x_length && y + 1 < y_length && AreaGrid[x + 1, y + 1] != null)
                        {
                            AreaGrid[x, y].NorthEast = AreaGrid[x + 1, y + 1];
                            AreaGrid[x + 1, y + 1].SouthWest = AreaGrid[x, y];
                        }
                        if (x - 1 >= 0 && y + 1 < y_length && AreaGrid[x - 1, y + 1] != null)
                        {
                            AreaGrid[x, y].SouthEast = AreaGrid[x - 1, y + 1];
                            AreaGrid[x - 1, y + 1].NorthWest = AreaGrid[x, y];
                        }
                        if (x - 1 >= 0 && y - 1 >= 0 && AreaGrid[x - 1, y - 1] != null)
                        {
                            AreaGrid[x, y].SouthWest = AreaGrid[x - 1, y - 1];
                            AreaGrid[x - 1, y - 1].NorthEast = AreaGrid[x, y];
                        }
                        if (x + 1 < x_length && y - 1 >= 0 && AreaGrid[x + 1, y - 1] != null)
                        {
                            AreaGrid[x, y].NorthWest = AreaGrid[x + 1, y - 1];
                            AreaGrid[x + 1, y - 1].SouthEast = AreaGrid[x, y];
                        }

                    }
                }
            }
        }
        private void generateAreaClimate()
        {
            int counter = 0;
            foreach (Area area in AreaGrid)
            {
                if (counter < 5)
                    area.AreaClimate = Climate.Arctic;
                else if (counter < 10)
                    area.AreaClimate = Climate.SubArctic;
                else if (counter < 15)
                    area.AreaClimate = Climate.Temperate;
                else if (counter < 20)
                    area.AreaClimate = Climate.SubTropical;
                else if (counter < 25)
                    area.AreaClimate = Climate.Tropical;

                counter += 1;
            }
        }
        private void generateDeities()
        {
            for (int i = 0; i < 5; i++)
            {
                Deities.Add(new Deity("Deity " + i, this));
            }


            int max_domains = Main.Constants.RND.Next(3, 6);
            int number_of_domains = Enum.GetValues(typeof(Domain)).Length;
            foreach (Deity deity in Deities)
            {
                for (int i = 0; i < max_domains; i++)
                {
                    Domain domain = (Domain)Enum.GetValues(typeof(Domain)).GetValue(Main.Constants.RND.Next(number_of_domains));
                    if (!deity.Domains.Contains(domain))                     
                        deity.Domains.Add(domain);
                }
            }
        }
    }
}
