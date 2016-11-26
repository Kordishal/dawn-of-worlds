using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    class CreateGrassland : ShapeLand
    {

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            // no grasslands in oceans.
            if (_location.Type == TerrainType.Ocean)
                return false;

            // Can't create grasslands on mountain ranges & hill ranges. Each hill/mountain has a built in biome type.
            if (_location.Type == TerrainType.MountainRange || _location.Type == TerrainType.HillRange)
                return false;

            return true;
        }

        public override int Weight(World current_world, Deity creator, int current_age)
        {
            int weight = base.Weight(current_world, creator, current_age);

            if (creator.Domains.Contains(Domain.Nature))
                weight += Constants.WEIGHT_MANY_CHANGE;

            if (creator.Domains.Contains(Domain.Drought))
                weight -= Constants.WEIGHT_MANY_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        public CreateGrassland(Terrain location) : base(location)
        {
            Name = "Create Grassland in Terrain " + location.Name;
        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            Grassland grassland = new Grassland("PlaceHolder", _location, creator);

            switch (_location.Area.ClimateArea)
            {
                case Climate.Arctic:
                    grassland.BiomeType = BiomeType.Tundra;
                    break;
                case Climate.SubArctic:
                    grassland.BiomeType = BiomeType.Tundra;
                    break;
                case Climate.Temperate:
                    grassland.BiomeType = BiomeType.TemperateGrassland;
                    break;
                case Climate.SubTropical:
                    grassland.BiomeType = BiomeType.TropicalGrassland;
                    break;
                case Climate.Tropical:
                    grassland.BiomeType = BiomeType.TropicalGrassland;
                    break;
            }

            grassland.Name = Constants.Names.GetName("grasslands");
            _location.PrimaryTerrainFeature = grassland;
            _location.UnclaimedTerritory.Add(grassland);

            creator.TerrainFeatures.Add(grassland);
            creator.LastCreation = grassland;
        }
    }
}
