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
    class MakeClimateColder : ShapeClimate
    {
        protected override void initialize()
        {
            base.initialize();
            Name = "Make Climate Colder";
            Tags = new List<CreationTag>() { CreationTag.Cold, CreationTag.Climate };
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

            // Set new climate
            switch (_chosen_location.LocalClimate)
            {
                case Climate.Tropical:
                    _chosen_location.LocalClimate = Climate.SubTropical;
                    break;
                case Climate.SubTropical:
                    _chosen_location.LocalClimate = Climate.Temperate;
                    break;
                case Climate.Temperate:
                    _chosen_location.LocalClimate = Climate.SubArctic;
                    break;
                case Climate.SubArctic:
                    _chosen_location.LocalClimate = Climate.Arctic;
                    break;
            }

            adjustTerrainFeatureBiomes();
        }

        public MakeClimateColder(Area location) : base(location) { initialize(); }

        private List<WeightedObjects<Province>> candidate_provinces()
        {
            List<WeightedObjects<Province>> weighted_provinces = new List<WeightedObjects<Province>>();
            foreach (Province province in _location.Provinces)
            {
                int[] climate_count = countClimateNeighbours(province);
                switch (province.LocalClimate)
                {
                    case Climate.Arctic: //  not possible to make colder
                        break;
                    case Climate.SubArctic:
                        if (climate_count[0] >= 2)
                        {
                            weighted_provinces.Add(new WeightedObjects<Province>(province));
                            weighted_provinces.Last().Weight += Constants.WEIGHT_STANDARD_CHANGE * climate_count[0];
                        }
                        break;
                    case Climate.Temperate:
                        if (climate_count[1] >= 2)
                        {
                            weighted_provinces.Add(new WeightedObjects<Province>(province));
                            weighted_provinces.Last().Weight += Constants.WEIGHT_STANDARD_CHANGE * climate_count[1];
                        }
                        break;
                    case Climate.SubTropical:
                        if (climate_count[2] >= 2)
                        {
                            weighted_provinces.Add(new WeightedObjects<Province>(province));
                            weighted_provinces.Last().Weight += Constants.WEIGHT_STANDARD_CHANGE * climate_count[2];
                        }
                        break;
                    case Climate.Tropical:
                        if (climate_count[3] >= 2)
                        {
                            weighted_provinces.Add(new WeightedObjects<Province>(province));
                            weighted_provinces.Last().Weight += Constants.WEIGHT_STANDARD_CHANGE * climate_count[3];
                        }
                        break;
                }
            }

            return weighted_provinces;
        }
    }
}
