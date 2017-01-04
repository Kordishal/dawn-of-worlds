using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Creations.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Creations.Organisations
{
    class City : Creation
    {
        // Owner Nation
        public Civilisation Owner { get; set; }

        // Territory
        public TerrainFeatures TerrainFeature { get; set; }

        public List<Building> Buildings { get; set; }

        public CityModifiers Modifiers { get; set; }



        public void changeOwnership(Civilisation to)
        {
            Owner.Cities.Remove(this);
            Owner = to;
        }

        public City(string name, Deity creator): base(name, creator)
        {
            Buildings = new List<Building>();
            Modifiers = new CityModifiers();
        }
    }

    class CityModifiers
    {
        public bool not_hasRaisedArmy { get; set; }
        public int FortificationLevel { get; set; }

        public CityModifiers()
        {
            not_hasRaisedArmy = true;
            FortificationLevel = 0;
        }
    }
}
