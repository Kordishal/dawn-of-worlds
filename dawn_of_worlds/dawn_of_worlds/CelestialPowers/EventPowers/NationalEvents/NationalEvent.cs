using dawn_of_worlds.Creations.Organisations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.CelestialPowers.EventPowers.NationalEvents
{
    abstract class NationalEvent : SpawnEvent
    {
        protected Nation _nation { get; set; }


        public NationalEvent(Nation nation)
        {
            _nation = nation;
        }
    }
}
