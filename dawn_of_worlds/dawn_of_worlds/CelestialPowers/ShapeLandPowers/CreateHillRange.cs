using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;
using dawn_of_worlds.Effects;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    class CreateHillRange : ShapeLand
    {
        protected override void initialize()
        {
            base.initialize();
            Name = "Create Hill Range";
            isPrimary = true;
            Tags = new List<CreationTag>() { CreationTag.Creation, CreationTag.Hilly, CreationTag.Earth };
        }

        public override int Effect(Deity creator)
        {
            HillRange hill_range = new HillRange(Program.GenerateNames.GetName("hill_range_names"), SelectedProvince, creator);
            SelectedProvince.Type = TerrainType.HillRange;
            SelectedProvince.PrimaryTerrainFeature = hill_range;
            SelectedProvince.isDefault = false;
            creator.TerrainFeatures.Add(hill_range);
            creator.LastCreation = hill_range;

            int chance = Constants.Random.Next(100);
            switch (SelectedProvince.LocalClimate)
            {
                case Climate.Arctic:
                    hill_range.BiomeType = BiomeType.PolarDesert;
                    break;
                case Climate.SubArctic:
                    if (chance < 50)
                        hill_range.BiomeType = BiomeType.Tundra;
                    else
                        hill_range.BiomeType = BiomeType.BorealForest;
                    break;
                case Climate.Temperate:
                    if (chance < 50)
                        hill_range.BiomeType = BiomeType.TemperateGrassland;
                    else
                        hill_range.BiomeType = BiomeType.TemperateDeciduousForest;
                    break;
                case Climate.SubTropical:
                    if (chance < 50)
                        hill_range.BiomeType = BiomeType.TropicalGrassland;
                    else
                        hill_range.BiomeType = BiomeType.TropicalDryForest;
                    break;
                case Climate.Tropical:
                    if (chance < 50)
                        hill_range.BiomeType = BiomeType.TropicalGrassland;
                    else
                        hill_range.BiomeType = BiomeType.TropicalRainforest;
                    break;
            }

            Program.WorldHistory.AddRecord(hill_range, hill_range.printTerrainFeature);

            return 0;
        }
    }
}
