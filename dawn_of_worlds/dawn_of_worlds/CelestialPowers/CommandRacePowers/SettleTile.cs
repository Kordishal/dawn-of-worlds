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
    class SettleTile : CommandRace
    {
        private Area _settling_area { get; set; }

        private List<WeightedObjects<Tile>> candidate_tiles()
        {
            List<WeightedObjects<Tile>> possible_locations = new List<WeightedObjects<Tile>>();

            foreach (Tile tile in _settling_area.Tiles)
            {
                WeightedObjects<Tile> candidate_tile = new WeightedObjects<Tile>(tile);

                if (tile.SettledRaces.Contains(_commanded_race))
                    continue;

                // Aquatic, exclude all areas, which do not have water to live in.
                if (_commanded_race.Habitat == RacialHabitat.Aquatic)
                    if (!(tile.Type == TerrainType.Ocean) && !(tile.SecondaryTerrainFeatures.Exists(x => x.GetType() == typeof(Lake))))
                        continue;

                // Subterranean, exlude all areas, which do not have an underworld or caves.
                if (_commanded_race.Habitat == RacialHabitat.Subterranean)
                    if (!tile.SecondaryTerrainFeatures.Exists(x => x.GetType() == typeof(Cave)))
                        continue;

                // Terranean, exclude all areas which do not include a landmass to live on
                if (_commanded_race.Habitat == RacialHabitat.Terranean)
                    if (tile.Type == TerrainType.Ocean)
                        continue;

                // Settle in areas close by.
                for (int i = 0; i < 8; i++)
                {
                    SystemCoordinates coords = tile.Coordinates.GetNeighbour(i);

                    if (coords.isInTileGridBounds())
                    {
                        if (Program.World.getTile(coords).SettledRaces.Contains(_commanded_race))
                            candidate_tile.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                    }
                }


                foreach (RacialPreferredHabitatTerrain terrain in _commanded_race.PreferredTerrain)
                {
                    switch (terrain)
                    {
                        case RacialPreferredHabitatTerrain.CaveDwellers:
                            if (tile.SecondaryTerrainFeatures.FindAll(x => x.GetType() == typeof(Cave)).Count > 0)
                                candidate_tile.Weight += tile.SecondaryTerrainFeatures.FindAll(x => x.GetType() == typeof(Cave)).Count * 10;
                            break;
                        case RacialPreferredHabitatTerrain.DesertDwellers:
                            if (tile.PrimaryTerrainFeature.GetType() == typeof(Desert))
                                candidate_tile.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                            break;
                        case RacialPreferredHabitatTerrain.ForestDwellers:
                            if (tile.PrimaryTerrainFeature.GetType() == typeof(Forest))
                                candidate_tile.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                            break;
                        case RacialPreferredHabitatTerrain.HillDwellers:
                            if (tile.Type == TerrainType.HillRange)
                                candidate_tile.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                            break;
                        case RacialPreferredHabitatTerrain.MountainDwellers:
                            if (tile.Type == TerrainType.MountainRange)
                                candidate_tile.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                            break;
                        case RacialPreferredHabitatTerrain.PlainDwellers:
                            if (tile.Type == TerrainType.Plain)
                                candidate_tile.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                            break;
                    }
                }

                foreach (RacialPreferredHabitatClimate climate in _commanded_race.PreferredClimate)
                {
                    switch (climate)
                    {
                        case RacialPreferredHabitatClimate.Arctic:
                            if (tile.LocalClimate == Climate.Arctic)
                                candidate_tile.Weight += Constants.WEIGHT_STANDARD_CHANGE * 2;
                            break;
                        case RacialPreferredHabitatClimate.Subarctic:
                            if (tile.LocalClimate == Climate.SubArctic)
                                candidate_tile.Weight += Constants.WEIGHT_STANDARD_CHANGE * 2;
                            break;
                        case RacialPreferredHabitatClimate.Tropical:
                            if (tile.LocalClimate == Climate.Tropical)
                                candidate_tile.Weight += Constants.WEIGHT_STANDARD_CHANGE * 2;
                            break;
                        case RacialPreferredHabitatClimate.Subtropical:
                            if (tile.LocalClimate == Climate.SubTropical)
                                candidate_tile.Weight += Constants.WEIGHT_STANDARD_CHANGE * 2;
                            break;
                        case RacialPreferredHabitatClimate.Temperate:
                            if (tile.LocalClimate == Climate.Temperate)
                                candidate_tile.Weight += Constants.WEIGHT_STANDARD_CHANGE * 2;
                            break;
                    }
                }

                if (candidate_tile.Weight > 0)
                    possible_locations.Add(candidate_tile);
            }
            return possible_locations;
        }


        public override int Cost()
        {
            int cost = base.Cost();
            cost -= 2;
            return cost;
        }

        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

            if (creator.Domains.Contains(Domain.Exploration))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            if (_commanded_race.Tags.Contains(RaceTags.RacialEpidemic))
                weight -= Constants.WEIGHT_STANDARD_CHANGE;

            if (_commanded_race.SocialCulturalCharacteristics.Contains(SocialCulturalCharacteristic.Nomadic))
                weight += Constants.WEIGHT_STANDARD_CHANGE * 2;

            if (_commanded_race.SocialCulturalCharacteristics.Contains(SocialCulturalCharacteristic.Sedentary))
                weight -= Constants.WEIGHT_STANDARD_CHANGE * 2;

            // The less settled tiles the more likle to settle new ones.
            weight += Constants.TOTAL_TILE_NUMBER;
            weight -= _commanded_race.SettledTiles.Count;



            return weight >= 0 ? weight : 0;
        }

        public override bool Precondition(Deity creator)
        {
            if (candidate_tiles().Count > 0)
                return true;
            else
                return false;
        }


        public override void Effect(Deity creator)
        {
            List<WeightedObjects<Tile>> possible_target_tile = candidate_tiles();

            int number_of_settled_tiles = Constants.BASE_TILES_SETTLED_BY_RACE;

            if (_commanded_race.SocialCulturalCharacteristics.Contains(SocialCulturalCharacteristic.Nomadic))
                number_of_settled_tiles += 1;
            if (_commanded_race.SocialCulturalCharacteristics.Contains(SocialCulturalCharacteristic.Sedentary))
                number_of_settled_tiles -= 1;

            if (number_of_settled_tiles > possible_target_tile.Count)
                number_of_settled_tiles = possible_target_tile.Count;

            List<Tile> target_tiles = WeightedObjects<Tile>.ChooseXHeaviestObjects(possible_target_tile, number_of_settled_tiles);

            foreach (Tile tile in target_tiles)
            {
                tile.SettledRaces.Add(_commanded_race);
                _commanded_race.SettledTiles.Add(tile);
            }         
        }


        public SettleTile(Race commanded_race, Area area) : base(commanded_race)
        {
            Name = "Settle Terrain: " + commanded_race.Name + " in " + area.Name;
            _settling_area = area;
        }
    }
}
