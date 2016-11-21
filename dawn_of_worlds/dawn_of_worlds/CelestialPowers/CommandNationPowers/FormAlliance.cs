using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Creations.Diplomacy;

namespace dawn_of_worlds.CelestialPowers.CommandNationPowers
{
    class FormAlliance : CommandNation
    {

        private List<Nation> candidate_nations { get; set; }

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            // If nation no longer exists.
            if (isObsolete)
                return false;

            // cannot make new alliances while at war.
            if (_commanded_nation.Wars.Count > 0)
                return false;

            compile_candidate_nations();

            // needs a nation it can ally with.
            if (candidate_nations.Count > 0)
                return true;

            return false;
        }

        private void compile_candidate_nations()
        {
            candidate_nations.Clear();

            // Alliances can only be formed with nations within the same area.
            List<Area> considered_areas = new List<Area>();
            foreach (Area settled_area in _commanded_nation.TerritoryAreas)
            {
                considered_areas.Add(settled_area);
                foreach (Area neighbour_area in settled_area.Neighbours)
                {
                    if (neighbour_area != null && !_commanded_nation.TerritoryAreas.Contains(neighbour_area))
                    {
                        considered_areas.Add(neighbour_area);
                    }
                }
            }

            // Alliances can only be formed with nations they are not allied with.
            foreach (Area area in considered_areas)
            {
                foreach (Nation nation in area.Nations)
                {
                    if (_commanded_nation.AlliedNations.Contains(nation))
                        candidate_nations.Add(nation);                    
                }
            }

        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            // The new ally will be chosen amongst the possible allies at random.
            Nation new_ally = null;
            while (new_ally == null)
            {
                new_ally = candidate_nations[Main.MainLoop.RND.Next(candidate_nations.Count)];
            }

            // Add nations to list of allied nations.
            _commanded_nation.AlliedNations.Add(new_ally);
            new_ally.AlliedNations.Add(_commanded_nation);

            creator.LastCreation = null;
        }


        public FormAlliance(Nation commanded_nation): base(commanded_nation)
        {
            Name = "Form Alliance: " + commanded_nation.Name;
            candidate_nations = new List<Nation>();
            compile_candidate_nations();
        }
    }
}
