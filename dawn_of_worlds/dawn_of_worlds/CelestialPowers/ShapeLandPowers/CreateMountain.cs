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

        public override bool Precondition(Deity creator)
        {
            // needs a possible terrain in the area.
            if (candidate_provinces().Count == 0)
                return false;

            return true;
        }

        private List<Province> candidate_provinces()
        {
            List<Province> province_list = new List<Province>();
            foreach (Province terrain in _location.Provinces)
            {
                if (terrain.Type == TerrainType.MountainRange)
                    province_list.Add(terrain);
            }

            return province_list;
        }

        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

            if (creator.Domains.Contains(Domain.Earth))
                weight += Constants.WEIGHT_MANY_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        public override void Effect(Deity creator)
        {
            List<Province> mountain_locations = candidate_provinces();
            Province mountain_location = mountain_locations[Constants.Random.Next(mountain_locations.Count)];
            Mountain mountain = new Mountain("PlaceHolder", mountain_location, creator);

            int chance = Constants.Random.Next(100);
            switch (mountain_location.LocalClimate)
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

            mountain.Name.Singular = Constants.Names.GetName("mountains");
            ((MountainRange)mountain_location.PrimaryTerrainFeature).Mountains.Add(mountain);
            mountain.Range = (MountainRange)mountain_location.PrimaryTerrainFeature;
            mountain_location.SecondaryTerrainFeatures.Add(mountain);

            mountain.Modifiers.NaturalDefenceValue += 3;
            switch (mountain.BiomeType)
            {
                case BiomeType.BorealForest:
                case BiomeType.TemperateDeciduousForest:
                case BiomeType.TropicalDryForest:
                case BiomeType.TropicalRainforest:
                    mountain.Modifiers.NaturalDefenceValue += 1;
                    break;
                default:
                    break;
            }

            creator.TerrainFeatures.Add(mountain);
            creator.LastCreation = mountain;

            Program.WorldHistory.AddRecord(mountain, mountain.printTerrainFeature);
        }


        public CreateMountain(Area location) : base (location)
        {
            Name = "Create Mountain in Area " + location.Name;
        }
    }
}
