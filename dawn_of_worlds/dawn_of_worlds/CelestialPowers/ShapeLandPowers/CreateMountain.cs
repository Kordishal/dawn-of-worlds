using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldModel;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;
using dawn_of_worlds.Effects;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    class CreateMountain : ShapeLand
    {

        protected override void initialize()
        {
            base.initialize();
            Name = "Create Mountain";
            isPrimary = false;
            Tags = new List<CreationTag>() { CreationTag.Creation, CreationTag.Earth };
        }

        protected override bool selectionModifier(Province province)
        {
            if (province.Type == TerrainType.MountainRange)
                return true;
            else
                return false;
        }
        public override int Effect(Deity creator)
        {
            Mountain mountain = new Mountain("PlaceHolder", SelectedProvince, creator);

            int chance = rnd.Next(100);
            switch (SelectedProvince.LocalClimate)
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

            mountain.Name = Program.GenerateNames.GetName("mountain_names");
            ((MountainRange)SelectedProvince.PrimaryTerrainFeature).Mountains.Add(mountain);
            mountain.Range = (MountainRange)SelectedProvince.PrimaryTerrainFeature;
            SelectedProvince.SecondaryTerrainFeatures.Add(mountain);

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

            return 0;
        }
    }
}
