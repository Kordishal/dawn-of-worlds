using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Main;
using dawn_of_worlds.Creations.Geography;

namespace dawn_of_worlds.CelestialPowers.CommandRacePowers
{
    class SettleTerrain : CommandRace
    {
        private Tile _settled_terrain { get; set; }

        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

            if (creator.Domains.Contains(Domain.Exploration))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            if (_commanded_race.Tags.Contains(RaceTags.RacialEpidemic))
                weight -= Constants.WEIGHT_STANDARD_CHANGE;

            foreach (RacialPreferredHabitatTerrain terrain in _commanded_race.PreferredTerrain)
            {
                switch (terrain)
                {
                    case RacialPreferredHabitatTerrain.CaveDwellers:
                        if (_settled_terrain.SecondaryTerrainFeatures.FindAll(x => x.GetType() == typeof(Cave)).Count > 0)
                            weight += _settled_terrain.SecondaryTerrainFeatures.FindAll(x => x.GetType() == typeof(Cave)).Count * 10;
                        break;
                    case RacialPreferredHabitatTerrain.DesertDwellers:
                        if (_settled_terrain.PrimaryTerrainFeature.GetType() == typeof(Desert))
                            weight += Constants.WEIGHT_STANDARD_CHANGE;
                        break;
                    case RacialPreferredHabitatTerrain.ForestDwellers:
                        if (_settled_terrain.PrimaryTerrainFeature.GetType() == typeof(Forest))
                            weight += Constants.WEIGHT_STANDARD_CHANGE;
                        break;
                    case RacialPreferredHabitatTerrain.HillDwellers:
                        if (_settled_terrain.Type == TerrainType.HillRange)
                            weight += Constants.WEIGHT_STANDARD_CHANGE;
                        break;
                    case RacialPreferredHabitatTerrain.MountainDwellers:
                        if (_settled_terrain.Type == TerrainType.MountainRange)
                            weight += Constants.WEIGHT_STANDARD_CHANGE;
                        break;
                    case RacialPreferredHabitatTerrain.PlainDwellers:
                        if (_settled_terrain.Type == TerrainType.Plain)
                            weight += Constants.WEIGHT_STANDARD_CHANGE;
                        break;
                }
            }

            foreach (RacialPreferredHabitatClimate climate in _commanded_race.PreferredClimate)
            {
                switch (climate)
                {
                    case RacialPreferredHabitatClimate.ColdAcclimated:
                        if (_settled_terrain.Area.ClimateArea == Climate.Arctic || _settled_terrain.Area.ClimateArea == Climate.SubArctic)
                            weight += Constants.WEIGHT_STANDARD_CHANGE * 2;
                        break;
                    case RacialPreferredHabitatClimate.HeatAcclimated:
                        if (_settled_terrain.Area.ClimateArea == Climate.Tropical || _settled_terrain.Area.ClimateArea == Climate.SubTropical)
                            weight += Constants.WEIGHT_STANDARD_CHANGE * 2;
                        break;
                    case RacialPreferredHabitatClimate.TemperateAcclimated:
                        if (_settled_terrain.Area.ClimateArea == Climate.Temperate)
                            weight += Constants.WEIGHT_STANDARD_CHANGE * 2;
                        break;
                }
            }


            return weight >= 0 ? weight : 0;
        }

        private bool neighbouringTerrainHasRace(World current_world)
        {
            for (int i = 0; i < 8; i++)
            {
                SystemCoordinates coords = _settled_terrain.Coordinates.GetNeighbour(i);

                if (coords.X >= 0 && coords.Y >= 0 && coords.X < Constants.TERRAIN_GRID_X && coords.Y < Constants.TERRAIN_GRID_Y)
                {
                    if (current_world.TerrainGrid[coords.X, coords.Y].SettledRaces.Contains(_commanded_race))
                        return true;
                }
            }
            return false;
        }

        public override bool Precondition(Deity creator)
        {
            if (_settled_terrain.SettledRaces.Contains(_commanded_race))
                return false;

            // if this is a subrace
            // Exclude all terrains where there are no similar races nearby.
            if (!neighbouringTerrainHasRace(Program.World))
                  return false;

            // Aquatic, exclude all areas, which do not have water to live in.
            if (_commanded_race.Habitat == RacialHabitat.Aquatic)
                if (!(_settled_terrain.Type == TerrainType.Ocean) && !(_settled_terrain.SecondaryTerrainFeatures.Exists(x => x.GetType() == typeof(Lake))))
                    return false;

            // Subterranean, exlude all areas, which do not have an underworld or caves.
            if (_commanded_race.Habitat == RacialHabitat.Subterranean)
                if (!_settled_terrain.SecondaryTerrainFeatures.Exists(x => x.GetType() == typeof(Cave)))
                    return false;

            // Terranean, exclude all areas which do not include a landmass to live on
            if (_commanded_race.Habitat == RacialHabitat.Terranean)
                if (_settled_terrain.Type == TerrainType.Ocean)
                    return false;

            return true;
        }


        public override void Effect(Deity creator)
        {
            _settled_terrain.SettledRaces.Add(_commanded_race);
            _commanded_race.SettledTerrains.Add(_settled_terrain);
            creator.LastCreation = _settled_terrain.PrimaryTerrainFeature;
        }


        public SettleTerrain(Race commanded_race, Tile terrain) : base(commanded_race)
        {
            Name = "Settle Terrain: " + commanded_race.Name + " in " + terrain.Name;
            _settled_terrain = terrain;
        }
    }
}
