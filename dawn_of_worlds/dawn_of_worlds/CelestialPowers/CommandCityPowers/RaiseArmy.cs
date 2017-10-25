using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldModel;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Main;
using dawn_of_worlds.Effects;

namespace dawn_of_worlds.CelestialPowers.CommandCityPowers
{
    class RaiseArmy : CommandCity
    {

        protected override void initialize()
        {
            base.initialize();
            Name = "Raise Army: " + _commanded_city.Name;
            Tags = new List<CreationTag>() { CreationTag.Peace, CreationTag.Diplomacy };
        }

        public override bool Precondition(Deity creator)
        {
            base.Precondition(creator);
            if (_commanded_city.Modifiers.not_hasRaisedArmy)
                return true;

            return false;
        }

        public override int Effect(Deity creator)
        {
            // Create a new army and place it on the map.
            Army army = new Army(_commanded_city.Name + " Army", creator);
            army.Location = _commanded_city.TerrainFeature.Province;
            army.Owner = _commanded_city.Owner;

            // Add army to nation which owns the city.
            _commanded_city.Owner.Armies.Add(army);

            // City has raised an army and cannot do so until next turn.
            _commanded_city.Modifiers.not_hasRaisedArmy = false;
            creator.LastCreation = army;
            return 0;
        }


        public RaiseArmy(City commanded_city) : base(commanded_city)
        {
            initialize();
        }
    }
}
