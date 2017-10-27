using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;
using dawn_of_worlds.Effects;
using dawn_of_worlds.WorldModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    class CreateHill : ShapeLand
    {
        protected override void initialize()
        {
            base.initialize();
            Name = "Create Hill";
            isPrimary = false;
            Tags = new List<string>() { "creation", "earth" };
        }

        protected override bool selectionModifier(Province province)
        {
            if (province.Type == TerrainType.HillRange)
                return true;
            else
                return false;
        }

        public override int Effect(Deity creator)
        {
            Hill hill = new Hill("PlaceHolder", SelectedProvince, creator);

            int chance = rnd.Next(100);
            switch (SelectedProvince.LocalClimate)
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

            hill.Name = Program.GenerateNames.GetName("hill_names");

            // Add hill to hill range.
            ((HillRange)SelectedProvince.PrimaryTerrainFeature).Hills.Add(hill);
            hill.Range = (HillRange)SelectedProvince.PrimaryTerrainFeature;
            SelectedProvince.SecondaryTerrainFeatures.Add(hill);

            hill.Modifiers.NaturalDefenceValue += 2;
            switch (hill.BiomeType)
            {
                case BiomeType.BorealForest:
                case BiomeType.TemperateDeciduousForest:
                case BiomeType.TropicalDryForest:
                case BiomeType.TropicalRainforest:
                    hill.Modifiers.NaturalDefenceValue += 1;
                    break;
                default:
                    break;
            }

            // Add mountain to deity lists
            creator.TerrainFeatures.Add(hill);
            creator.LastCreation = hill;

            Program.WorldHistory.AddRecord(hill, hill.printTerrainFeature);

            return 0;
        }
    }
}
