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
    class CreateMountain : ShapeLand
    {

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            // Requires moutainrange where mountain can be added.
            if (_location.MountainRanges != null)
                return true;

            return false;
        }

        public override int Weight(World current_world, Deity creator, int current_age)
        {
            int weight = base.Weight(current_world, creator, current_age);

            if (creator.Domains.Contains(Domain.Earth))
                weight += Constants.WEIGHT_MANY_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            Mountain mountain = new Mountain("PlaceHolder", _location, creator);

            int chance = Main.Constants.RND.Next(100);
            switch (_location.AreaClimate)
            {
                case Climate.Arctic:
                    mountain.BiomeType = BiomeType.Tundra;
                    break;
                case Climate.SubArctic:                   
                    if (chance < 50)
                        mountain.BiomeType = BiomeType.Tundra;
                    else 
                        mountain.BiomeType = BiomeType.BorealForest;
                    break;
                case Climate.Temperate:
                    if (chance < 50)
                        mountain.BiomeType = BiomeType.TemperateGrassland;
                    else
                        mountain.BiomeType = BiomeType.TemperateDeciduousForest;
                    break;
                case Climate.SubTropical:
                    if (chance < 50)
                        mountain.BiomeType = BiomeType.TropicalGrassland;
                    else
                        mountain.BiomeType = BiomeType.TropicalDryForest;
                    break;
                case Climate.Tropical:
                    if (chance < 50)
                        mountain.BiomeType = BiomeType.TropicalGrassland;
                    else
                        mountain.BiomeType = BiomeType.TropicalRainforest;
                    break;           
            }

            mountain.Name = "Mount Albert with " + Enum.GetName(typeof(BiomeType), mountain.BiomeType);

            // Add mountain to area lists.
            _location.MountainRanges.Mountains.Add(mountain);
            _location.Terrain.Add(mountain);
            _location.UnclaimedTerritory.Add(mountain);

            // Add mountainrange to mountain
            mountain.Range = _location.MountainRanges;

            // Add mountain to deity lists
            creator.Creations.Add(mountain);
            creator.LastCreation = mountain;           
        }


        public CreateMountain(Area location) : base (location)
        {
            Name = "Create Mountain in Area " + location.Name;
        }
    }
}
