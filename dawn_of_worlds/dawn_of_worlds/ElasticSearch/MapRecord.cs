using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using dawn_of_worlds.WorldClasses;

namespace dawn_of_worlds.ElasticSearch
{
    class MapRecord
    {
        public int x_coordinate { get; set; }
        public int y_coordinate { get; set; }

        public string Name { get; set; }
        public string WorldName { get; set; }
        public string AreaName { get; set; }
        public string RegionName { get; set; }

        public string Terrain { get; set; }
        public Climate Climate { get; set; }


    }
}
