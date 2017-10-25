using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Main;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.WorldClasses
{
    [Serializable]
    class Area
    {
        public string Name { get; set; }

        public AreaType Type { get; set; }

        [JsonIgnore]
        public SystemCoordinates Coordinates { get; set; }

        [JsonIgnore]
        public Region Region { get; set; }
       
        public List<Province> Provinces { get; set; }

        public Area(Region region)
        {
            Name = Program.GenerateNames.GetName("area_names");
            Region = region;

            Provinces = new List<Province>();   
        }

        public override string ToString()
        {
            return Name + Coordinates;
        }
    }

    enum AreaType
    {
        Continent,
        Ocean,
    }
}
