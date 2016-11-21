using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Inhabitants;

namespace dawn_of_worlds.CelestialPowers.CommandRacePowers
{
    class SettleArea : CommandRace
    {
        private List<Area> _possible_target_areas { get; set; }

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            if (_commanded_race.Tags.Contains(RaceTags.RacialEpidemic))
                return false;

            _possible_target_areas = new List<Area>();

            foreach (Area a in _commanded_race.SettledAreas)
            {
                foreach (Area b in a.Neighbours)
                {
                    if (b != null && b.AreaRegion.Landmass && !_commanded_race.SettledAreas.Contains(b))
                    {
                        _possible_target_areas.Add(b);
                    }
                }
            }

            // if there is no possibilty to settle a new area the command is removed
            if (_possible_target_areas.Count == 0)
            {
                foreach (Deity d in current_world.Deities)
                {
                    d.Powers.Remove(this);
                }
                return false;
            }
            else
            {
                return true;
            }
        }


        public override void Effect(World current_world, Deity creator, int current_age)
        {
            Area new_settlement = _possible_target_areas[Main.MainLoop.RND.Next(_possible_target_areas.Count)];

            new_settlement.Inhabitants.Add(_commanded_race);
            _commanded_race.SettledAreas.Add(new_settlement);
        }


        public SettleArea(Race commanded_race) : base(commanded_race)
        {
            Name = "Settle Area: " + commanded_race.Name;
        }
    }
}
