using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Creations.Geography;

namespace dawn_of_worlds.CelestialPowers.CommandCityPowers
{
    class ExpandCityInfluence : CommandCity
    {

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            foreach (Area a in _commanded_city.Owner.TerritoryAreas)
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

            foreach (Area a in _commanded_city.Owner.TerritoryAreas)
            {
                unclaimed_territory.AddRange(a.UnclaimedTerritory);
            }

            // Add territory to city and nation
            GeographicalFeature new_territory = unclaimed_territory[Main.MainLoop.RND.Next(unclaimed_territory.Count)];
            _commanded_city.CitySphereOfÌnfluence.Add(new_territory);

            // Tell territory to whom it belongs now.
            new_territory.Owner = _commanded_city.Owner;
            new_territory.SphereOfInfluenceCity = _commanded_city;

            // Remove territory from the unclaimed list.
            foreach (Area a in _commanded_city.Owner.TerritoryAreas)
            {
                if (a.UnclaimedTerritory.Contains(new_territory))
                {
                    a.UnclaimedTerritory.Remove(new_territory);
                }
            }
        }


        public ExpandCityInfluence(City comanded_city) : base(comanded_city)
        {
            Name = "Expand City Influence: " + comanded_city.Name;
        }
    }
}
