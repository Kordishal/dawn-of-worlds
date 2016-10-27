using dawn_of_worlds.Actors;
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
        public City WarGoalCity { get; set; }
        public List<Geography.GeographicalFeature> WarGoalTerritory { get; set; }

        public WarGoal(City city, List<Geography.GeographicalFeature> territory)
        {
            WarGoalCity = city;
            WarGoalTerritory = territory;

            if (WarGoalTerritory == null)
                WarGoalTerritory = new List<Geography.GeographicalFeature>();

            if (WarGoalCity != null)
            {
                WarGoalTerritory.Add(WarGoalCity.CityLocation);
            }
        }
    }
}
