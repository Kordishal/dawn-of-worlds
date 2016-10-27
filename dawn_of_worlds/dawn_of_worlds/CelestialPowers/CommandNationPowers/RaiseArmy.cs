using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Organisations;

namespace dawn_of_worlds.CelestialPowers.CommandNationPowers
{
    class RaiseArmy : CommandNation
    {
        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            if (_commanded_nation.Cities.Count > 0)
            {
                foreach (City c in _commanded_nation.Cities)
                {
                    if (c.not_hasRaisedArmy)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        public override void Effect(World current_world, Deity creator, int current_age)
        {

            City garrison_city = null;

            while (garrison_city == null)
            {
                garrison_city = _commanded_nation.Cities[Main.MainLoop.RND.Next(_commanded_nation.Cities.Count)];

                if (!garrison_city.not_hasRaisedArmy)
                {
                    garrison_city = null;
                }
            }

            Army army = new Army(_commanded_nation.Name + " Army", creator);
            army.ArmyLocation = garrison_city.CityLocation.Location;
            army.ArmyLocation.Armies.Add(army);

            _commanded_nation.Armies.Add(army);

            garrison_city.not_hasRaisedArmy = false;     
        }


        public RaiseArmy(Nation commanded_nation) : base(commanded_nation)
        {
            Name = "Raise Army: " + commanded_nation.Name;
        }
    }
}
