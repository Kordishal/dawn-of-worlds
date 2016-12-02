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
    class CreateDesert : ShapeLand
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
                if (terrain.isDefault && terrain.Type == TerrainType.Plain)
                    terrain_list.Add(terrain);
            }

            return terrain_list;
        }

        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

            if (creator.Domains.Contains(Domain.Drought))
                weight += Constants.WEIGHT_MANY_CHANGE;

            if (creator.Domains.Contains(Domain.Water))
                weight -= Constants.WEIGHT_MANY_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        public CreateDesert(Area location) : base(location)
        {
            Name = "Create Desert in Area " + location.Name;
        }

        public override void Effect(Deity creator)
        {
            // Pick a random terrain tile.
            List<Tile> candidate_desert_locations = candidate_terrain();
            Tile desert_location = candidate_desert_locations[Constants.Random.Next(candidate_desert_locations.Count)];

            Desert desert = new Desert(Constants.Names.GetName("deserts"), desert_location, creator);

            int chance = Constants.Random.Next(100);
            switch (_location.ClimateArea)
            {
                case Climate.SubArctic:
                    if (chance < 50)
                        desert.BiomeType = BiomeType.ColdDesert;
                    else
                        desert.BiomeType = BiomeType.Tundra;
                    break;
                case Climate.Temperate:
                    if (chance < 50)
                        desert.BiomeType = BiomeType.ColdDesert;
                    else
                        desert.BiomeType = BiomeType.HotDesert;
                    break;
                case Climate.SubTropical:
                    desert.BiomeType = BiomeType.HotDesert;
                    break;
                case Climate.Tropical:
                    desert.BiomeType = BiomeType.HotDesert;
                    break;
            }

            desert_location.PrimaryTerrainFeature = desert;
            desert_location.UnclaimedTerritories.Add(desert);
            desert_location.UnclaimedTravelAreas.Add(desert);
            desert_location.UnclaimedHuntingGrounds.Add(desert);
            desert_location.isDefault = false;

            // Add forest to the deity.
            creator.TerrainFeatures.Add(desert);
            creator.LastCreation = desert;
        }
    }
}
