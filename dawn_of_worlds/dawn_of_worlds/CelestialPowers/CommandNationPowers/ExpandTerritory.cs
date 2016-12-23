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
            if (targetProvinces().Count > 0)
                return true;
            else
                return false;
        }

        private List<Province> targetProvinces()
        {
            List<Province> target_provinces = new List<Province>();

            foreach (Province province in _commanded_nation.Territory)
            {
                for (int i = 0; i < 8; i++)
                {
                    SystemCoordinates coords = province.Coordinates.GetNeighbour(i);

                    if (coords.isInTileGridBounds())
                    {
                        Province neighbour_province = Program.World.getProvince(coords);
                        // Ignore this if it is already part of the territory.
                        if (_commanded_nation.Territory.Contains(neighbour_province))
                            continue;

                        switch (_commanded_nation.Type)
                        {
                            case NationTypes.FeudalNation:
                            case NationTypes.TribalNation:
                            case NationTypes.LairTerritory:
                                if (neighbour_province.Owner == null)
                                    target_provinces.Add(neighbour_province);
                                break;
                            case NationTypes.HuntingGrounds:
                            case NationTypes.NomadicTribe:
                                target_provinces.Add(neighbour_province);
                                break;
                        }

                    }
                }
            }

            return target_provinces;
        }

        public override int Weight(Deity creator)
        {
            int weight = base.Weight(creator);

            weight += Constants.TOTAL_PROVINCE_NUMBER;
            weight -= _commanded_nation.Territory.Count;


            if (creator.Domains.Contains(Domain.Conquest))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            return weight >= 0 ? weight : 0;
        }


        public override void Effect(Deity creator)
        {
            List<Province> provinces = targetProvinces();

            Province new_territory = provinces[Constants.Random.Next(provinces.Count)];
            _commanded_nation.Territory.Add(new_territory);
            switch (_commanded_nation.Type)
            {
                case NationTypes.FeudalNation:
                case NationTypes.TribalNation:
                case NationTypes.LairTerritory:
                    new_territory.Owner = _commanded_nation;
                    break;
                case NationTypes.HuntingGrounds:
                    new_territory.HuntingGrounds.Add(_commanded_nation);
                    break;
                case NationTypes.NomadicTribe:
                    new_territory.NomadicPresence.Add(_commanded_nation);
                    break;
            }

            WarGoal war_goal = new WarGoal(WarGoalType.Conquest);
            war_goal.Territory = new_territory;
            _commanded_nation.PossibleWarGoals.Add(war_goal);
        }


        public ExpandTerritory(Nation commanded_nation) : base(commanded_nation)
        {
            Name = "Expand National Territory: " + commanded_nation.Name;
        }
    }
}
