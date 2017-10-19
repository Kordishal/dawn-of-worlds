using dawn_of_worlds.Names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Main
{
    class Constants
    {
        static public Random Random { get; set; }

        public const string HISTORY_FOLDER = @"C:\Users\Jonas Waeber\Documents\Projects\dawn_of_worlds\dawn_of_worlds\dawn_of_worlds\Log\Output\History\";

        public const int TOTAL_TURNS = 100;
        public const int DEITY_ACTIONS_PER_TURN = 10;
        public const int DEITY_BASE_POWERPOINT_MAX_GAIN = 12;
        public const int DEITY_BASE_POWERPOINT_MIN_GAIN = 2;

        public const int AREA_GRID_X = 5;
        public const int AREA_GRID_Y = 5;

        public const int TILE_GRID_X = 25;
        public const int TILE_GRID_Y = 25;
        public const int TOTAL_PROVINCE_NUMBER = TILE_GRID_X * TILE_GRID_Y;

        public const int ARCTIC_CLIMATE_BORDER = 3;
        public const int SUB_ARCTIC_CLIMATE_BORDER = 8;
        public const int TEMPERATE_CLIMATE_BORDER = 17;
        public const int SUB_TROPICAL_CLIMATE_BORDER = 22;
        public const int TROPICAL_CLIMATE_BORDER = 25;

        public const int BASE_TILES_SETTLED_BY_RACE = 5;

        // Anything that costs less than this gets a bonus, anything that costs more than this gets a penalty.
        public const int WEIGHT_COST_DEVIATION_MEDIUM = 10;

        public const int WEIGHT_STANDARD_LOW = 100;
        public const int WEIGHT_STANDARD_MEDIUM = 200;
        public const int WEIGHT_STANDARD_HIGH = 300;

        public const int WEIGHT_STANDARD_CHANGE = 100;

        public const int WEIGHT_STANDARD_COST_DEVIATION = 10;

        public const int WEIGHT_MANY_LOW = 5;
        public const int WEIGHT_MANY_MEDIUM = 10;
        public const int WEIGHT_MANY_HIGH = 15;

        public const int WEIGHT_MANY_CHANGE = 5;

        public const int WEIGHT_MANY_COST_DEVIATION = 1;

        public const int COST_CHANGE_VALUE = 2;


        static Constants()
        {
            Random = new Random(100);
        }
    }

    class WeightedObjects<S>
    {
        public S Object { get; set; }
        public int Weight { get; set; }

        public static int TotalWeight(List<WeightedObjects<S>> weighted_objects)
        {
            int result = 0;
            foreach (WeightedObjects<S> objects in weighted_objects)
            {
                result += objects.Weight;
            }
            return result;
        }

        public static S ChooseRandomObject(List<WeightedObjects<S>> weighted_objects)
        {
            int chance = Constants.Random.Next(TotalWeight(weighted_objects));
            int prev_weight = 0, current_weight = 0;
            foreach (WeightedObjects<S> weigted_object in weighted_objects)
            {
                current_weight += weigted_object.Weight;
                if (prev_weight <= chance && chance < current_weight)
                {
                    return weigted_object.Object;
                }
                prev_weight += weigted_object.Weight;
            }

            return default(S);
        }

        public static List<S> ChooseHeaviestObjects(List<WeightedObjects<S>> weighted_objects)
        {
            weighted_objects.Sort(WeightedObjects<S>.Compare);
            List<S> objects = new List<S>();
            weighted_objects = weighted_objects.FindAll(x => weighted_objects[0].Weight == x.Weight);
            foreach (WeightedObjects<S> weighted_object in weighted_objects)
                objects.Add(weighted_object.Object);

            return objects;
        }

        public static List<S> ChooseXHeaviestObjects(List<WeightedObjects<S>> weighted_objects, int X)
        {
            weighted_objects.Sort(WeightedObjects<S>.Compare);
            List<S> objects = new List<S>();

            for (int i = 0; i < X; i++)
            {
                objects.Add(weighted_objects[i].Object);
            }

            return objects;
        }


        public static int Compare(WeightedObjects<S> first, WeightedObjects<S> second)
        {
            if (first.Weight > second.Weight)
                return -1;
            else if (first.Weight == second.Weight)
                return 0;
            else //(first.Weight < second.Weight)
                return 1;
        }

        public WeightedObjects(S objects)
        {
            Object = objects;
            Weight = 0;
        }
    }
}
