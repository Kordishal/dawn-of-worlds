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
    class CreateCave : ShapeLand
    {
        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            // no caves in oceans.
            if (_location.Type == TerrainType.Ocean)
                return false;

            return true;
        }

        public override int Weight(World current_world, Deity creator, int current_age)
        {
            int weight = base.Weight(current_world, creator, current_age);

            if (creator.Domains.Contains(Domain.Earth))
                weight += Constants.WEIGHT_MANY_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        public CreateCave(Terrain location) : base(location)
        {
            Name = "Create Cave in Terrain " + location.Name;
        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            Cave cave = new Cave("PlaceHolder", _location, creator);

            int chance = Constants.RND.Next(100);
            switch (_location.Area.ClimateArea)
            {
                case Climate.SubArctic:
                    cave.BiomeType = BiomeType.Subterranean;
                    break;
                case Climate.Temperate:
                    cave.BiomeType = BiomeType.Subterranean;
                    break;
                case Climate.SubTropical:
                    cave.BiomeType = BiomeType.Subterranean;
                    break;
                case Climate.Tropical:
                    cave.BiomeType = BiomeType.Subterranean;
                    break;
            }

            cave.Name = Constants.Names.GetName("caves");
            _location.SecondaryTerrainFeatures.Add(cave);
            _location.UnclaimedTerritory.Add(cave);

            // Add forest to the deity.
            creator.TerrainFeatures.Add(cave);
            creator.LastCreation = cave;
        }
    }
}
