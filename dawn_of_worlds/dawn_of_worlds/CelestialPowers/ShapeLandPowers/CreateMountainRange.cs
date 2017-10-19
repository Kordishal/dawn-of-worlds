using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;
using dawn_of_worlds.Effects;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    class CreateMountainRange : ShapeLand
    {
        protected override void initialize()
        {
            base.initialize();
            Name = "Create Mountain Range";
            Tags = new List<CreationTag>() { CreationTag.Creation, CreationTag.Mountainous, CreationTag.Earth };
        }

        public override int Effect(Deity creator)
        {
            MountainRange mountain_range = new MountainRange(Program.GenerateNames.GetName("mountain_range_names"), SelectedProvince, creator);
            SelectedProvince.Type = TerrainType.MountainRange;
            SelectedProvince.PrimaryTerrainFeature = mountain_range;
        
            int chance = Constants.Random.Next(100);
            switch (SelectedProvince.LocalClimate)
            {
                case Climate.Arctic:
                    mountain_range.BiomeType = BiomeType.PolarDesert;
                    break;
                case Climate.SubArctic:
                    if (chance < 50)
                        mountain_range.BiomeType = BiomeType.Tundra;
                    else
                        mountain_range.BiomeType = BiomeType.BorealForest;
                    break;
                case Climate.Temperate:
                    if (chance < 50)
                        mountain_range.BiomeType = BiomeType.TemperateGrassland;
                    else
                        mountain_range.BiomeType = BiomeType.TemperateDeciduousForest;
                    break;
                case Climate.SubTropical:
                    if (chance < 50)
                        mountain_range.BiomeType = BiomeType.TropicalGrassland;
                    else
                        mountain_range.BiomeType = BiomeType.TropicalDryForest;
                    break;
                case Climate.Tropical:
                    if (chance < 50)
                        mountain_range.BiomeType = BiomeType.TropicalGrassland;
                    else
                        mountain_range.BiomeType = BiomeType.TropicalRainforest;
                    break;
            }


            SelectedProvince.isDefault = false;
            creator.TerrainFeatures.Add(mountain_range);
            creator.LastCreation = mountain_range;

            Program.WorldHistory.AddRecord(mountain_range, mountain_range.printTerrainFeature);

            return 0;
        }
    }
}
