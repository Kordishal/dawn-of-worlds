using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    class CreateHill : ShapeLand
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
            List<Province> terrain_list = new List<Province>();
            foreach (Province terrain in _location.Provinces)
            {
                if (terrain.Type == TerrainType.HillRange)
                    terrain_list.Add(terrain);
            }

            return terrain_list;
        }

        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

            if (creator.Domains.Contains(Domain.Earth))
                weight += Constants.WEIGHT_MANY_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        public override void Effect(Deity creator)
        {
            // Pick a random terrain province.
            List<Province> hill_locations = candidate_terrain();
            Province hill_location = hill_locations[Constants.Random.Next(hill_locations.Count)];
            Hill hill = new Hill("PlaceHolder", hill_location, creator);

            int chance = Constants.Random.Next(100);
            switch (hill_location.LocalClimate)
            {
                case Climate.Arctic:
                    hill.BiomeType = BiomeType.Tundra;
                    break;
                case Climate.SubArctic:
                    if (chance < 33)
                        hill.BiomeType = BiomeType.Tundra;
                    else if (chance < 66)
                        hill.BiomeType = BiomeType.ColdDesert;
                    else
                        hill.BiomeType = BiomeType.BorealForest;
                    break;
                case Climate.Temperate:
                    if (chance < 25)
                        hill.BiomeType = BiomeType.TemperateGrassland;
                    else if (chance < 50)
                        hill.BiomeType = BiomeType.ColdDesert;
                    else if (chance < 75)
                        hill.BiomeType = BiomeType.HotDesert;
                    else
                        hill.BiomeType = BiomeType.TemperateDeciduousForest;
                    break;
                case Climate.SubTropical:
                    if (chance < 33)
                        hill.BiomeType = BiomeType.TropicalGrassland;
                    else if (chance < 66)
                        hill.BiomeType = BiomeType.HotDesert;
                    else
                        hill.BiomeType = BiomeType.TropicalRainforest;
                    break;
                case Climate.Tropical:
                    if (chance < 33)
                        hill.BiomeType = BiomeType.TropicalGrassland;
                    else if (chance < 66)
                        hill.BiomeType = BiomeType.HotDesert;
                    else
                        hill.BiomeType = BiomeType.TropicalRainforest;
                    break;
            }

            hill.Name.Singular = Constants.Names.GetName("hills");

            // Add hill to hill range.
            ((HillRange)hill_location.PrimaryTerrainFeature).Hills.Add(hill);
            hill.Range = (HillRange)hill_location.PrimaryTerrainFeature;
            hill_location.SecondaryTerrainFeatures.Add(hill);

            // Add mountain to deity lists
            creator.TerrainFeatures.Add(hill);
            creator.LastCreation = hill;

            Program.WorldHistory.AddRecord(hill, hill.printTerrainFeature);
        }


    public CreateHill(Area location) : base (location)
    {
        Name = "Create Hill in Area " + location.Name;
    }
}
}
