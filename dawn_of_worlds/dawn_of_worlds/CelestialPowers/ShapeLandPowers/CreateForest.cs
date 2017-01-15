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
            Tags = new List<CreationTag>() { CreationTag.Creation, CreationTag.Nature };
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
            List<Province> forest_locations = candidate_terrain();
            Province forest_location = forest_locations[Constants.Random.Next(forest_locations.Count)];           
                           
            Forest forest = new Forest("PlaceHolder", forest_location, creator);
            
            switch (forest_location.LocalClimate)
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
            forest_location.PrimaryTerrainFeature = forest;
            forest_location.isDefault = false;

            forest.Modifiers.NaturalDefenceValue += 1;

            // Add forest to the deity.
            creator.TerrainFeatures.Add(forest);
            creator.LastCreation = forest;

            forest.Name.Singular = Constants.Names.GetForestName(forest);
            Program.WorldHistory.AddRecord(forest, forest.printTerrainFeature);        
        }

        public CreateForest(Area location) : base(location) { }

        private List<Province> candidate_terrain()
        {
            List<Province> province_list = new List<Province>();
            foreach (Province province in _location.Provinces)
            {
                if (province.isDefault && province.Type == TerrainType.Plain)
                    if (province.LocalClimate != Climate.Arctic)
                        province_list.Add(province);
            }

            return province_list;
        }
    }
}
