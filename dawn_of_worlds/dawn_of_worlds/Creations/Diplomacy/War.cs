using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Creations.Organisations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Creations.Diplomacy
{
    class War : Creation
    {
        public List<Nation> Attackers { get; set; }
        public List<Nation> Defenders { get; set; }

        public WarGoal WarGoalAttackers { get; set; }
        public WarGoal WarGoalDefenders { get; set; }


        public War(string name, Deity creator) : base(name, creator)
        {
            Attackers = new List<Nation>();
            Defenders = new List<Nation>();
        }
    }

    struct WarGoal
    {
        public Nation Winner { get; set; }
        public City WarGoalCity { get; set; }

        public WarGoal(Nation winner, City city)
        {
            Winner = winner;
            WarGoalCity = city;
        }
    }
}
