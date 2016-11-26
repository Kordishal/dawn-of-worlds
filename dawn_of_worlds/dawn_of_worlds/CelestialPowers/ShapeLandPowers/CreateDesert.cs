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
            if (_location.Type == TerrainType.Ocean)
                return false;
            // not on mountains or hills
            if (_location.Type == TerrainType.HillRange || _location.Type == TerrainType.MountainRange)
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

        public CreateDesert(Terrain location) : base(location)
        {
            Name = "Create Desert in Terrain " + location.Name;
        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            Desert desert = new Desert(Constants.Names.GetName("deserts"), _location, creator);

            int chance = Constants.RND.Next(100);
            switch (_location.Area.ClimateArea)
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

            _location.PrimaryTerrainFeature = desert;
            _location.UnclaimedTerritory.Add(desert);

            // Add forest to the deity.
            creator.TerrainFeatures.Add(desert);
            creator.LastCreation = desert;
        }
    }
}
