using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.WorldClasses
{
    [Serializable]
    class SystemCoordinates
    {
        public int X;
        public int Y;

        public SystemCoordinates North
        {
            get
            {
                return new SystemCoordinates(X + 1, Y);
            }
        }
        public SystemCoordinates NorthEast
        {
            get
            {
                return new SystemCoordinates(X + 1, Y + 1);
            }
        }
        public SystemCoordinates East
        {
            get
            {
                return new SystemCoordinates(X, Y + 1);
            }
        }
        public SystemCoordinates SouthEast
        {
            get
            {
                return new SystemCoordinates(X - 1, Y + 1);
            }
        }
        public SystemCoordinates South
        {
            get
            {
                return new SystemCoordinates(X - 1, Y);
            }
        }
        public SystemCoordinates SouthWest
        {
            get
            {
                return new SystemCoordinates(X - 1, Y - 1);
            }
        }
        public SystemCoordinates West
        {
            get
            {
                return new SystemCoordinates(X, Y - 1);
            }
        }
        public SystemCoordinates NorthWest
        {
            get
            {
                return new SystemCoordinates(X + 1, Y - 1);
            }
        }

        public SystemCoordinates GetNeighbour(int i)
        {
            switch (i)
            {
                case 0:
                    return North;
                case 1:
                    return NorthEast;
                case 2:
                    return East;
                case 3:
                    return SouthEast;
                case 4:
                    return South;
                case 5:
                    return SouthWest;
                case 6:
                    return West;
                case 7:
                    return NorthWest;
                default:
                    return null;
            }
        }

        public bool isInTileGridBounds()
        {
            if (X >= 0 && Y >= 0 && X < Constants.TILE_GRID_X && Y < Constants.TILE_GRID_Y)
                return true;
            else
                return false;
        }

        public bool isInAreaGridBounds()
        {
            if (X >= 0 && Y >= 0 && X < Constants.AREA_GRID_X && Y < Constants.AREA_GRID_Y)
                return true;
            else
                return false;

        }

        public override string ToString()
        {
            return "(x:" + X + "|y:" + Y + ")";
        }

        public SystemCoordinates(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
