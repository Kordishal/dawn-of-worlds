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
    class CreateForest : ShapeLand
    {
        protected override void initialize()
        {
            base.initialize();
            Name = "Create Forest";
            isPrimary = true;
            Tags = new List<CreationTag>() { CreationTag.Creation, CreationTag.Nature, CreationTag.Tree };
        }

        public override int Effect(Deity creator)
        {           
            Forest forest = new Forest("PlaceHolder", SelectedProvince, creator);
            
            switch (SelectedProvince.LocalClimate)
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
            SelectedProvince.PrimaryTerrainFeature = forest;
            SelectedProvince.isDefault = false;

            forest.Modifiers.NaturalDefenceValue += 1;

            // Add forest to the deity.
            creator.TerrainFeatures.Add(forest);
            creator.LastCreation = forest;

            forest.Name = Program.GenerateNames.GetName();
            Program.WorldHistory.AddRecord(forest, forest.printTerrainFeature);

            return 0;      
        }
    }
}
