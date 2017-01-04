﻿using System;
using System.Collections.Generic;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Main;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Modifiers;

namespace dawn_of_worlds.CelestialPowers.ShapeClimatePowers
{
    class CreateSpecialClimate : ShapeClimate
    {
        private Climate _climate { get; set; }

        protected override void initialize()
        {
            base.initialize();
            Name = "Create Special Climate (" + _climate.ToString() + ")";
            Tags = new List<CreationTag>() { CreationTag.Climate };
            switch (_climate)
            {
                case Climate.Inferno:
                     Tags.AddRange(new List<CreationTag>() { CreationTag.Fire, CreationTag.Heat, CreationTag.Destruction });
                    break;
            }
        }

        public override bool Precondition(Deity creator)
        {
            base.Precondition(creator);
            foreach (Province province in _location.Provinces)
                if (province.LocalClimate != _climate)
                    return true;

            return false;
        }

        public override void Effect(Deity creator)
        {
            
        }

        public CreateSpecialClimate(Area location, Climate climate) : base(location)
        {
            _climate = climate;
            initialize();
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

    }
}
