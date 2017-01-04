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
    class CreateMountainRange : ShapeLand
    {
        protected override void initialize()
        {
            base.initialize();
            Name = "Create Mountain Range";
            Tags = new List<CreationTag>() { CreationTag.Creation, CreationTag.Mountainous, CreationTag.Earth };
        }

        public override bool Precondition(Deity creator)
        {
            base.Precondition(creator);
            // needs a possible terrain in the area.
            if (candidate_provinces().Count == 0)
                return false;

            return true;
        }

        public override void Effect(Deity creator)
        {
            List<WeightedObjects<Province>> mountain_range_locations = candidate_provinces();
            Province mountain_range_location = WeightedObjects<Province>.ChooseRandomObject(mountain_range_locations);
            MountainRange mountain_range = new MountainRange(Constants.Names.GetName("mountain_ranges"), mountain_range_location, creator);
            mountain_range_location.Type = TerrainType.MountainRange;
            mountain_range_location.PrimaryTerrainFeature = mountain_range;

            int chance = Constants.Random.Next(100);
            switch (mountain_range_location.LocalClimate)
            {
                case Climate.Arctic:
                    mountain_range.BiomeType = BiomeType.PolarDesert;
                    break;
                case Climate.SubArctic:
                    if (chance < 50)
                        mountain_range.BiomeType = BiomeType.Tundra;
                    else
                        mountain_range.BiomeType = BiomeType.BorealForest;
                    break;
                case Climate.Temperate:
                    if (chance < 50)
                        mountain_range.BiomeType = BiomeType.TemperateGrassland;
                    else
                        mountain_range.BiomeType = BiomeType.TemperateDeciduousForest;
                    break;
                case Climate.SubTropical:
                    if (chance < 50)
                        mountain_range.BiomeType = BiomeType.TropicalGrassland;
                    else
                        mountain_range.BiomeType = BiomeType.TropicalDryForest;
                    break;
                case Climate.Tropical:
                    if (chance < 50)
                        mountain_range.BiomeType = BiomeType.TropicalGrassland;
                    else
                        mountain_range.BiomeType = BiomeType.TropicalRainforest;
                    break;
            }


            mountain_range_location.isDefault = false;
            creator.TerrainFeatures.Add(mountain_range);
            creator.LastCreation = mountain_range;

            Program.WorldHistory.AddRecord(mountain_range, mountain_range.printTerrainFeature);
        }

        public CreateMountainRange(Area location) : base (location) { }

        private List<WeightedObjects<Province>> candidate_provinces()
        {
            List<WeightedObjects<Province>> province_list = new List<WeightedObjects<Province>>();
            foreach (Province province in _location.Provinces)
            {
                if (province.isDefault && province.Type == TerrainType.Plain)
                {
                    WeightedObjects<Province> weighted_province = new WeightedObjects<Province>(province);
                    weighted_province.Weight += 5; // Add so that each possible province has at least some weight as otherwise none will be chosen.

                    for (int i = 0; i < 8; i++)
                    {
                        SystemCoordinates coords = province.Coordinates;
                        coords = coords.GetNeighbour(i);

                        if (coords.isInTileGridBounds())
                        {
                            if (Program.World.ProvinceGrid[coords.X, coords.Y].Type == TerrainType.MountainRange)
                                weighted_province.Weight += 20;
                            if (Program.World.ProvinceGrid[coords.X, coords.Y].Type == TerrainType.HillRange)
                                weighted_province.Weight += 10;
                        }
                    }

                    province_list.Add(weighted_province);
                }
            }

            return province_list;
        }
    }
}
