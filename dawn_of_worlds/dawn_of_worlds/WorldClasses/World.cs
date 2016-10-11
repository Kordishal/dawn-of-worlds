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

        public List<Region> WorldRegions { get; set; }

        public Area[,] AreaGrid { get; set; }

        public World(string world_name, int min_regions, int max_regions, int min_areas, int max_areas)
        {
            WorldRegions = new List<Region>();
            Name = world_name;

            for (int i = 0; i < Main.MainLoop.RND.Next(min_regions, max_regions); i++)
            {
                WorldRegions.Add(new Region(min_areas, max_areas));
            }

            int total_areas = 0;
            foreach (Region r in WorldRegions)
            {
                total_areas += r.RegionAreas.Count;
            }

            AreaGrid = new Area[total_areas / 2, total_areas - (total_areas / 2)];


            int x_length = AreaGrid.GetLength(0);
            int y_length = AreaGrid.GetLength(1);
            int x = 0, y = 0;
            bool has_valid_starter_coordinates;
            for (int i = 0; i < WorldRegions.Count; i++)
            {
                has_valid_starter_coordinates = false;
                while (!has_valid_starter_coordinates)
                {
                    x = Main.MainLoop.RND.Next(x_length);
                    y = Main.MainLoop.RND.Next(y_length);

                    if (AreaGrid[x, y] == null)
                    {
                        has_valid_starter_coordinates = true;
                    }
                }

                AreaGrid[x, y] = WorldRegions[i].RegionAreas[0];

                bool has_valid_neighbour = false;
                for (int j = 1; j < WorldRegions[i].RegionAreas.Count; j++)
                {
                    List<int> direction = new List<int>() { 0, 1, 2, 3};
                    while (!has_valid_neighbour)
                    {
                        switch (direction[Main.MainLoop.RND.Next(direction.Count)])
                        {
                            case 0:
                                if (y + 1 < y_length)
                                {
                                    if (AreaGrid[x, y].North == null)
                                    {
                                        has_valid_neighbour = true;
                                        AreaGrid[x, y].North = WorldRegions[i].RegionAreas[j];
                                        AreaGrid[x, y + 1] = WorldRegions[i].RegionAreas[j];
                                        WorldRegions[i].RegionAreas[j].South = AreaGrid[x, y];
                                        y += 1;
                                    }
                                }
                                direction.Remove(0);
                                break;
                            case 1:
                                if (x + 1 < x_length)
                                {
                                    if (AreaGrid[x, y].East == null)
                                    {
                                        has_valid_neighbour = true;
                                        AreaGrid[x, y].East = WorldRegions[i].RegionAreas[j];
                                        AreaGrid[x + 1, y] = WorldRegions[i].RegionAreas[j];
                                        WorldRegions[i].RegionAreas[j].West = AreaGrid[x, y];
                                        x += 1;
                                    }
                                }
                                direction.Remove(1);
                                break;
                            case 2:
                                if (y - 1 > 0)
                                {
                                    if (AreaGrid[x, y].South == null)
                                    {
                                        has_valid_neighbour = true;
                                        AreaGrid[x, y].South = WorldRegions[i].RegionAreas[j];
                                        AreaGrid[x, y - 1] = WorldRegions[i].RegionAreas[j];
                                        WorldRegions[i].RegionAreas[j].North = AreaGrid[x, y];
                                        y -= 1;
                                    }
                                }
                                direction.Remove(2);
                                break;
                            case 3:
                                if (x - 1 > 0)
                                {
                                    if (AreaGrid[x, y].West == null)
                                    {
                                        has_valid_neighbour = true;
                                        AreaGrid[x, y].West = WorldRegions[i].RegionAreas[j];
                                        AreaGrid[x - 1, y] = WorldRegions[i].RegionAreas[j];
                                        WorldRegions[i].RegionAreas[j].East = AreaGrid[x, y];
                                        x -= 1;
                                    }
                                }
                                direction.Remove(3);
                                break;
                            default:
                                has_valid_starter_coordinates = false;
                                while (!has_valid_starter_coordinates)
                                {
                                    x = Main.MainLoop.RND.Next(x_length);
                                    y = Main.MainLoop.RND.Next(y_length);

                                    if (AreaGrid[x, y] == null)
                                    {
                                        has_valid_starter_coordinates = true;
                                    }
                                }
                                break;
                        }
                    }
                
                }
            }
        }
    }
}
