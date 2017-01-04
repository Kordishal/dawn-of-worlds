using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;
using dawn_of_worlds.Modifiers;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    class CreateLake : ShapeLand
    {
        protected override void initialize()
        {
            base.initialize();
            Name = "Create Lake";
            Tags = new List<CreationTag>() { CreationTag.Creation, CreationTag.Water };
        }

        public override bool Precondition(Deity creator)
        {
            base.Precondition(creator);
            // needs a possible terrain in the area.
            if (candidate_terrain().Count == 0)
                return false;

            return true;
        }

        public override void Effect(Deity creator)
        {
            List<Province> lake_locations = candidate_terrain();
            Province lake_location = lake_locations[Constants.Random.Next(lake_locations.Count)];

            // Create the lake
            Lake lake = new Lake(Constants.Names.GetName("lakes"), lake_location, creator);
            lake.BiomeType = BiomeType.PermanentFreshWaterLake;

            // Choose random river which the lake is connected to.
            List<TerrainFeatures> rivers = lake_location.SecondaryTerrainFeatures.FindAll(x => x.GetType() == typeof(River));
            River river = (River)rivers[Constants.Random.Next(rivers.Count)];

            river.ConnectedLakes.Add(lake);
            lake.SourceRivers.Add(river);
            lake.OutGoingRiver = river;

            lake_location.SecondaryTerrainFeatures.Add(lake);

            // Add lake to deity lists
            creator.TerrainFeatures.Add(lake);
            creator.LastCreation = lake;

            Program.WorldHistory.AddRecord(lake, lake.printTerrainFeature);
        }



        public CreateLake(Area location) : base(location) { }

        private List<Province> candidate_terrain()
        {
            List<Province> terrain_list = new List<Province>();
            foreach (Province terrain in _location.Provinces)
            {
                if (terrain.hasRivers)
                    terrain_list.Add(terrain);
            }

            return terrain_list;
        }
    }
}
