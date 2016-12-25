using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.CelestialPowers.CommandArmyPowers;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.CelestialPowers.CommandCityPowers
{
    class RaiseArmy : CommandCity
    {
        public override bool Precondition(Deity creator)
        {
            if (_commanded_city.Modifiers.not_hasRaisedArmy)
                return true;

            return false;
        }

        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

            if (creator.Domains.Contains(Domain.War))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            if (creator.Domains.Contains(Domain.Peace))
                weight -= Constants.WEIGHT_STANDARD_CHANGE;

            return weight >= 0 ? weight : 0;
        }


        public override void Effect(Deity creator)
        {
            // Create a new army and place it on the map.
            Army army = new Army(_commanded_city.Name + " Army", creator);
            army.Location = _commanded_city.TerrainFeature.Province;
            army.Owner = _commanded_city.Owner;

            // Add army to nation which owns the city.
            _commanded_city.Owner.Armies.Add(army);

            // City has raised an army and cannot do so until next turn.
            _commanded_city.Modifiers.not_hasRaisedArmy = false;

            // Powers related to this army.
            army.Creator.Powers.Add(new AttackArmy(army));

            creator.LastCreation = army;
        }


        public RaiseArmy(City commanded_city) : base(commanded_city)
        {
            Name = "Raise Army: " + commanded_city.Name;
        }
    }
}
