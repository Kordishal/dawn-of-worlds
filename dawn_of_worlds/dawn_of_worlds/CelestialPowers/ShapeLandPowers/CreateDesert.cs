using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    class CreateDesert : ShapeLand
    {
        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            // no deserts in oceans.
            if (!_location.AreaRegion.Landmass)
                return false;

            return true;
        }

        public override int Weight(World current_world, Deity creator, int current_age)
        {
            int weight = base.Weight(current_world, creator, current_age);

            if (creator.Domains.Contains(Domain.Drought))
                weight += Constants.WEIGHT_MANY_CHANGE;

            if (creator.Domains.Contains(Domain.Water))
                weight -= Constants.WEIGHT_MANY_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        public CreateDesert(Area location) : base(location)
        {
            Name = "Create Desert in Area " + location.Name;
        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            Desert desert = new Desert(Constants.Names.GetName("deserts"), _location, creator);

            int chance = Main.Constants.RND.Next(100);
            switch (_location.AreaClimate)
            {
                case Climate.SubArctic:
                    if (chance < 50)
                        desert.BiomeType = BiomeType.ColdDesert;
                    else
                        desert.BiomeType = BiomeType.Tundra;
                    break;
                case Climate.Temperate:
                    if (chance < 50)
                        desert.BiomeType = BiomeType.ColdDesert;
                    else
                        desert.BiomeType = BiomeType.HotDesert;
                    break;
                case Climate.SubTropical:
                    desert.BiomeType = BiomeType.HotDesert;
                    break;
                case Climate.Tropical:
                    desert.BiomeType = BiomeType.HotDesert;
                    break;
            }

            // Add forest to the area lists.
            _location.Deserts.Add(desert);
            _location.Terrain.Add(desert);
            _location.UnclaimedTerritory.Add(desert);

            // Add forest to the deity.
            creator.Creations.Add(desert);
            creator.LastCreation = desert;
        }
    }
}
