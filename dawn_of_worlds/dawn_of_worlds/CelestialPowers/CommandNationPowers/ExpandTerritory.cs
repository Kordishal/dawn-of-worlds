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
                    foreach (Tile tile in _commanded_nation.Tiles)
                    {
                        if (tile.UnclaimedTerritories.Count > 0)
                            return true;
                    }
                    break;
                case NationTypes.HuntingGrounds:
                    foreach (Tile tile in _commanded_nation.Tiles)
                    {
                        if (tile.UnclaimedHuntingGrounds.Count > 0)
                            return true;
                    }
                    break;
                case NationTypes.NomadicTribe:
                    foreach (Tile tile in _commanded_nation.Tiles)
                    {
                        if (tile.UnclaimedTravelAreas.Count > 0)
                            return true;
                    }
                    break;
            }

            // Checks whether there is a neighbour terrain where we can find an unclaimed territory
            foreach (Tile tile in _commanded_nation.Tiles)
            {
                for (int i = 0; i < 8; i++)
                {
                    SystemCoordinates coords = tile.Coordinates.GetNeighbour(i);

                    if (coords.isInTileGridBounds())
                    {
                        switch (_commanded_nation.Type)
                        {
                            case NationTypes.FeudalNation:
                            case NationTypes.TribalNation:
                            case NationTypes.LairTerritory:
                                if (!_commanded_nation.Tiles.Exists(x => x.Equals(Program.World.TileGrid[coords.X, coords.Y])) 
                                    && Program.World.TileGrid[coords.X, coords.Y].UnclaimedTerritories.Count > 0)
                                    return true;
                                break;
                            case NationTypes.HuntingGrounds:
                                if (!_commanded_nation.Tiles.Exists(x => x.Equals(Program.World.TileGrid[coords.X, coords.Y])) 
                                    && Program.World.TileGrid[coords.X, coords.Y].UnclaimedHuntingGrounds.Count > 0)
                                    return true;
                                break;
                            case NationTypes.NomadicTribe:
                                if (!_commanded_nation.Tiles.Exists(x => x.Equals(Program.World.TileGrid[coords.X, coords.Y])) 
                                    && Program.World.TileGrid[coords.X, coords.Y].UnclaimedTravelAreas.Count > 0)
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

            foreach (Tile tile in _commanded_nation.Tiles)
            {
                switch (_commanded_nation.Type)
                {
                    case NationTypes.FeudalNation:
                    case NationTypes.TribalNation:
                    case NationTypes.LairTerritory:
                        unclaimed_territory.AddRange(tile.UnclaimedTerritories);
                        break;
                    case NationTypes.HuntingGrounds:
                        unclaimed_territory.AddRange(tile.UnclaimedHuntingGrounds);
                        break;
                    case NationTypes.NomadicTribe:
                        unclaimed_territory.AddRange(tile.UnclaimedTravelAreas);
                        break;
                }
            }

            // Once no unclaimed space is left in a terrain the neighbouring terrains are chosen.
            if (unclaimed_territory.Count == 0)
            {
                List<Tile> neighbouring_tiles = new List<Tile>();
                foreach (Tile tile in _commanded_nation.Tiles)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        SystemCoordinates coords = tile.Coordinates.GetNeighbour(i);

                        if (coords.isInTileGridBounds())
                        {
                            // puts together a list of all neighbouring terrains with 
                            // unclaimed territory, which are not already in the territory list.
                            switch (_commanded_nation.Type)
                            {
                                case NationTypes.FeudalNation:
                                case NationTypes.TribalNation:
                                case NationTypes.LairTerritory:
                                    if (!_commanded_nation.Tiles.Contains(Program.World.TileGrid[coords.X, coords.Y]) 
                                        && Program.World.TileGrid[coords.X, coords.Y].UnclaimedTerritories.Count > 0)
                                        neighbouring_tiles.Add(Program.World.TileGrid[coords.X, coords.Y]);
                                    break;
                                case NationTypes.HuntingGrounds:
                                    if (!_commanded_nation.Tiles.Contains(Program.World.TileGrid[coords.X, coords.Y])
                                        && Program.World.TileGrid[coords.X, coords.Y].UnclaimedHuntingGrounds.Count > 0)
                                        neighbouring_tiles.Add(Program.World.TileGrid[coords.X, coords.Y]);
                                    break;
                                case NationTypes.NomadicTribe:
                                    if (!_commanded_nation.Tiles.Contains(Program.World.TileGrid[coords.X, coords.Y])
                                        && Program.World.TileGrid[coords.X, coords.Y].UnclaimedTravelAreas.Count > 0)
                                        neighbouring_tiles.Add(Program.World.TileGrid[coords.X, coords.Y]);
                                    break;
                            }
                        }
                    }
                }

                foreach (Tile tile in neighbouring_tiles)
                {
                    switch (_commanded_nation.Type)
                    {
                        case NationTypes.FeudalNation:
                        case NationTypes.TribalNation:
                        case NationTypes.LairTerritory:
                            unclaimed_territory.AddRange(tile.UnclaimedTerritories);
                            break;
                        case NationTypes.HuntingGrounds:
                            unclaimed_territory.AddRange(tile.UnclaimedHuntingGrounds);
                            break;
                        case NationTypes.NomadicTribe:
                            unclaimed_territory.AddRange(tile.UnclaimedTravelAreas);
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

            if (!_commanded_nation.Tiles.Contains(new_territory.Location))
                _commanded_nation.Tiles.Add(new_territory.Location);

            foreach (Tile tile in _commanded_nation.Tiles)
            {
                tile.UnclaimedTerritories.Remove(new_territory);
                switch (_commanded_nation.Type)
                {
                    case NationTypes.FeudalNation:
                    case NationTypes.TribalNation:
                    case NationTypes.LairTerritory:
                        tile.UnclaimedTerritories.Remove(new_territory);
                        break;
                    case NationTypes.HuntingGrounds:
                        tile.UnclaimedHuntingGrounds.Remove(new_territory);
                        break;
                    case NationTypes.NomadicTribe:
                        tile.UnclaimedTravelAreas.Remove(new_territory);
                        break;
                }
            }

            WarGoal war_goal = new WarGoal(WarGoalType.TerritoryConquest);
            war_goal.Territory = new_territory;
            _commanded_nation.PossibleWarGoals.Add(war_goal);

            creator.LastCreation = new_territory;
        }


        public ExpandTerritory(Nation commanded_nation) : base(commanded_nation)
        {
            Name = "Expand National Territory: " + commanded_nation.Name;
        }
    }
}
