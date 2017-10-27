using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldModel;
using dawn_of_worlds.Effects;

namespace dawn_of_worlds.CelestialPowers.EventPowers.ProvincialEvents
{
    class PlagueBreakOut : ProvincialEvent
    {
        protected override void initialize()
        {
            base.initialize();
            Name = "Event: Plague Break Out (" +_affected_province.Name + ")";
            Tags = new List<string>() { "disease", "death" };
        }


        public override bool Precondition(Deity creator)
        {
            if (!base.Precondition(creator))
                return false;

            if (_affected_province.SettledRaces.Count == 0)
                return false;

            if (Diseases.Plague.AffectedProvinces.Contains(_affected_province))
                return false;


            return true;
        }

        public override int Effect(Deity creator)
        {
            Diseases.Plague.AffectedProvinces.Add(_affected_province);
            _affected_province.ProvincialModifiers.Add(Diseases.Plague.Effect);

            return 0;
        }

        public PlagueBreakOut(Province affected_province) : base(affected_province)
        {
        }
    }
}
