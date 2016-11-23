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
            // no forests in oceans.
            if (!_location.AreaRegion.Landmass)
                return false;

            // no forests in arctic regions
            if (_location.AreaClimate == Climate.Arctic)
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

        public CreateGrassland(Area location) : base(location)
        {
            Name = "Create Grassland in Area " + location.Name;
        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            Grassland grassland = new Grassland("PlaceHolder", _location, creator);

            switch (_location.AreaClimate)
            {
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

            grassland.Name = Enum.GetName(typeof(BiomeType), grassland.BiomeType) + _location.Name;

            // Add forest to the area lists.
            _location.Grasslands.Add(grassland);
            _location.Terrain.Add(grassland);
            _location.UnclaimedTerritory.Add(grassland);

            // Add forest to the deity.
            creator.Creations.Add(grassland);
            creator.LastCreation = grassland;
        }
    }
}
