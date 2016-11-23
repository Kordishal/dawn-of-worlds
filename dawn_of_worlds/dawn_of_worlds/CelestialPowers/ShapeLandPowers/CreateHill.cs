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
    class CreateHill : ShapeLand
    {

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            // Requires moutainrange where mountain can be added.
            if (_location.HillRanges != null)
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
            Hill hill = new Hill("PlaceHolder", _location, creator);

            int chance = Main.Constants.RND.Next(100);
            switch (_location.AreaClimate)
            {
                case Climate.Arctic:
                    hill.BiomeType = BiomeType.Tundra;
                    break;
                case Climate.SubArctic:
                    if (chance < 33)
                        hill.BiomeType = BiomeType.Tundra;
                    else if (chance < 66)
                        hill.BiomeType = BiomeType.ColdDesert;
                    else
                        hill.BiomeType = BiomeType.BorealForest;
                    break;
                case Climate.Temperate:
                    if (chance < 25)
                        hill.BiomeType = BiomeType.TemperateGrassland;
                    else if (chance < 50)
                        hill.BiomeType = BiomeType.ColdDesert;
                    else if (chance < 75)
                        hill.BiomeType = BiomeType.HotDesert;
                    else
                        hill.BiomeType = BiomeType.TemperateDeciduousForest;
                    break;
                case Climate.SubTropical:
                    if (chance < 33)
                        hill.BiomeType = BiomeType.TropicalGrassland;
                    else if (chance < 66)
                        hill.BiomeType = BiomeType.HotDesert;
                    else
                        hill.BiomeType = BiomeType.TropicalRainforest;
                    break;
                case Climate.Tropical:
                    if (chance < 33)
                        hill.BiomeType = BiomeType.TropicalGrassland;
                    else if (chance < 66)
                        hill.BiomeType = BiomeType.HotDesert;
                    else
                        hill.BiomeType = BiomeType.TropicalRainforest;
                    break;
            }

            hill.Name = "Hill with " + Enum.GetName(typeof(BiomeType), hill.BiomeType);

            // Add mountain to area lists.
            _location.HillRanges.Hills.Add(hill);
            _location.Terrain.Add(hill);
            _location.UnclaimedTerritory.Add(hill);

            // Add mountainrange to mountain
            hill.Range = _location.HillRanges;

            // Add mountain to deity lists
            creator.Creations.Add(hill);
            creator.LastCreation = hill;
    }


    public CreateHill(Area location) : base (location)
    {
        Name = "Create Hill in Area " + location.Name;
    }
}
}
