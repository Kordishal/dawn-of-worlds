using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Creations.Organisations
{
    class Army : Creation
    {
        public int ArmyStrenghtBonus { get; set; }

        public Nation Owner { get; set; }

        public bool isScattered { get; set; }

        public Area ArmyLocation { get; set; }

        public Army(string name, Deity creator) : base(name, creator)
        {
            ArmyStrenghtBonus = 0;
        }


    }
}
