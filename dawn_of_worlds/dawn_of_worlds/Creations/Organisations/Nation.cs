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
        public bool isDestroyed { get; set; }
        public List<Army> Armies { get; set; }

        // Diplomacy
        public List<Nation> AlliedNations { get; set; }
        public List<War> Wars { get; set; }


        public void DestroyNation()
        {
            isDestroyed = true;

            foreach (Area a in TerritoryAreas)
            {
                a.Nations.Remove(this);
            }

            foreach (Army a in Armies)
            {
                a.isScattered = true;
            }

            foreach (Nation n in AlliedNations)
            {
                n.AlliedNations.Remove(this);
            }

            foreach (War w in Wars)
            {
                // If this nation is the war leader in another war this war should end as well.
                if (w.Attackers[0].Equals(this) || w.Defenders[0].Equals(this))
                {
                    foreach (Nation attacker in w.Attackers)
                    {
                        attacker.Wars.Remove(w);
                    }
                    foreach (Nation defender in w.Defenders)
                    {
                        defender.Wars.Remove(w);
                    }

                    this.TerritoryAreas[0].AreaRegion.RegionWorld.OngoingWars.Remove(w);
                } // an attacker or defender ally is simply removed from the war.
                else if (w.Attackers.Contains(this))
                {
                    w.Attackers.Remove(this);
                }
                else if (w.Defenders.Contains(this))
                {
                    w.Defenders.Remove(this);
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

            isDestroyed = false;       
        }
    }
}
