using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Creations.Organisations;
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

        public Area[] Neighbours { get; set; }
        public Area[] RandomizedDirections()
        {
            switch (Main.MainLoop.RND.Next(4))
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

        public Region AreaRegion { get; set; }

        public List<Nation> Nations { get; set; }
        public List<Army> Armies { get; set; }

        public List<GeographicalFeature> GeographicalFeatures { get; set; }
        public List<GeographicalFeature> UnclaimedTerritory { get; set; }

        public List<Forest> Forests { get; set; }

        public List<Lake> Lakes { get; set; }
        public List<River> Rivers { get; set; }

        public MountainRange MountainRanges { get; set; }

        public List<Race> Inhabitants { get; set; }

        public Area(Region region)
        {
            Name = id.ToString();
            id += 1;
            AreaRegion = region;
            GeographicalFeatures = new List<GeographicalFeature>();
            UnclaimedTerritory = new List<GeographicalFeature>();
            Forests = new List<Forest>();
            Lakes = new List<Lake>();
            Rivers = new List<River>();
            Neighbours = new Area[4];
            AreaClimate = new Climate();
            Inhabitants = new List<Race>();
            Armies = new List<Army>();
            Nations = new List<Nation>();
        }


        public override string ToString()
        {
            return "Area: " + Name;
        }
    }
}
