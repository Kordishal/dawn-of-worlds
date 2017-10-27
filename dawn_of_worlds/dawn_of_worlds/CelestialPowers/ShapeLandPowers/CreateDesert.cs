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
    class CreateDesert : ShapeLand
    {
        protected override void initialize()
        {
            base.initialize();
            Name = "Create Desert";
            isPrimary = true;
            Tags = new List<string>() { "creation", "plain", "dry" };
        }

        public override int Effect(Deity creator)
        {
            Desert desert = new Desert(Program.GenerateNames.GetName("desert_names"), SelectedProvince, creator);

            int chance = rnd.Next(100);
            switch (SelectedProvince.LocalClimate)
            {
                case Climate.Arctic:
                    desert.BiomeType = BiomeType.PolarDesert;
                    break;
                case Climate.SubArctic:
                    if (chance < 50)
                        desert.BiomeType = BiomeType.ColdDesert;
                    else
                        desert.BiomeType = BiomeType.Tundra;
                    break;
                case Climate.Temperate:
                    if (chance < 50)
                        desert.BiomeType = BiomeType.ColdDesert;
                    else
                        desert.BiomeType = BiomeType.HotDesert;
                    break;
                case Climate.SubTropical:
                    desert.BiomeType = BiomeType.HotDesert;
                    break;
                case Climate.Tropical:
                    desert.BiomeType = BiomeType.HotDesert;
                    break;
            }

            SelectedProvince.PrimaryTerrainFeature = desert;
            SelectedProvince.isDefault = false;

            creator.TerrainFeatures.Add(desert);
            creator.LastCreation = desert;

            Program.WorldHistory.AddRecord(desert, desert.printTerrainFeature);

            return 0;
        }
    }
}
