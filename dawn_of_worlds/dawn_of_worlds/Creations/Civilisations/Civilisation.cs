using dawn_of_worlds.Actors;
using dawn_of_worlds.CelestialPowers.CommandNationPowers;
using dawn_of_worlds.Creations.Diplomacy;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Creations.Civilisations
{
    class Civilisation : Creation
    {
        // General
        public Polity PoliticalOrganisation { get; set; }
        public bool isNomadic { get; set; }

        // Inhabitants
        public Race FoundingRace { get { return InhabitantRaces[0]; } }
        public List<Race> InhabitantRaces { get; set; }

        public Avatar Leader { get; set; }
        public List<Avatar> Avatars { get; set; }

        // Cities
        public bool hasCities { get; set; }
        public City CapitalCity
        {
            get
            {
                return Cities[0];
            }
        }
        public List<City> Cities { get; set; }

        // Territory
        public List<Province> Territory { get; set; }

        // Conflict 
        public bool isDestroyed { get; set; }
        public List<Army> Armies { get; set; }
        public List<WarGoal> PossibleWarGoals { get; set; }


        // Diplomacy
        public bool hasDiplomacy { get; set; }
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
        public List<CivilisationTags> LocalTags { get; set; }

        public void DestroyNation()
        {
            isDestroyed = true;

            foreach (Army a in Armies)
            {
                a.isScattered = true;
            }

            foreach (Relations relation in Relationships)
            {
                if (relation.Status == RelationStatus.AtWar)
                {
                    (new WhitePeace(this, Program.World.OngoingWars.Find(x => x.isInWar(this) && x.isInWar(relation.Target)))).Effect(Creator);
                }
                relation.Status = RelationStatus.None;
            }
        }

        public Civilisation(string name, Deity creator) :base(name, creator)
        {
            InhabitantRaces = new List<Race>();
            Avatars = new List<Avatar>();
            Cities = new List<City>();
            Territory = new List<Province>();         
            Armies = new List<Army>();
            Relationships = new List<Relations>();
            NationalOrders = new List<Order>();
            LocalTags = new List<CivilisationTags>();
            PossibleWarGoals = new List<WarGoal>();
            isDestroyed = false;       
        }

        public string printCivilisation()
        {
            string result = "";
            result += "Name: " + Name + "\n";
            result += PoliticalOrganisation.print() + "\n";
            result += "Founding Race: " + FoundingRace.Name + "\n";
            result += "Races: ";
            result += "Destroyed: " + isDestroyed + "\n";
            if (isDestroyed)
                return result;
            foreach (Race race in InhabitantRaces)
                result += race.Name + " ";
            result += "\n";
            if (Leader != null)
                result += "Leader: " + Leader.Name + "\n";
            result += "Avatars: ";
            foreach (Avatar avatar in Avatars)
                result += avatar.Name + " ";
            result += "\n";
            if (hasCities)
            {
                result += "Capital City: " + CapitalCity.Name + "\n";
                result += "Cities: ";
                foreach (City city in Cities)
                    result += city.Name + " ";
                result += "\n";
            }
            result += "Territory: ";
            foreach (Province province in Territory)
                result += province.Name + " ";
            result += "\n";
            result += "Orders: ";
            foreach (Order order in NationalOrders)
                result += order.Name + " ";
            result += "\n";
            result += "Tags: ";
            foreach (CivilisationTags tag in LocalTags)
                result += tag + " ";
            result += "\n";
            return result;
        }
    }
}
