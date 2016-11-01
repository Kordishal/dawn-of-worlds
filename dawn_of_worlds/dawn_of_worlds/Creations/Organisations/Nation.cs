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

        // Cities
        public City CapitalCity
        {
            get
            {
                return Cities[0];
            }
        }
        public List<City> Cities { get; set; }

        // Territory
        public List<Area> TerritoryAreas { get; set; }

        // Conflict 
        public List<Army> Armies { get; set; }

        // Diplomacy
        public List<Nation> AlliedNations { get; set; }
        public List<War> Wars { get; set; }


        public void DestroyNation()
        {
            this.Creator.FoundedNations.Remove(this);

            foreach (Area a in TerritoryAreas)
            {
                a.Nations.Remove(this);
            }

            foreach (Army a in Armies)
            {
                a.ArmyLocation.Armies.Remove(a);
            }
            Armies.Clear();

            foreach (Nation n in AlliedNations)
            {
                n.AlliedNations.Remove(this);
            }

            foreach (War w in Wars)
            {
                if (w.Attackers.Contains(this))
                {
                    //if (w.Attackers[0] == w.)
                }
            }
        }


        public Nation(string name, Deity creator) :base(name, creator)
        {
            Cities = new List<City>();

            TerritoryAreas = new List<Area>();
            
            Armies = new List<Army>();

            AlliedNations = new List<Nation>();
            Wars = new List<War>();          
        }
    }
}
