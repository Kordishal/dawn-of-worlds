using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Creations.Diplomacy;

namespace dawn_of_worlds.CelestialPowers.CommandNationPowers
{
    class ExpandTerritory : CommandNation
    {
        public override bool Precondition(Deity creator)
        {
            switch (_commanded_nation.Type)
            {
                case NationTypes.FeudalNation:
                case NationTypes.TribalNation:
                case NationTypes.LairTerritory:
                    foreach (Tile terrain in _commanded_nation.TerrainTerritory)
                    {
                        if (terrain.UnclaimedTerritories.Count > 0)
                            return true;
                    }
                    break;
                case NationTypes.HuntingGrounds:
                    foreach (Tile terrain in _commanded_nation.TerrainTerritory)
                    {
                        if (terrain.UnclaimedHuntingGrounds.Count > 0)
                            return true;
                    }
                    break;
                case NationTypes.NomadicTribe:
                    foreach (Tile terrain in _commanded_nation.TerrainTerritory)
                    {
                        if (terrain.UnclaimedTravelAreas.Count > 0)
                            return true;
                    }
                    break;
            }

            // Checks whether there is a neighbour terrain where we can find an unclaimed territory
            foreach (Tile terrain in _commanded_nation.TerrainTerritory)
            {
                for (int i = 0; i < 8; i++)
                {
                    SystemCoordinates coords = terrain.Coordinates.GetNeighbour(i);

                    if (coords.isInTerrainGridBounds())
                    {
                        switch (_commanded_nation.Type)
                        {
                            case NationTypes.FeudalNation:
                            case NationTypes.TribalNation:
                            case NationTypes.LairTerritory:
                                if (!_commanded_nation.TerrainTerritory.Exists(x => x.Equals(Program.World.TerrainGrid[coords.X, coords.Y])) 
                                    && Program.World.TerrainGrid[coords.X, coords.Y].UnclaimedTerritories.Count > 0)
                                    return true;
                                break;
                            case NationTypes.HuntingGrounds:
                                if (!_commanded_nation.TerrainTerritory.Exists(x => x.Equals(Program.World.TerrainGrid[coords.X, coords.Y])) 
                                    && Program.World.TerrainGrid[coords.X, coords.Y].UnclaimedHuntingGrounds.Count > 0)
                                    return true;
                                break;
                            case NationTypes.NomadicTribe:
                                if (!_commanded_nation.TerrainTerritory.Exists(x => x.Equals(Program.World.TerrainGrid[coords.X, coords.Y])) 
                                    && Program.World.TerrainGrid[coords.X, coords.Y].UnclaimedTravelAreas.Count > 0)
                                    return true;
                                break;
                        }

                    }
                }
            }


            return false;
        }

        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

            if (creator.Domains.Contains(Domain.Conquest))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            return weight >= 0 ? weight : 0;
        }


        public override void Effect(Deity creator)
        {
            List<TerrainFeatures> unclaimed_territory = new List<TerrainFeatures>();

            foreach (Tile terrain in _commanded_nation.TerrainTerritory)
            {
                switch (_commanded_nation.Type)
                {
                    case NationTypes.FeudalNation:
                    case NationTypes.TribalNation:
                    case NationTypes.LairTerritory:
                        unclaimed_territory.AddRange(terrain.UnclaimedTerritories);
                        break;
                    case NationTypes.HuntingGrounds:
                        unclaimed_territory.AddRange(terrain.UnclaimedHuntingGrounds);
                        break;
                    case NationTypes.NomadicTribe:
                        unclaimed_territory.AddRange(terrain.UnclaimedTravelAreas);
                        break;
                }
            }

            // Once no unclaimed space is left in a terrain the neighbouring terrains are chosen.
            if (unclaimed_territory.Count == 0)
            {
                List<Tile> neighbouring_terrains = new List<Tile>();
                foreach (Tile terrain in _commanded_nation.TerrainTerritory)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        SystemCoordinates coords = terrain.Coordinates.GetNeighbour(i);

                        if (coords.isInTerrainGridBounds())
                        {
                            // puts together a list of all neighbouring terrains with 
                            // unclaimed territory, which are not already in the territory list.
                            switch (_commanded_nation.Type)
                            {
                                case NationTypes.FeudalNation:
                                case NationTypes.TribalNation:
                                case NationTypes.LairTerritory:
                                    if (!_commanded_nation.TerrainTerritory.Contains(Program.World.TerrainGrid[coords.X, coords.Y]) 
                                        && Program.World.TerrainGrid[coords.X, coords.Y].UnclaimedTerritories.Count > 0)
                                        neighbouring_terrains.Add(Program.World.TerrainGrid[coords.X, coords.Y]);
                                    break;
                                case NationTypes.HuntingGrounds:
                                    if (!_commanded_nation.TerrainTerritory.Contains(Program.World.TerrainGrid[coords.X, coords.Y])
                                        && Program.World.TerrainGrid[coords.X, coords.Y].UnclaimedHuntingGrounds.Count > 0)
                                        neighbouring_terrains.Add(Program.World.TerrainGrid[coords.X, coords.Y]);
                                    break;
                                case NationTypes.NomadicTribe:
                                    if (!_commanded_nation.TerrainTerritory.Contains(Program.World.TerrainGrid[coords.X, coords.Y])
                                        && Program.World.TerrainGrid[coords.X, coords.Y].UnclaimedTravelAreas.Count > 0)
                                        neighbouring_terrains.Add(Program.World.TerrainGrid[coords.X, coords.Y]);
                                    break;
                            }
                        }
                    }
                }

                foreach (Tile terrain in neighbouring_terrains)
                {
                    switch (_commanded_nation.Type)
                    {
                        case NationTypes.FeudalNation:
                        case NationTypes.TribalNation:
                        case NationTypes.LairTerritory:
                            unclaimed_territory.AddRange(terrain.UnclaimedTerritories);
                            break;
                        case NationTypes.HuntingGrounds:
                            unclaimed_territory.AddRange(terrain.UnclaimedHuntingGrounds);
                            break;
                        case NationTypes.NomadicTribe:
                            unclaimed_territory.AddRange(terrain.UnclaimedTravelAreas);
                            break;
                    }
                }
            }

            TerrainFeatures new_territory = unclaimed_territory[Constants.Random.Next(unclaimed_territory.Count)];
            _commanded_nation.Territory.Add(new_territory);
            switch (_commanded_nation.Type)
            {
                case NationTypes.FeudalNation:
                case NationTypes.TribalNation:
                case NationTypes.LairTerritory:
                    new_territory.NationalTerritory = _commanded_nation;
                    break;
                case NationTypes.HuntingGrounds:
                    new_territory.HuntingGround = _commanded_nation;
                    break;
                case NationTypes.NomadicTribe:
                    new_territory.TraveledArea = _commanded_nation;
                    break;
            }

            if (!_commanded_nation.TerrainTerritory.Contains(new_territory.Location))
                _commanded_nation.TerrainTerritory.Add(new_territory.Location);

            foreach (Tile terrain in _commanded_nation.TerrainTerritory)
            {
                terrain.UnclaimedTerritories.Remove(new_territory);
                switch (_commanded_nation.Type)
                {
                    case NationTypes.FeudalNation:
                    case NationTypes.TribalNation:
                    case NationTypes.LairTerritory:
                        terrain.UnclaimedTerritories.Remove(new_territory);
                        break;
                    case NationTypes.HuntingGrounds:
                        terrain.UnclaimedHuntingGrounds.Remove(new_territory);
                        break;
                    case NationTypes.NomadicTribe:
                        terrain.UnclaimedTravelAreas.Remove(new_territory);
                        break;
                }
            }

            _commanded_nation.PossibleWarGoals.Add(new WarGoal(WarGoalType.TerritoryConquest));

            creator.LastCreation = new_territory;
        }


        public ExpandTerritory(Nation commanded_nation) : base(commanded_nation)
        {
            Name = "Expand National Territory: " + commanded_nation.Name;
        }
    }
}
