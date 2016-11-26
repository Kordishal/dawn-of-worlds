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
        public NationTypes Type { get; set; }

        // Inhabitants
        public Race FoundingRace { get { return InhabitantRaces[0]; } }
        public List<Race> InhabitantRaces { get; set; }
        public Avatar Leader { get; set; }
        public List<Avatar> Subjects { get; set; }

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
        public List<Terrain> TerrainTerritory { get; set; }
        public List<TerrainFeatures> Territory { get; set; }

        // Conflict 
        public bool isDestroyed { get; set; }
        public List<Army> Armies { get; set; }

        // Diplomacy
        public List<Relations> Relationships { get; set; }
        public bool isAtWar
        {
            get
            {
                foreach (Relations relation in Relationships)
                {
                    if (relation.Status == RelationStatus.AtWar)
                        return true;
                }
                return false;
            }
        }

        public List<Nation> AlliedNations { get; set; }
        public List<War> Wars { get; set; }

        // Orders
        public Order OriginOrder
        {
            get
            {
                return NationalOrders[0];
            }
        }
        public List<Order> NationalOrders { get; set; }

        // Status
        public List<NationalTags> Tags { get; set; }

        public void DestroyNation()
        {
            isDestroyed = true;

            foreach (Army a in Armies)
            {
                a.isScattered = true;
            }

            foreach (Nation n in AlliedNations)
            {
                n.AlliedNations.Remove(this);
            }

            for(int i = 0; i < Wars.Count; i++)
            {
                // If this nation is the war leader in another war this war should end as well.
                if (Wars[i].Attackers[0].Equals(this) || Wars[i].Defenders[0].Equals(this))
                {
                    War war = Wars[i];
                    foreach (Nation attacker in war.Attackers)
                    {
                        attacker.Wars.Remove(war);
                    }
                    foreach (Nation defender in war.Defenders)
                    {
                        defender.Wars.Remove(war);
                    }

                    this.Territory[0].Location.Area.RegionArea.RegionWorld.OngoingWars.Remove(war);
                    war = null;
                } // an attacker or defender ally is simply removed from the war.
                else if (Wars[i].Attackers.Contains(this))
                {
                    Wars[i].Attackers.Remove(this);
                }
                else if (Wars[i].Defenders.Contains(this))
                {
                    Wars[i].Defenders.Remove(this);
                }
                // Wars are removed while removing them
                i -= 1;
            }
        }


        public Nation(string name, Deity creator) :base(name, creator)
        {
            InhabitantRaces = new List<Race>();
            Subjects = new List<Avatar>();
            Cities = new List<City>();
            TerrainTerritory = new List<Terrain>();
            Territory = new List<TerrainFeatures>();            
            Armies = new List<Army>();
            Relationships = new List<Relations>();
            AlliedNations = new List<Nation>();
            Wars = new List<War>();
            NationalOrders = new List<Order>();
            Tags = new List<NationalTags>();
            isDestroyed = false;       
        }
    }

    enum NationTypes
    {
        LairTerritory,
        NomadicTribe,
        TribalNation,
        FeudalNation,
    }

    enum NationalTags
    {
        VeryRich,
        GoldMine,
    }
}
