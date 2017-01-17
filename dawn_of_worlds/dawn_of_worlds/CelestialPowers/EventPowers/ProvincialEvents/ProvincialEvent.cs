using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Effects;

namespace dawn_of_worlds.CelestialPowers.EventPowers.ProvincialEvents
{
    abstract class ProvincialEvent : SpawnEvent
    {
        protected Province _affected_province { get; set; }

        public override bool Precondition(Deity creator)
        {
            foreach (Modifier modifier in _affected_province.ProvincialModifiers)
            {
                {
                    if (modifier.Forbids != null)
                        for (int i = 0; i < modifier.Forbids.Length; i++)
                            if (Tags.Contains(modifier.Forbids[i]))
                                return false;
                }
            }

            return true;
        }

        public ProvincialEvent(Province affected_province)
        {
            _affected_province = affected_province;
            initialize();
        }
    }
}
