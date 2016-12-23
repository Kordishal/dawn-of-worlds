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

        private List<WeightedObjects<Province>> candidate_provinces()
        {
            List<WeightedObjects<Province>> possible_locations = new List<WeightedObjects<Province>>();

            foreach (Province province in _settling_area.Provinces)
            {
                WeightedObjects<Province> candidate_province = new WeightedObjects<Province>(province);

                if (province.SettledRaces.Contains(_commanded_race))
                    continue;

                // Aquatic, exclude all areas, which do not have water to live in.
                if (_commanded_race.Habitat == RacialHabitat.Aquatic)
                    if (!(province.Type == TerrainType.Ocean) && !(province.SecondaryTerrainFeatures.Exists(x => x.GetType() == typeof(Lake))))
                        continue;

                // Subterranean, exlude all areas, which do not have an underworld or caves.
                if (_commanded_race.Habitat == RacialHabitat.Subterranean)
                    if (!province.SecondaryTerrainFeatures.Exists(x => x.GetType() == typeof(Cave)))
                        continue;

                // Terranean, exclude all areas which do not include a landmass to live on
                if (_commanded_race.Habitat == RacialHabitat.Terranean)
                    if (province.Type == TerrainType.Ocean)
                        continue;

                // Settle in areas close by.
                for (int i = 0; i < 8; i++)
                {
                    SystemCoordinates coords = province.Coordinates.GetNeighbour(i);

                    if (coords.isInTileGridBounds())
                    {
                        if (Program.World.getProvince(coords).SettledRaces.Contains(_commanded_race))
                            candidate_province.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                    }
                }


                foreach (RacialPreferredHabitatTerrain terrain in _commanded_race.PreferredTerrain)
                {
                    switch (terrain)
                    {
                        case RacialPreferredHabitatTerrain.CaveDwellers:
                            if (province.SecondaryTerrainFeatures.FindAll(x => x.GetType() == typeof(Cave)).Count > 0)
                                candidate_province.Weight += province.SecondaryTerrainFeatures.FindAll(x => x.GetType() == typeof(Cave)).Count * 10;
                            break;
                        case RacialPreferredHabitatTerrain.DesertDwellers:
                            if (province.PrimaryTerrainFeature.GetType() == typeof(Desert))
                                candidate_province.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                            break;
                        case RacialPreferredHabitatTerrain.ForestDwellers:
                            if (province.PrimaryTerrainFeature.GetType() == typeof(Forest))
                                candidate_province.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                            break;
                        case RacialPreferredHabitatTerrain.HillDwellers:
                            if (province.Type == TerrainType.HillRange)
                                candidate_province.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                            break;
                        case RacialPreferredHabitatTerrain.MountainDwellers:
                            if (province.Type == TerrainType.MountainRange)
                                candidate_province.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                            break;
                        case RacialPreferredHabitatTerrain.PlainDwellers:
                            if (province.Type == TerrainType.Plain)
                                candidate_province.Weight += Constants.WEIGHT_STANDARD_CHANGE;
                            break;
                    }
                }

                foreach (RacialPreferredHabitatClimate climate in _commanded_race.PreferredClimate)
                {
                    switch (climate)
                    {
                        case RacialPreferredHabitatClimate.Arctic:
                            if (province.LocalClimate == Climate.Arctic)
                                candidate_province.Weight += Constants.WEIGHT_STANDARD_CHANGE * 2;
                            break;
                        case RacialPreferredHabitatClimate.Subarctic:
                            if (province.LocalClimate == Climate.SubArctic)
                                candidate_province.Weight += Constants.WEIGHT_STANDARD_CHANGE * 2;
                            break;
                        case RacialPreferredHabitatClimate.Tropical:
                            if (province.LocalClimate == Climate.Tropical)
                                candidate_province.Weight += Constants.WEIGHT_STANDARD_CHANGE * 2;
                            break;
                        case RacialPreferredHabitatClimate.Subtropical:
                            if (province.LocalClimate == Climate.SubTropical)
                                candidate_province.Weight += Constants.WEIGHT_STANDARD_CHANGE * 2;
                            break;
                        case RacialPreferredHabitatClimate.Temperate:
                            if (province.LocalClimate == Climate.Temperate)
                                candidate_province.Weight += Constants.WEIGHT_STANDARD_CHANGE * 2;
                            break;
                    }
                }

                if (candidate_province.Weight > 0)
                    possible_locations.Add(candidate_province);
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

            // The less settled provinces the more likle to settle new ones.
            weight += Constants.TOTAL_TILE_NUMBER;
            weight -= _commanded_race.SettledProvinces.Count;



            return weight >= 0 ? weight : 0;
        }

        public override bool Precondition(Deity creator)
        {
            if (candidate_provinces().Count > 0)
                return true;
            else
                return false;
        }


        public override void Effect(Deity creator)
        {
            List<WeightedObjects<Province>> possible_target_province = candidate_provinces();

            int number_of_settled_provinces = Constants.BASE_TILES_SETTLED_BY_RACE;

            if (_commanded_race.SocialCulturalCharacteristics.Contains(SocialCulturalCharacteristic.Nomadic))
                number_of_settled_provinces += 1;
            if (_commanded_race.SocialCulturalCharacteristics.Contains(SocialCulturalCharacteristic.Sedentary))
                number_of_settled_provinces -= 1;

            if (number_of_settled_provinces > possible_target_province.Count)
                number_of_settled_provinces = possible_target_province.Count;

            List<Province> target_provinces = WeightedObjects<Province>.ChooseXHeaviestObjects(possible_target_province, number_of_settled_provinces);

            foreach (Province province in target_provinces)
            {
                province.SettledRaces.Add(_commanded_race);
                _commanded_race.SettledProvinces.Add(province);
            }         
        }


        public SettleTile(Race commanded_race, Area area) : base(commanded_race)
        {
            Name = "Settle Terrain: " + commanded_race.Name + " in " + area.Name;
            _settling_area = area;
        }
    }
}
