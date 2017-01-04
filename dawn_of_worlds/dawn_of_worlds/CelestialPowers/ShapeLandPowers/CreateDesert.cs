using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;
using dawn_of_worlds.Modifiers;
using dawn_of_worlds.WorldClasses;
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
            Tags = new List<CreationTag>() { CreationTag.Creation, CreationTag.Plain, CreationTag.Dry };
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
            // Pick a random terrain province.
            List<Province> candidate_desert_locations = candidate_terrain();
            Province desert_location = candidate_desert_locations[Constants.Random.Next(candidate_desert_locations.Count)];

            Desert desert = new Desert(Constants.Names.GetName("deserts"), desert_location, creator);

            int chance = Constants.Random.Next(100);
            switch (desert_location.LocalClimate)
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

            desert_location.PrimaryTerrainFeature = desert;
            desert_location.isDefault = false;

            // Add forest to the deity.
            creator.TerrainFeatures.Add(desert);
            creator.LastCreation = desert;

            Program.WorldHistory.AddRecord(desert, desert.printTerrainFeature);
        }

        public CreateDesert(Area location) : base(location) { }


        private List<Province> candidate_terrain()
        {
            List<Province> terrain_list = new List<Province>();
            foreach (Province terrain in _location.Provinces)
            {
                if (terrain.isDefault && terrain.Type == TerrainType.Plain)
                    terrain_list.Add(terrain);
            }

            return terrain_list;
        }
    }
}
