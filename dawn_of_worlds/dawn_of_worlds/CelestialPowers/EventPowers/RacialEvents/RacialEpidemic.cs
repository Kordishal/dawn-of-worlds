using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.CelestialPowers.EventPowers.RacialEvents
{
    class RacialEpidemic : RacialEvent
    {
        public RacialEpidemic(Race race) : base(race)
        {
            Name = "Racial Event: Racial Epidemic!";
        }

        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

            if (creator.Domains.Contains(Domain.Pestilence))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        public override bool Precondition(Deity creator)
        {
            return !_race.Tags.Contains(RaceTags.RacialEpidemic);
        }

        public override void Effect(Deity creator)
        {
            _race.Tags.Add(RaceTags.RacialEpidemic);

            creator.LastCreation = _race;
        }
    }
}
