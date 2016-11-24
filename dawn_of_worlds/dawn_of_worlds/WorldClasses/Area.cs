using dawn_of_worlds.Creations.Geography;
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
    class Area
    {
        private static int id = 0;

        public string Name { get; set; }

        public Climate AreaClimate { get; set; }

        public Region AreaRegion { get; set; }

        public List<Terrain> Terrain { get; set; }
        public List<Forest> Forests { get; set; }
        public List<Grassland> Grasslands { get; set; }
        public List<Desert> Deserts { get; set; }

        public List<Cave> Caves { get; set; }

        public List<Lake> Lakes { get; set; }
        public List<River> Rivers { get; set; }

        public MountainRange MountainRanges { get; set; }
        public HillRange HillRanges { get; set; }

        public List<Race> Inhabitants { get; set; }

        public List<Nation> Nations { get; set; }
        public List<Army> Armies { get; set; }

        public List<Terrain> UnclaimedTerritory { get; set; }

        public Area(Region region)
        {
            Name = Constants.Names.GetName("area");
            id += 1;
            AreaRegion = region;
            Terrain = new List<Terrain>();
            UnclaimedTerritory = new List<Terrain>();
            Forests = new List<Forest>();
            Lakes = new List<Lake>();
            Rivers = new List<River>();
            Neighbours = new Area[4];
            AreaClimate = new Climate();
            Inhabitants = new List<Race>();
            Armies = new List<Army>();
            Nations = new List<Nation>();
            DiagonalNeighbours = new Area[4];
            Grasslands = new List<Grassland>();
            Deserts = new List<Desert>();
            Caves = new List<Cave>();
        }

        public Area[] Neighbours { get; set; }
        public Area[] DiagonalNeighbours { get; set; }
        public Area[] RandomizedDirections()
        {
            switch (Main.Constants.RND.Next(4))
            {
                case 0:
                    return new Area[4] { _north, _east, _south, _west };
                case 1:
                    return new Area[4] { _east, _south, _west, _north };
                case 2:
                    return new Area[4] { _south, _west, _north, _east };
                case 3:
                    return new Area[4] { _west, _north, _east, _south };
                default:
                    return new Area[4] { _north, _south, _east, _west };
            }
        }
        public Area[] GetNeighbours(int primary_direction)
        {
            switch (primary_direction)
            {
                case 0:
                    return new Area[4] { _north, _east, _west, _south };
                case 1:
                    return new Area[4] { _east, _south, _north, _west };
                case 2:
                    return new Area[4] { _south, _west, _east, _north };
                case 3:
                    return new Area[4] { _west, _north, _south, _east };
                default:
                    return new Area[4] { _north, _south, _east, _west };
            }
        }

        // X + 1
        private Area _north;
        public Area North
        {
            get
            {
                return _north;
            }
            set
            {
                _north = value;
                Neighbours[0] = value;
            }
        }
        // X - 1
        private Area _south;
        public Area South
        {
            get
            {
                return _south;
            }
            set
            {
                _south = value;
                Neighbours[2] = value;
            }
        }
        // Y + 1
        private Area _east;
        public Area East
        {
            get
            {
                return _east;
            }
            set
            {
                _east = value;
                Neighbours[1] = value;
            }
        }
        // Y - 1
        private Area _west;
        public Area West
        {
            get
            {
                return _west;
            }
            set
            {
                _west = value;
                Neighbours[3] = value;
            }
        }

        // X + 1 Y + 1
        private Area _north_east;
        public Area NorthEast
        {
            get
            {
                return _north_east;
            }
            set
            {
                _north_east = value;
                DiagonalNeighbours[0] = value;
            }
        }
        // X - 1 Y + 1
        private Area _south_east;
        public Area SouthEast
        {
            get
            {
                return _south_east;
            }
            set
            {
                _south_east = value;
                DiagonalNeighbours[1] = value;
            }
        }
        // X - 1 Y - 1
        private Area _south_west;
        public Area SouthWest
        {
            get
            {
                return _south_west;
            }
            set
            {
                _south_west = value;
                DiagonalNeighbours[2] = value;
            }
        }
        // X + 1 Y - 1
        private Area _north_west;
        public Area NorthWest
        {
            get
            {
                return _north_west;
            }
            set
            {
                _north_west = value;
                DiagonalNeighbours[3] = value;
            }
        }

        public override string ToString()
        {
            return "Area: " + Name;
        }


        public string printArea()
        {
            string result = "";
            result += "Name: " + Name + "\n";
            result += "Region: " + AreaRegion.Name + "\n";
            result += "Ocean: " + (AreaRegion.Landmass ? "no" : "yes") + "\n";
            result += "Climate: " + AreaClimate.ToString() + "\n";
            result += "Terrain: " + Terrain.Count.ToString() + "\n";
            result += "Forests: " + Forests.Count.ToString() + "\n";
            result += "Grasslands: " + Grasslands.Count.ToString() + "\n";
            result += "Deserts: " + Deserts.Count.ToString() + "\n";
            result += "Caves: " + Caves.Count.ToString() + "\n";
            result += "Rivers: " + Rivers.Count.ToString() + "\n";
            result += "Lakes: " + Lakes.Count.ToString() + "\n";
            result += "Mountains: " + (MountainRanges != null ? MountainRanges.Mountains.Count.ToString() : "none") + "\n";
            result += "Hills: " + (HillRanges != null ? HillRanges.Hills.Count.ToString() : "none") + "\n";
            result += "Races: " + Inhabitants.Count.ToString() + "\n";
            result += "Nations: " + Nations.Count.ToString() + "\n";
            result += "Unclaimed Territory: " + UnclaimedTerritory.Count.ToString() + "\n";
            result += "Armies: " + Armies.Count.ToString() + "\n";


            return result;
        }
    }


    class WeightedArea
    {
        public Area Area { get; set; }
        public int Weight { get; set; }

        public WeightedArea(Area area)
        {
            Area = area;
            Weight = 0;
        }
    }
}
