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

namespace dawn_of_worlds.CelestialPowers.ShapeClimatePowers
{
    class MakeClimateWarmer : ShapeClimate
    {
        protected override void initialize()
        {
            base.initialize();
            Name = "Make Climate Warmer";
            Tags = new List<CreationTag>() { CreationTag.Heat, CreationTag.Fire, CreationTag.Climate };
        }

        public override bool Precondition(Deity creator)
        {
            base.Precondition(creator);
            if (candidate_provinces().Count > 0)
                return true;

            return false;
        }

        public override void Effect(Deity creator)
        {
            List<WeightedObjects<Province>> provinces = candidate_provinces();
            _chosen_location = WeightedObjects<Province>.ChooseRandomObject(provinces);

            // change climate.
            switch (_chosen_location.LocalClimate)
            {
                case Climate.Arctic:
                    _chosen_location.LocalClimate = Climate.SubArctic;
                    break;
                case Climate.SubArctic:
                    _chosen_location.LocalClimate = Climate.Temperate;
                    break;
                case Climate.Temperate:
                    _chosen_location.LocalClimate = Climate.SubTropical;
                    break;
                case Climate.SubTropical:
                    _chosen_location.LocalClimate = Climate.Tropical;
                    break;
            }

            adjustTerrainFeatureBiomes();
        }

        public MakeClimateWarmer() { initialize(); }

        private List<WeightedObjects<Province>> candidate_provinces()
        {
            List<WeightedObjects<Province>> weighted_provinces = new List<WeightedObjects<Province>>();
            foreach (Province province in Program.World.ProvinceGrid)
            {
                int[] climate_count = countClimateNeighbours(province);
                switch (province.LocalClimate)
                {
                    case Climate.Arctic:
                        if (climate_count[1] >= 2)
                        {
                            weighted_provinces.Add(new WeightedObjects<Province>(province));
                            weighted_provinces.Last().Weight += Constants.WEIGHT_STANDARD_CHANGE * climate_count[1];
                        }
                        break;
                    case Climate.SubArctic:
                        if (climate_count[2] >= 2)
                        {
                            weighted_provinces.Add(new WeightedObjects<Province>(province));
                            weighted_provinces.Last().Weight += Constants.WEIGHT_STANDARD_CHANGE * climate_count[2];
                        }
                        break;
                    case Climate.Temperate:
                        if (climate_count[3] >= 2)
                        {
                            weighted_provinces.Add(new WeightedObjects<Province>(province));
                            weighted_provinces.Last().Weight += Constants.WEIGHT_STANDARD_CHANGE * climate_count[3];
                        }
                        break;
                    case Climate.SubTropical:
                        if (climate_count[4] >= 2)
                        {
                            weighted_provinces.Add(new WeightedObjects<Province>(province));
                            weighted_provinces.Last().Weight += Constants.WEIGHT_STANDARD_CHANGE * climate_count[4];
                        }
                        break;
                    case Climate.Tropical:// not possible to make warmer
                        break;
                }
            }

            return weighted_provinces;
        }
    }
}
