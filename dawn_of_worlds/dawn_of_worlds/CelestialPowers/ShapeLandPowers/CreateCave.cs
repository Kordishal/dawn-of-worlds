using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;
using dawn_of_worlds.Effects;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    class CreateCave : ShapeLand
    {
        protected override void initialize()
        {
            base.initialize();
            Name = "Create Cave";
            isPrimary = false;
            Tags = new List<CreationTag>() { CreationTag.Subterranean, CreationTag.Earth };
        }

        public override int Effect(Deity creator)
        {
            Cave cave = new Cave("PlaceHolder", SelectedProvince, creator);

            int chance = Constants.Random.Next(100);
            cave.BiomeType = BiomeType.Subterranean;

            cave.Name.Singular = Program.GenerateNames.GetName("cave_names");
            SelectedProvince.SecondaryTerrainFeatures.Add(cave);

            // Caves offer some protection for defenders.
            cave.Modifiers.NaturalDefenceValue += 1;
            creator.TerrainFeatures.Add(cave);
            creator.LastCreation = cave;
            Program.WorldHistory.AddRecord(cave, cave.printTerrainFeature);

            return 0;
        }
    }
}
