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

        private List<WeightedObjects<Tile>> candidate_tiles()
        {
            List<WeightedObjects<Tile>> weighted_tiles = new List<WeightedObjects<Tile>>();
            foreach (Tile tile in _location.Tiles)
            {
                if (_climate != tile.LocalClimate)
                {
                    WeightedObjects<Tile> weighted_tile = new WeightedObjects<Tile>(tile);
                    weighted_tile.Weight += 5;

                    // If there is a neighbouring tile with the same climate it will be more likely to appear there.
                    for (int i = 0; i < 8; i++)
                    {
                        if (tile.Coordinates.GetNeighbour(i).isInTileGridBounds())
                        {
                            if (Program.World.getTile(tile.Coordinates.GetNeighbour(i)).LocalClimate == _climate)
                                weighted_tile.Weight += 10;
                        }
                    }
                    weighted_tiles.Add(weighted_tile);
                }
            }

            return weighted_tiles;
        }

        public override bool Precondition(Deity creator)
        {
            foreach (Tile tile in _location.Tiles)
                if (tile.LocalClimate != _climate)
                    return true;

            return false;
        }

        public override void Effect(Deity creator)
        {
            
        }
    }
}
