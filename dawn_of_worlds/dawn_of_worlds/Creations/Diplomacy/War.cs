using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Civilisations;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Log;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Creations.Diplomacy
{
    [Serializable]
    class War : Creation
    {
        public bool hasEnded { get; set; }

        public int Begin { get; set; }
        public int End { get; set; }

        public List<Civilisation> Attackers { get; set; }
        public List<Civilisation> Defenders { get; set; }

        public WarGoal WarGoalAttackers { get; set; }
        public WarGoal WarGoalDefenders { get; set; }

        public bool isInWar(Civilisation nation)
        {
            return isDefender(nation) || isAttacker(nation);
        }
        public bool isWarLeader(Civilisation nation)
        {
            if (Attackers[0] == nation || Defenders[0] == nation)
                return true;
            else
                return false;
        }
        public bool isDefender(Civilisation nation)
        {
            foreach (Civilisation defender in Defenders)
                if (defender == nation)
                    return true;

            return false;
        }
        public bool isAttacker(Civilisation nation)
        {
            foreach (Civilisation attacker in Attackers)
                if (attacker == nation)
                    return true;

            return false;
        }


        public War(string name, Deity creator) : base(name, creator)
        {
            Attackers = new List<Civilisation>();
            Defenders = new List<Civilisation>();
            hasEnded = false;
        }

        public static string printWar(Record record)
        {
            string result = "";
            result += "Name: " + record.War.Name.Singular + "\n";
            result += "Beginn: " + record.War.Begin + "\n";
            if (record.War.hasEnded)
                result += "End: " + record.War.End + "\n";
            result += "Attackers: ";
            foreach (Civilisation nation in record.War.Attackers)
                result += nation.Name + " ";
            result += "\n";
            result += "Wargoal: " + record.War.WarGoalAttackers + "\n";
            result += "Defenders: ";
            foreach (Civilisation nation in record.War.Defenders)
                result += nation.Name + " ";
            result += "\n";
            result += "Wargoal: " + record.War.WarGoalDefenders + "\n";
            return result;
        }
    }

    class WarGoal
    {
        public WarGoalType Type { get; set; }
        public Civilisation Winner { get; set; }
        public City City { get; set; }
        public Province Territory { get; set; }

        public WarGoal(WarGoalType type)
        {
            Type = type;
        }
    }

    enum WarGoalType
    {
        CityConquest,
        Conquest,
        RemoveNomadicPresence,
        VassalizeCity,
        TravelAreaConquest,
    }
}
