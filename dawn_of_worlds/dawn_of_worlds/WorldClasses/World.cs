using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Diplomacy;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Main;
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
        public Terrain[,] TerrainGrid { get; set; }

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

            defineAreaAndTerrainCoordiantes();

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

            AreaGrid = new Area[Constants.AREA_GRID_X, Constants.AREA_GRID_Y];


            int x_length = AreaGrid.GetLength(0);
            int y_length = AreaGrid.GetLength(1);
            int x = 0, y = 0;
            bool has_valid_starter_coordinates;
            for (int i = 0; i < WorldRegions.Count; i++)
            {
                has_valid_starter_coordinates = false;
                while (!has_valid_starter_coordinates)
                {
                    x = Constants.RND.Next(x_length);
                    y = Constants.RND.Next(y_length);

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
                        switch (direction.Count > 0 ? direction[Constants.RND.Next(direction.Count)] : 4)
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
                                    x = Constants.RND.Next(x_length);
                                    y = Constants.RND.Next(y_length);

                                    if (AreaGrid[x, y] == null)
                                    {
                                        has_valid_starter_coordinates = true;
                                    }
                                }

                                AreaGrid[x, y] = WorldRegions[i].RegionAreas[j];
                                has_valid_neighbour = true;
                                break;
                        }
                    }
                }
            }
        }
        private void defineAreaAndTerrainCoordiantes()
        {
            TerrainGrid = new Terrain[Constants.TERRAIN_GRID_X, Constants.TERRAIN_GRID_Y];

            for (int i = 0; i < Constants.AREA_GRID_X; i++)
            {
                for (int j = 0; j < Constants.AREA_GRID_Y; j++)
                {
                    AreaGrid[i, j].Coordinates = new SystemCoordinates(i, j);
                    for (int k = i * 5; k < i * 5 + Constants.AREA_GRID_X; k++)
                    {
                        for (int l = j * 5; l < j * 5 + Constants.AREA_GRID_Y; l++)
                        {
                            TerrainGrid[k, l] = new Terrain(AreaGrid[i, j]);        
                            TerrainGrid[k, l].Coordinates = new SystemCoordinates(k, l);
                            AreaGrid[i, j].TerrainArea.Add(TerrainGrid[k, l]);
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
                    area.ClimateArea = Climate.Arctic;
                else if (counter < 10)
                    area.ClimateArea = Climate.SubArctic;
                else if (counter < 15)
                    area.ClimateArea = Climate.Temperate;
                else if (counter < 20)
                    area.ClimateArea = Climate.SubTropical;
                else if (counter < 25)
                    area.ClimateArea = Climate.Tropical;

                counter += 1;
            }
        }
        private void generateDeities()
        {
            for (int i = 0; i < 5; i++)
            {
                Deities.Add(new Deity(Constants.Names.GetName("deities"), this));
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
