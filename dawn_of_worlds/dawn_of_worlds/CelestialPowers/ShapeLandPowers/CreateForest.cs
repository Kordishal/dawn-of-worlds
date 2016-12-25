using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    class CreateForest : ShapeLand
    {
        public override bool Precondition(Deity creator)
        {

            // needs a possible terrain in the area.
            if (candidate_terrain().Count == 0)
                return false;

            return true;
        }

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

        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

            if (creator.Domains.Contains(Domain.Nature))
                weight += Constants.WEIGHT_MANY_CHANGE;

            if (creator.Domains.Contains(Domain.Drought))
                weight -= Constants.WEIGHT_MANY_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        public CreateForest(Area location) : base (location)
        {
            Name = "Create Forest in Area " + location.Name;
        }
    }
}
