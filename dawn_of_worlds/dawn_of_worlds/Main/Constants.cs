using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Main
{
    class Constants
    {
        static public Random RND = new Random(100);

        public const int AREA_GRID_X = 5;
        public const int AREA_GRID_Y = 5;

        // Anything that costs less than this gets a bonus, anything that costs more than this gets a penalty.
        public const int WEIGHT_COST_DEVIATION_MEDIUM = 10;

        public const int WEIGHT_STANDARD_LOW = 25;
        public const int WEIGHT_STANDARD_MEDIUM = 50;
        public const int WEIGHT_STANDARD_HIGH = 75;

        public const int WEIGHT_STANDARD_CHANGE = 25;

        public const int WEIGHT_STANDARD_COST_DEVIATION = 3;

        public const int WEIGHT_MANY_LOW = 5;
        public const int WEIGHT_MANY_MEDIUM = 10;
        public const int WEIGHT_MANY_HIGH = 15;

        public const int WEIGHT_MANY_CHANGE = 5;

        public const int WEIGHT_MANY_COST_DEVIATION = 1;
    }
}
