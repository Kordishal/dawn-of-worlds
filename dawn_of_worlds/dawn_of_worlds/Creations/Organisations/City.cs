using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Geography;
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
        public Nation Owner { get; set; }

        // Territory
        public TerrainFeatures CityLocation
        {
            get
            {
                return CitySphereOfÌnfluence[0];
            }
        }
        public List<TerrainFeatures> CitySphereOfÌnfluence { get; set; }

        // A city can only raise one army per turn.
        public bool not_hasRaisedArmy { get; set; }

        public void changeOwnership(Nation to)
        {
            foreach (TerrainFeatures sphere_of_influence in CitySphereOfÌnfluence)
            {
                sphere_of_influence.NationalTerritory = to;
                Owner.Territory.Remove(sphere_of_influence);
            }
            Owner.Cities.Remove(this);
            Owner = to;
        }

        public City(string name, Deity creator): base(name, creator)
        {
            CitySphereOfÌnfluence = new List<TerrainFeatures>();
            not_hasRaisedArmy = true;
        }
    }
}
