using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Inhabitants;

namespace dawn_of_worlds.CelestialPowers.EventPowers
{
    abstract class RacialEvent : SpawnEvent
    {
        protected Race _race { get; set; }

        public RacialEvent(Race race)
        {
            _race = race;
            initialize();
        }
    }
}
