﻿using System;
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
    class MakeClimateWarmer : ShapeClimate
    {
        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

            if (creator.Domains.Contains(Domain.Heat))
                weight += Constants.WEIGHT_MANY_CHANGE;

            if (creator.Domains.Contains(Domain.Fire))
                weight += Constants.WEIGHT_MANY_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        private List<WeightedObjects<Tile>> candidate_tiles()
        {
            List<WeightedObjects<Tile>> weighted_tiles = new List<WeightedObjects<Tile>>();
            foreach (Tile tile in _location.Tiles)
            {
                int[] climate_count = countClimateNeighbours(tile);
                switch (tile.LocalClimate)
                {
                    case Climate.Arctic:
                        if (climate_count[1] >= 2)
                        {
                            weighted_tiles.Add(new WeightedObjects<Tile>(tile));
                            weighted_tiles.Last().Weight += Constants.WEIGHT_STANDARD_CHANGE * climate_count[1];
                        }
                        break;
                    case Climate.SubArctic:
                        if (climate_count[2] >= 2)
                        {
                            weighted_tiles.Add(new WeightedObjects<Tile>(tile));
                            weighted_tiles.Last().Weight += Constants.WEIGHT_STANDARD_CHANGE * climate_count[2];
                        }
                        break;
                    case Climate.Temperate:
                        if (climate_count[3] >= 2)
                        {
                            weighted_tiles.Add(new WeightedObjects<Tile>(tile));
                            weighted_tiles.Last().Weight += Constants.WEIGHT_STANDARD_CHANGE * climate_count[3];
                        }
                        break;
                    case Climate.SubTropical:
                        if (climate_count[4] >= 2)
                        {
                            weighted_tiles.Add(new WeightedObjects<Tile>(tile));
                            weighted_tiles.Last().Weight += Constants.WEIGHT_STANDARD_CHANGE * climate_count[4];
                        }
                        break;
                    case Climate.Tropical:// not possible to make warmer
                        break;
                }
            }

            return weighted_tiles;
        }

        public override bool Precondition(Deity creator)
        {
            if (candidate_tiles().Count > 0)
                return true;

            return false;
        }

        public override void Effect(Deity creator)
        {
            List<WeightedObjects<Tile>> tiles = candidate_tiles();
            _chosen_location = WeightedObjects<Tile>.ChooseRandomObject(tiles);

            // change climate.
            switch (_chosen_location.LocalClimate)
            {
                case Climate.Arctic:
                    _chosen_location.LocalClimate = Climate.SubArctic;
                    break;
                case Climate.SubArctic:
                    _chosen_location.LocalClimate = Climate.Temperate;
                    break;
                case Climate.Temperate:
                    _chosen_location.LocalClimate = Climate.SubTropical;
                    break;
                case Climate.SubTropical:
                    _chosen_location.LocalClimate = Climate.Tropical;
                    break;
            }

            adjustTerrainFeatureBiomes();
        }

        public MakeClimateWarmer(Area location) : base (location)
        {
            Name = "Make Climate Warmer in " + location.Name;
        }
    }
}
