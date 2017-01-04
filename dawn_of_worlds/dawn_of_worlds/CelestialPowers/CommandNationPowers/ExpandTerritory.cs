using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Creations.Diplomacy;
using dawn_of_worlds.Modifiers;

namespace dawn_of_worlds.CelestialPowers.CommandNationPowers
{
    class ExpandTerritory : CommandNation
    {
        public override bool Precondition(Deity creator)
        {
            base.Precondition(creator);
            if (targetProvinces().Count > 0)
                return true;
            else
                return false;
        }


        protected override void initialize()
        {
            base.initialize();
            Name = "Expand National Territory: " + _commanded_nation.Name;
            Tags = new List<CreationTag>() { CreationTag.Expansion };
        }

        public override void Effect(Deity creator)
        {
            List<Province> provinces = targetProvinces();

            Province new_territory = provinces[Constants.Random.Next(provinces.Count)];
            _commanded_nation.Territory.Add(new_territory);
            switch (_commanded_nation.GovernmentForm)
            {
                case GovernmentForm.FeudalNation:
                case GovernmentForm.TribalNation:
                case GovernmentForm.LairTerritory:
                    new_territory.Owner = _commanded_nation;
                    break;
                case GovernmentForm.HuntingGrounds:
                    new_territory.HuntingGrounds.Add(_commanded_nation);
                    break;
                case GovernmentForm.NomadicTribe:
                    new_territory.NomadicPresence.Add(_commanded_nation);
                    break;
            }

            WarGoal war_goal = new WarGoal(WarGoalType.Conquest);
            war_goal.Territory = new_territory;
            _commanded_nation.PossibleWarGoals.Add(war_goal);
        }


        public ExpandTerritory(Civilisation commanded_nation) : base(commanded_nation) { initialize(); }

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

                        switch (_commanded_nation.GovernmentForm)
                        {
                            case GovernmentForm.FeudalNation:
                            case GovernmentForm.TribalNation:
                            case GovernmentForm.LairTerritory:
                                if (neighbour_province.Owner == null)
                                    target_provinces.Add(neighbour_province);
                                break;
                            case GovernmentForm.HuntingGrounds:
                            case GovernmentForm.NomadicTribe:
                                target_provinces.Add(neighbour_province);
                                break;
                        }

                    }
                }
            }

            return target_provinces;
        }
    }
}
