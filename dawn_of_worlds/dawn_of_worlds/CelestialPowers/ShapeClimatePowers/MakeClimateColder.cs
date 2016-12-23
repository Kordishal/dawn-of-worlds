using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.CelestialPowers.ShapeClimatePowers
{
    class MakeClimateColder : ShapeClimate
    {
        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

            if (creator.Domains.Contains(Domain.Cold))
                weight += Constants.WEIGHT_MANY_CHANGE;

            if (creator.Domains.Contains(Domain.Wind))
                weight += Constants.WEIGHT_MANY_CHANGE;

            return weight >= 0 ? weight : 0;
        }

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

        public override bool Precondition(Deity creator)
        {
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

        public MakeClimateColder(Area location) : base(location)
        {
            Name = "Make Climate Colder in " + location.Name;
        }
    }
}
