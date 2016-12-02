using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    class CreateHillRange : ShapeLand
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

            if (creator.Domains.Contains(Domain.Earth))
                weight += Constants.WEIGHT_MANY_CHANGE;

            return weight >= 0 ? weight : 0;
        }


        public override void Effect(Deity creator)
        {
            List<Tile> hill_range_locations = candidate_terrain();
            Tile hill_range_location = hill_range_locations[Constants.Random.Next(hill_range_locations.Count)];
            HillRange hill_range = new HillRange(Constants.Names.GetName("hill_ranges"), hill_range_location, creator);
            hill_range_location.Type = TerrainType.HillRange;
            hill_range_location.PrimaryTerrainFeature = hill_range;
            hill_range_location.isDefault = false;
            creator.TerrainFeatures.Add(hill_range);
            creator.LastCreation = hill_range;
        }

        public CreateHillRange(Area location) : base (location)
        {
            Name = "Create Hill Range in Area " + location.Name;
        }
    }
}
