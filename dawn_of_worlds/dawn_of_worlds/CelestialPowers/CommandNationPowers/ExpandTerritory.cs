using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Geography;

namespace dawn_of_worlds.CelestialPowers.CommandNationPowers
{
    class ExpandTerritory : CommandNation
    {
        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            foreach (Terrain terrain in _commanded_nation.TerrainTerritory)
            {
                if (terrain.UnclaimedTerritory.Count > 0)
                    return true;
            }

            // Checks whether there is a neighbour terrain where we can find an unclaimed territory
            foreach (Terrain terrain in _commanded_nation.TerrainTerritory)
            {
                for (int i = 0; i < 8; i++)
                {
                    SystemCoordinates coords = terrain.Coordinates.GetNeighbour(i);

                    if (coords.isInTerrainGridBounds())
                    {
                        if (!_commanded_nation.TerrainTerritory.Exists(x => x.Equals(current_world.TerrainGrid[coords.X, coords.Y])) &&
                            current_world.TerrainGrid[coords.X, coords.Y].UnclaimedTerritory.Count > 0)
                            return true;
                    }
                }
            }


            return false;
        }

        public override int Weight(World current_world, Deity creator, int current_age)
        {
            int weight = base.Weight(current_world, creator, current_age);

            if (creator.Domains.Contains(Domain.Conquest))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            return weight >= 0 ? weight : 0;
        }


        public override void Effect(World current_world, Deity creator, int current_age)
        {
            List<TerrainFeatures> unclaimed_territory = new List<TerrainFeatures>();

            foreach (Terrain terrain in _commanded_nation.TerrainTerritory)
            {
                unclaimed_territory.AddRange(terrain.UnclaimedTerritory);
            }

            // Once no unclaimed space is left in a terrain the neighbouring terrains are chosen.
            if (unclaimed_territory.Count == 0)
            {
                List<Terrain> neighbouring_terrains = new List<Terrain>();
                foreach (Terrain terrain in _commanded_nation.TerrainTerritory)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        SystemCoordinates coords = terrain.Coordinates.GetNeighbour(i);

                        if (coords.isInTerrainGridBounds())
                        {
                            // puts together a list of all neighbouring terrains with 
                            // unclaimed territory, which are not already in the territory list.
                            if (!_commanded_nation.TerrainTerritory.Contains(current_world.TerrainGrid[coords.X, coords.Y]) &&
                                current_world.TerrainGrid[coords.X, coords.Y].UnclaimedTerritory.Count > 0)
                                neighbouring_terrains.Add(current_world.TerrainGrid[coords.X, coords.Y]);
                        }
                    }
                }

                foreach (Terrain terrain in neighbouring_terrains)
                {
                    unclaimed_territory.AddRange(terrain.UnclaimedTerritory);
                }
            }

            TerrainFeatures new_territory = unclaimed_territory[Constants.RND.Next(unclaimed_territory.Count)];
            _commanded_nation.Territory.Add(new_territory);
            new_territory.Owner = _commanded_nation;

            if (!_commanded_nation.TerrainTerritory.Contains(new_territory.Location))
                _commanded_nation.TerrainTerritory.Add(new_territory.Location);

            foreach (Terrain terrain in _commanded_nation.TerrainTerritory)
            {
                terrain.UnclaimedTerritory.Remove(new_territory);
            }

            creator.LastCreation = new_territory;
        }


        public ExpandTerritory(Nation commanded_nation) : base(commanded_nation)
        {
            Name = "Expand National Territory: " + commanded_nation.Name;
        }
    }
}
