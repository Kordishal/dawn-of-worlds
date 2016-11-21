using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.WorldClasses;

namespace dawn_of_worlds.CelestialPowers.EventPowers.RacialEvents
{
    class RacialEpidemic : RacialEvent
    {
        public RacialEpidemic(Race race) : base(race)
        {
            Name = "Racial Event: Racial Epidemic!";
        }

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            return !_race.Tags.Contains(RaceTags.RacialEpidemic);
        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            _race.Tags.Add(RaceTags.RacialEpidemic);
        }
    }
}
