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

        private List<Tile> candidate_terrain()
        {
            List<Tile> terrain_list = new List<Tile>();
            foreach (Tile terrain in _location.TerrainArea)
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
            // Pick a random terrain tile.
            List<Tile> hill_locations = candidate_terrain();
            Tile hill_location = hill_locations[Constants.Random.Next(hill_locations.Count)];
            Hill hill = new Hill("PlaceHolder", hill_location, creator);

            int chance = Constants.Random.Next(100);
            switch (_location.ClimateArea)
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

            hill.Name = Constants.Names.GetName("hills");

            // Add hill to hill range.
            ((HillRange)hill_location.PrimaryTerrainFeature).Hills.Add(hill);
            hill.Range = (HillRange)hill_location.PrimaryTerrainFeature;
            hill_location.UnclaimedTerritories.Add(hill);
            hill_location.UnclaimedTravelAreas.Add(hill);
            hill_location.UnclaimedHuntingGrounds.Add(hill);

            // Add mountain to deity lists
            creator.TerrainFeatures.Add(hill);
            creator.LastCreation = hill;
    }


    public CreateHill(Area location) : base (location)
    {
        Name = "Create Hill in Area " + location.Name;
    }
}
}
