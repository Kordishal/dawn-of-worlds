using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Creations.Inhabitants
{
    class Race : Creation
    {
        public Area HomeArea { get; set; }

        public bool isSubRace { get; set; }
        public Race MainRace { get; set; }

        public List<Race> SubRaces { get; set; }

        public Organisation OriginOrder { get; set; }

        public Race(string name, Deity creator, Area home, Organisation originorder) : base(name, creator)
        {
            HomeArea = home;
            SubRaces = new List<Race>();
            OriginOrder = originorder;
        }
    }
}
