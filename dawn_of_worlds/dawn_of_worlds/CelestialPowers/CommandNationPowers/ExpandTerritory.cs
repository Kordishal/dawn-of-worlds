using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldModel;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Creations.Diplomacy;
using dawn_of_worlds.Effects;
using dawn_of_worlds.Creations.Civilisations;

namespace dawn_of_worlds.CelestialPowers.CommandNationPowers
{
    class ExpandTerritory : CommandNation
    {
        private List<Province> ExpansionTargetProvinces { get; set; }

        public override bool Precondition(Deity creator)
        {
            base.Precondition(creator);
            if (ExpansionTargetProvinces.Count > 0)
                return true;
            else
                return false;
        }

        protected override void initialize()
        {
            base.initialize();
            Name = "Expand National Territory: " + _commanded_nation.Name;
            Tags = new List<CreationTag>() { CreationTag.Expansion };
            ExpansionTargetProvinces = new List<Province>();
            foreach (Province province in _commanded_nation.Territory)
            {
                ExpansionTargetProvinces.AddRange(newExpansionProvinces(province));
            }
        }

        public override int Effect(Deity creator)
        {
            Province new_territory = ExpansionTargetProvinces[rnd.Next(ExpansionTargetProvinces.Count)];
            _commanded_nation.Territory.Add(new_territory);
            if (_commanded_nation.isNomadic)
                new_territory.NomadicPresence.Add(_commanded_nation);
            else
                new_territory.Owner = _commanded_nation;

            ExpansionTargetProvinces.AddRange(newExpansionProvinces(new_territory));

            //WarGoal war_goal = new WarGoal(WarGoalType.Conquest);
            //war_goal.Territory = new_territory;
            //_commanded_nation.PossibleWarGoals.Add(war_goal);

            return 0;
        }


        public ExpandTerritory(Civilisation commanded_nation) : base(commanded_nation) { initialize(); }

        private List<Province> newExpansionProvinces(Province province)
        {
            List<Province> target_provinces = new List<Province>();

            for (int i = 0; i < 8; i++)
            {
                SystemCoordinates coords = province.Coordinates.GetNeighbour(i);

                if (coords.isInTileGridBounds())
                {
                    Province neighbour_province = Program.State.getProvince(coords);

                    // Ignore this if it is already part of the territory.
                    if (_commanded_nation.Territory.Contains(neighbour_province))
                        continue;
                    // Do not duplicate provinces in this list.
                    if (ExpansionTargetProvinces.Contains(neighbour_province))
                        continue;

                    if (_commanded_nation.isNomadic)
                        target_provinces.Add(neighbour_province);
                    else
                        if (!neighbour_province.hasOwner)
                                target_provinces.Add(neighbour_province);
                }
            }         

            return target_provinces;
        }
    }
}
