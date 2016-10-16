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

        public Area ArmyLocation { get; set; }

        public Army(string name, Deity creator) : base(name, creator)
        {

        }


    }
}
