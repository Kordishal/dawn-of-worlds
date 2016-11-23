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
    class CreateForest : ShapeLand
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


        public override void Effect(World current_world, Deity creator, int current_age)
        {                                
            Forest forest = new Forest("PlaceHolder", _location, creator);
            
            switch (_location.AreaClimate)
            {
                case Climate.SubArctic:
                    forest.BiomeType = BiomeType.BorealForest;
                    break;
                case Climate.Temperate:
                    forest.BiomeType = BiomeType.TemperateDeciduousForest;
                    break;
                case Climate.SubTropical:
                    forest.BiomeType = BiomeType.TropicalDryForest;
                    break;
                case Climate.Tropical:
                    forest.BiomeType = BiomeType.TropicalRainforest;
                    break;
            }

            forest.Name = Enum.GetName(typeof(BiomeType), forest.BiomeType) + _location.Name;

            // Add forest to the area lists.
            _location.Forests.Add(forest);
            _location.Terrain.Add(forest);
            _location.UnclaimedTerritory.Add(forest);

            // Add forest to the deity.
            creator.Creations.Add(forest);
            creator.LastCreation = forest;            
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

        public CreateForest(Area location) : base (location)
        {
            Name = "Create Forest in " + location.Name;
        }
    }
}
