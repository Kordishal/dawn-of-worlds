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

namespace dawn_of_worlds.WorldClasses
{
    class World
    {
        public string Name { get; set; }

        public List<Region> Regions { get; set; }

        public Area[,] AreaGrid { get; set; }
        public Area getArea(SystemCoordinates coords) { return AreaGrid[coords.X, coords.Y]; }
        public Province[,] ProvinceGrid { get; set; }
        public Province getProvince(SystemCoordinates coords) { return ProvinceGrid[coords.X, coords.Y]; }

        public List<Deity> Deities { get; set; }
        public List<Race> Races { get; set; }
        public List<Civilisation> Nations { get; set; }
        public List<City> Cities { get; set; }
        public List<Order> Orders { get; set; }

        public List<War> OngoingWars { get; set; }

        public World(string world_name)
        {
            Races = new List<Race>();
            Regions = new List<Region>();
            Deities = new List<Deity>();
            Nations = new List<Civilisation>();
            Cities = new List<City>();
            Orders = new List<Order>();
            OngoingWars = new List<War>();
            Name = world_name;
        }

        public void initialize(int num_regions, int num_areas)
        {
            generateWorldRegions(num_regions, num_areas);
            generateAreaGrid();
            defineAreaAndTerrainCoordiantes();

            defineContinentsAndOceans();

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (AreaGrid[i, j].Type == AreaType.Continent)
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

            AreaGrid = new Area[Constants.AREA_GRID_X, Constants.AREA_GRID_Y];


            int x_length = AreaGrid.GetLength(0);
            int y_length = AreaGrid.GetLength(1);
            int x = 0, y = 0;
            bool has_valid_starter_coordinates;
            for (int i = 0; i < Regions.Count; i++)
            {
                has_valid_starter_coordinates = false;
                while (!has_valid_starter_coordinates)
                {
                    x = Constants.Random.Next(x_length);
                    y = Constants.Random.Next(y_length);

                    if (AreaGrid[x, y] == null)
                    {
                        has_valid_starter_coordinates = true;
                    }
                }

                AreaGrid[x, y] = Regions[i].Areas[0];

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
                                    if (AreaGrid[x, y + 1] == null)
                                    {
                                        has_valid_neighbour = true;
                                        AreaGrid[x, y + 1] = Regions[i].Areas[j];
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
                                        AreaGrid[x + 1, y] = Regions[i].Areas[j];
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
                                        AreaGrid[x, y - 1] = Regions[i].Areas[j];
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
                                        AreaGrid[x - 1, y] = Regions[i].Areas[j];
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

                                    if (AreaGrid[x, y] == null)
                                    {
                                        has_valid_starter_coordinates = true;
                                    }
                                }

                                AreaGrid[x, y] = Regions[i].Areas[j];
                                has_valid_neighbour = true;
                                break;
                        }
                    }
                }
            }
        }
        private void defineAreaAndTerrainCoordiantes()
        {
            ProvinceGrid = new Province[Constants.TILE_GRID_X, Constants.TILE_GRID_Y];

            for (int i = 0; i < Constants.AREA_GRID_X; i++)
            {
                for (int j = 0; j < Constants.AREA_GRID_Y; j++)
                {
                    AreaGrid[i, j].Coordinates = new SystemCoordinates(i, j);
                    for (int k = i * 5; k < i * 5 + Constants.AREA_GRID_X; k++)
                    {
                        for (int l = j * 5; l < j * 5 + Constants.AREA_GRID_Y; l++)
                        {
                            ProvinceGrid[k, l] = new Province(AreaGrid[i, j], new SystemCoordinates(k, l));
                            ProvinceGrid[k, l].initialize();
                            AreaGrid[i, j].Provinces.Add(ProvinceGrid[k, l]);
                        }
                    }
                }
            }

        }

        private void defineContinentsAndOceans()
        {
            Regions = new List<Region>();
            foreach (Area area in AreaGrid)
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
                    next_area = AreaGrid[Constants.Random.Next(Constants.AREA_GRID_X), Constants.Random.Next(Constants.AREA_GRID_Y)];

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
                foreach (Area area in AreaGrid)
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
                    Area next_area = getArea(coords);
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
                Deities.Add(new Deity(Constants.Names.GetName("deities")));
            }

            List<ModifierTag> domain_tags = new List<ModifierTag>();
            Array modifier_tags = Enum.GetValues(typeof(ModifierTag));
            for (int i = (int)ModifierTag.DomainsBegin + 1; i < (int)ModifierTag.DomainsEnd; i++)
                domain_tags.Add((ModifierTag)modifier_tags.GetValue(i));

            foreach (Deity deity in Deities)
            {
                for (int i = 0; i < 5; i++)
                {
                    while (deity.Domains[i] == null)
                    {
                        bool is_valid_domain = true;
                        ModifierTag domain = domain_tags[Constants.Random.Next(domain_tags.Count)];

                        // Checks whether there is an incompatible domain and whether there is the same domain already in.
                        for (int j = 0; j < 5; j++)
                            if (deity.Domains[j] != null && (deity.Domains[j].Excludes != null && deity.Domains[j].Excludes.Contains(domain) || deity.Domains[j].Tag == domain))
                                is_valid_domain = false;

                        if (is_valid_domain)
                            deity.Domains[i] = new Modifier(domain);
                    }
                }
            }
        }
    }
}
