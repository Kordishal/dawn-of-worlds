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
    class CreateLake : ShapeLand
    {
        protected override void initialize()
        {
            base.initialize();
            Name = "Create Lake";
            isPrimary = false;
            Tags = new List<CreationTag>() { CreationTag.Creation, CreationTag.Water };
        }

        protected override bool selectionModifier(Province province)
        {
            if (province.hasRivers)
                return true;
            else
                return false;
        }

        public override void Effect(Deity creator)
        {
            Lake lake = new Lake(Constants.Names.GetName("lakes"), SelectedProvince, creator);
            lake.BiomeType = BiomeType.PermanentFreshWaterLake;

            // Choose random river which the lake is connected to.
            List<TerrainFeatures> rivers = SelectedProvince.SecondaryTerrainFeatures.FindAll(x => x.GetType() == typeof(River));
            River river = (River)rivers[Constants.Random.Next(rivers.Count)];

            river.ConnectedLakes.Add(lake);
            lake.SourceRivers.Add(river);
            lake.OutGoingRiver = river;

            SelectedProvince.SecondaryTerrainFeatures.Add(lake);

            // Add lake to deity lists
            creator.TerrainFeatures.Add(lake);
            creator.LastCreation = lake;

            Program.WorldHistory.AddRecord(lake, lake.printTerrainFeature);
        }
    }
}
