using dawn_of_worlds.Creations.Civilisations;
using dawn_of_worlds.Creations.Organisations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.CelestialPowers.EventPowers.NationalEvents
{
    abstract class CivilisationEvents : SpawnEvent
    {
        protected Civilisation _nation { get; set; }


        public CivilisationEvents(Civilisation nation)
        {
            _nation = nation;
            initialize();
        }
    }
}
