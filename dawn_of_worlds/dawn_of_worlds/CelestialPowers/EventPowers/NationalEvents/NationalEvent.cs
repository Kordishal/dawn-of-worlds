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
        protected Civilisation _nation { get; set; }


        public NationalEvent(Civilisation nation)
        {
            _nation = nation;
            initialize();
        }
    }
}
