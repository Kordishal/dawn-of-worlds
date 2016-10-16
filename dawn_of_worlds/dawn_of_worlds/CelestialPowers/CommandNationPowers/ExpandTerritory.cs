using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Creations.Geography;

namespace dawn_of_worlds.CelestialPowers.CommandNationPowers
{
    class ExpandTerritory : CommandNation
    {

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            foreach (Area a in _commanded_nation.TerritoryAreas)
            {
                if (a.UnclaimedTerritory.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }


        public override void Effect(World current_world, Deity creator, int current_age)
        {
            List<GeographicalFeature> unclaimed_territory = new List<GeographicalFeature>();

            foreach (Area a in _commanded_nation.TerritoryAreas)
            {
                unclaimed_territory.AddRange(a.UnclaimedTerritory);
            }


            GeographicalFeature new_territory = unclaimed_territory[Main.MainLoop.RND.Next(unclaimed_territory.Count)];

            _commanded_nation.Territory.Add(new_territory);
            new_territory.Owner = _commanded_nation;

            foreach (Area a in _commanded_nation.TerritoryAreas)
            {
                if (a.UnclaimedTerritory.Contains(new_territory))
                {
                    a.UnclaimedTerritory.Remove(new_territory);
                }
            }


        }


        public ExpandTerritory(Nation commanded_nation) : base(commanded_nation)
        {
            Name = "Expand Territory: " + commanded_nation.Name;
        }
    }
}
