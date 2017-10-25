using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Civilisations;
using dawn_of_worlds.Creations.Diplomacy;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Main;
using dawn_of_worlds.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace dawn_of_worlds.WorldClasses
{

    // TODO: Extract world generation from model class into generator class.
    // TODO: Simplyfy world generation/change algorithm.
    // TODO: Improve world naming.
    [Serializable]
    class World
    {
        public string Name { get; set; }

        public List<Region> Regions { get; set; }

        public World(string world_name)
        {
            Name = world_name;
        }

        public void initialize(int num_regions, int num_areas)
        {
            Regions = new List<Region>();

            generateWorldRegions(num_regions, num_areas);
            generateAreaGrid();
            defineAreaAndTerrainCoordiantes();

            defineContinentsAndOceans();

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (Program.State.AreaGrid[i, j].Type == AreaType.Continent)
                        Console.Write("C ");
                    else
                        Console.Write("O ");
                }
                Console.WriteLine();
            }


            DefinedRaces.defineRaces();

            generateDeities();
        }

        private void generateWorldRegions(int num_regions, int num_areas)
        {
            for (int i = 0; i < num_regions; i++)
            {
                Regions.Add(new Region(num_areas));
            }

            Regions[0].Type = RegionType.Continent;
            Regions[1].Type = RegionType.Continent;
            Regions[2].Type = RegionType.Ocean;
            Regions[3].Type = RegionType.Continent;
            Regions[4].Type = RegionType.Ocean;

            foreach (Region region in Regions)
            {
                foreach (Area area in region.Areas)
                {
                    if (region.Type == RegionType.Continent)
                        area.Type = AreaType.Continent;
                    else
                        area.Type = AreaType.Ocean;
                }
            }

        }
        private void generateAreaGrid()
        {
            int total_areas = 0;
            foreach (Region r in Regions)
            {
                total_areas += r.Areas.Count;
            }

            Program.State.AreaGrid = new Area[Constants.AREA_GRID_X, Constants.AREA_GRID_Y];


            int x_length = Program.State.AreaGrid.GetLength(0);
            int y_length = Program.State.AreaGrid.GetLength(1);
            int x = 0, y = 0;
            bool has_valid_starter_coordinates;
            for (int i = 0; i < Regions.Count; i++)
            {
                has_valid_starter_coordinates = false;
                while (!has_valid_starter_coordinates)
                {
                    x = Constants.Random.Next(x_length);
                    y = Constants.Random.Next(y_length);

                    if (Program.State.AreaGrid[x, y] == null)
                    {
                        has_valid_starter_coordinates = true;
                    }
                }

                Program.State.AreaGrid[x, y] = Regions[i].Areas[0];

                bool has_valid_neighbour = false;
                for (int j = 1; j < Regions[i].Areas.Count; j++)
                {
                    List<int> direction = new List<int>() { 0, 1, 2, 3 };
                    has_valid_neighbour = false;
                    while (!has_valid_neighbour)
                    {
                        switch (direction.Count > 0 ? direction[Constants.Random.Next(direction.Count)] : 4)
                        {
                            case 0:
                                if (y + 1 < y_length)
                                {
                                    if (Program.State.AreaGrid[x, y + 1] == null)
                                    {
                                        has_valid_neighbour = true;
                                        Program.State.AreaGrid[x, y + 1] = Regions[i].Areas[j];
                                        y += 1;
                                    }
                                }
                                direction.Remove(0);
                                break;
                            case 1:
                                if (x + 1 < x_length)
                                {
                                    if (Program.State.AreaGrid[x + 1, y] == null)
                                    {
                                        has_valid_neighbour = true;
                                        Program.State.AreaGrid[x + 1, y] = Regions[i].Areas[j];
                                        x += 1;
                                    }
                                }
                                direction.Remove(1);
                                break;
                            case 2:
                                if (y - 1 >= 0)
                                {
                                    if (Program.State.AreaGrid[x, y - 1] == null)
                                    {
                                        has_valid_neighbour = true;
                                        Program.State.AreaGrid[x, y - 1] = Regions[i].Areas[j];
                                        y -= 1;
                                    }
                                }
                                direction.Remove(2);
                                break;
                            case 3:
                                if (x - 1 >= 0)
                                {
                                    if (Program.State.AreaGrid[x - 1, y] == null)
                                    {
                                        has_valid_neighbour = true;
                                        Program.State.AreaGrid[x - 1, y] = Regions[i].Areas[j];
                                        x -= 1;
                                    }
                                }
                                direction.Remove(3);
                                break;
                            default:
                                has_valid_starter_coordinates = false;
                                while (!has_valid_starter_coordinates)
                                {
                                    x = Constants.Random.Next(x_length);
                                    y = Constants.Random.Next(y_length);

                                    if (Program.State.AreaGrid[x, y] == null)
                                    {
                                        has_valid_starter_coordinates = true;
                                    }
                                }

                                Program.State.AreaGrid[x, y] = Regions[i].Areas[j];
                                has_valid_neighbour = true;
                                break;
                        }
                    }
                }
            }
        }
        private void defineAreaAndTerrainCoordiantes()
        {
            Program.State.ProvinceGrid = new Province[Constants.TILE_GRID_X, Constants.TILE_GRID_Y];

            for (int i = 0; i < Constants.AREA_GRID_X; i++)
            {
                for (int j = 0; j < Constants.AREA_GRID_Y; j++)
                {
                    Program.State.AreaGrid[i, j].Coordinates = new SystemCoordinates(i, j);
                    for (int k = i * 5; k < i * 5 + Constants.AREA_GRID_X; k++)
                    {
                        for (int l = j * 5; l < j * 5 + Constants.AREA_GRID_Y; l++)
                        {
                            Program.State.ProvinceGrid[k, l] = new Province(Program.State.AreaGrid[i, j], new SystemCoordinates(k, l));
                            Program.State.ProvinceGrid[k, l].initialize();
                            Program.State.AreaGrid[i, j].Provinces.Add(Program.State.ProvinceGrid[k, l]);
                        }
                    }
                }
            }

        }

        private void defineContinentsAndOceans()
        {
            Regions = new List<Region>();
            foreach (Area area in Program.State.AreaGrid)
                area.Region = null;

            bool has_undefined_area = true;
            Region next_region = null;
            Area next_area = null;

            while (has_undefined_area)
            {
                next_region = null;
                next_area = null;
                
                while (next_area == null)
                {
                    next_area = Program.State.AreaGrid[Constants.Random.Next(Constants.AREA_GRID_X), Constants.Random.Next(Constants.AREA_GRID_Y)];

                    if (next_area.Region != null)
                        next_area = null;
                }

                if (next_area.Provinces[0].Type == TerrainType.Ocean)
                    next_region = new Region(this, RegionType.Ocean);
                else
                    next_region = new Region(this, RegionType.Continent);

                addArea(next_region, next_area);
                Regions.Add(next_region);
                
                bool area_is_not_assigned = false;
                foreach (Area area in Program.State.AreaGrid)
                    if (area.Region == null)
                        area_is_not_assigned = true;

                if (!area_is_not_assigned)
                    has_undefined_area = false;
            }
        }

        private void addArea(Region region, Area area)
        {
            region.Areas.Add(area);
            area.Region = region;

            for (int i = 0; i < 7; i += 2)
            {
                SystemCoordinates coords = area.Coordinates.GetNeighbour(i);

                if (coords.isInAreaGridBounds())
                {
                    Area next_area = Program.State.getArea(coords);
                    if (next_area.Type == AreaType.Continent && region.Type == RegionType.Continent || 
                            next_area.Type == AreaType.Ocean && region.Type == RegionType.Ocean)
                        if (next_area.Region == null)
                            addArea(region, next_area);                    
                }
            }
        }

        private void generateDeities()
        {
            for (int i = 0; i < 5; i++)
            {
                Program.State.Deities.Add(new Deity());
            }
        }
    }
}
