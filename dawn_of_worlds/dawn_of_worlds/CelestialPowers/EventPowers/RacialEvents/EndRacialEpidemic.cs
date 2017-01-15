using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Effects;

namespace dawn_of_worlds.CelestialPowers.EventPowers.RacialEvents
{
    class EndRacialEpidemic : RacialEvent
    {
        protected override void initialize()
        {
            base.initialize();
            Name = "Racial Event: End Racial Epidemic (" + _race.Name + ")";
            Tags = new List<CreationTag>() { CreationTag.Health };
        }

        public override bool Precondition(Deity creator)
        {
            base.Precondition(creator);

            return _race.Modifiers.Exists(x => x.Tag == ModifierTag.RacialEpidemic);
        }

        public override void Effect(Deity creator)
        {
            _race.Modifiers.Remove(_race.Modifiers.Find(x => x.Tag == ModifierTag.RacialEpidemic));
       
            creator.LastCreation = _race;
        }

        public EndRacialEpidemic(Race race) : base(race) { }
    }
}
