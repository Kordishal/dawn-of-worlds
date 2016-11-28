using dawn_of_worlds.Actors;
using dawn_of_worlds.CelestialPowers.CommandNationPowers;
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

        public void DestroyNation(World current_world)
        {
            isDestroyed = true;

            foreach (Army a in Armies)
            {
                a.isScattered = true;
            }
            foreach (Relations relation in Relationships)
            {
                relation.Target.Relationships.Remove(relation.Target.Relationships.Find(x => x.Target.Equals(this)));
            }

            for (int i = 0; i < current_world.OngoingWars.Count; i++)
            {
                War current_war = current_world.OngoingWars[i];

                if (current_war.isInWar(this))
                {
                    new WhitePeace(this, current_war).Effect(current_world, Creator, 0);
                }
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
