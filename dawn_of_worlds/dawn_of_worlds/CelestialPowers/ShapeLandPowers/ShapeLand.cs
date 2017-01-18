using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Main;
using System;
using System.Collections.Generic;
using dawn_of_worlds.Effects;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    abstract class ShapeLand : Power
    {
        protected bool isPrimary { get; set; }
        protected Province SelectedProvince { get; set; }

        protected override void initialize()
        {
            Name = "Shape Land";
            BaseCost = new int[] { 2, 4, 6 };
            CostChange = 2;

            BaseWeight = new int[] { 20, 15, 10 };
            WeightChange = 5; 
        }

        public override bool Precondition(Deity creator)
        {
            SelectedProvince = selectProvince();

            if (SelectedProvince == null)
                return false;
            else
                return true;
        }

        virtual protected bool selectionModifier(Province province)
        {
            return true;
        }

        protected Province selectProvince()
        {
            List<WeightedObjects<Province>> weighted_provinces = new List<WeightedObjects<Province>>();
            foreach (Province province in Program.World.ProvinceGrid)
            {
                bool add_province = true;

                // Do not change the primary terrain feature more than once.
                if (!province.isDefault && isPrimary)
                    add_province = false;

                foreach (Modifier modifier in province.ProvincialModifiers)
                {
                    if (modifier.Forbids != null)
                        for (int i = 0; i < modifier.Forbids.Length; i++)
                            if (Tags.Contains(modifier.Forbids[i]))
                                add_province = false;                                    
                }

                add_province = selectionModifier(province);

                if (add_province)
                    weighted_provinces.Add(new WeightedObjects<Province>(province));
            }

            foreach(WeightedObjects<Province> weighted_province in weighted_provinces)
            {
                weighted_province.Weight += 5;

                foreach(Modifier modifier in weighted_province.Object.ProvincialModifiers)
                    if (modifier.IncreasesWeight != null)
                        for (int i = 0; i <modifier.IncreasesWeight.Length; i++)
                            if (Tags.Contains(modifier.IncreasesWeight[i]))
                                weighted_province.Weight += WeightChange;

                // Primary terrain features are more likely to appear next to each other.
                if (isPrimary)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        SystemCoordinates coords = weighted_province.Object.Coordinates;
                        coords = coords.GetNeighbour(i);

                        if (coords.isInTileGridBounds())
                        {
                            if (Program.World.ProvinceGrid[coords.X, coords.Y].PrimaryTerrainFeature.GetType() == weighted_province.Object.PrimaryTerrainFeature.GetType())
                                weighted_province.Weight += WeightChange;
                        }
                    }
                }

            }

            return WeightedObjects<Province>.ChooseRandomObject(weighted_provinces);
        }

        public ShapeLand()
        {
            initialize();
        }
    }
}
