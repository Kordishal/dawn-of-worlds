using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Diplomacy;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Creations.Organisations
{
    class Nation : Creation
    {

        // Inhabitants
        public Race FoundingRace { get; set; }

        // Territory
        public List<Area> TerritoryAreas { get; set; }
        public List<GeographicalFeature> Territory { get; set; }

        // Cities
        public List<City> Cities { get; set; }

        // Diplomacy
        public List<Alliance> Alliances { get; set; }


        public Nation(string name, Deity creator) :base(name, creator)
        {
            Territory = new List<GeographicalFeature>();
            TerritoryAreas = new List<Area>();
            Cities = new List<City>();
            Alliances = new List<Alliance>();
        }
    }
}
