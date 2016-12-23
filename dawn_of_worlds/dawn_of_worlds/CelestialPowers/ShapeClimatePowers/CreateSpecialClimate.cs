using System;
using System.Collections.Generic;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Main;
using dawn_of_worlds.WorldClasses;

namespace dawn_of_worlds.CelestialPowers.ShapeClimatePowers
{
    class CreateSpecialClimate : ShapeClimate
    {
        private Climate _climate { get; set; }

        public CreateSpecialClimate(Area location, Climate climate) : base(location)
        {
            Name = "Create Special Climate (" + climate.ToString() + ")";
            _climate = climate;
        }

        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

            if (_climate == Climate.Inferno)
            {
                if (creator.Domains.Contains(Domain.Heat) || creator.Domains.Contains(Domain.Fire))
                    weight += Constants.WEIGHT_MANY_CHANGE * 2;
                else
                    weight = 0;
            }

            return weight >= 0 ? weight : 0;
        }

        private List<WeightedObjects<Province>> candidate_provinces()
        {
            List<WeightedObjects<Province>> weighted_provinces = new List<WeightedObjects<Province>>();
            foreach (Province province in _location.Provinces)
            {
                if (_climate != province.LocalClimate)
                {
                    WeightedObjects<Province> weighted_province = new WeightedObjects<Province>(province);
                    weighted_province.Weight += 5;

                    // If there is a neighbouring province with the same climate it will be more likely to appear there.
                    for (int i = 0; i < 8; i++)
                    {
                        if (province.Coordinates.GetNeighbour(i).isInTileGridBounds())
                        {
                            if (Program.World.getProvince(province.Coordinates.GetNeighbour(i)).LocalClimate == _climate)
                                weighted_province.Weight += 10;
                        }
                    }
                    weighted_provinces.Add(weighted_province);
                }
            }

            return weighted_provinces;
        }

        public override bool Precondition(Deity creator)
        {
            foreach (Province province in _location.Provinces)
                if (province.LocalClimate != _climate)
                    return true;

            return false;
        }

        public override void Effect(Deity creator)
        {
            
        }
    }
}
