using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Organisations;

namespace dawn_of_worlds.CelestialPowers.CommandCityPowers
{
    class RaiseArmy : CommandCity
    {
        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            if (_commanded_city.not_hasRaisedArmy)
                return true;

            return false;
        }


        public override void Effect(World current_world, Deity creator, int current_age)
        {
            // Create a new army and place it on the map.
            Army army = new Army(_commanded_city.Name + " Army", creator);
            army.ArmyLocation = _commanded_city.CityLocation.Location;
            army.ArmyLocation.Armies.Add(army);

            // Add army to nation which owns the city.
            _commanded_city.Owner.Armies.Add(army);

            // City has raised an army and cannot do so until next turn.
            _commanded_city.not_hasRaisedArmy = false;     
        }


        public RaiseArmy(City commanded_city) : base(commanded_city)
        {
            Name = "Raise Army: " + commanded_city.Name;
        }
    }
}
