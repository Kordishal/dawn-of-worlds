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
            compile_candidate_nations();
            // can only be used if the commanded nation is not at war.

            // needs a nation it can ally with.
            if (candidate_nations.Count > 0)
                return true;

            return false;
        }

        private void compile_candidate_nations()
        {
            candidate_nations.Clear();

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

            foreach (Area area in considered_areas)
            {
                bool is_allied = false;
                foreach (Nation nation in area.Nations)
                {
                    // Checks whether the nation is already in an alliance with the commanded nation.
                    is_allied = false;
                    foreach (Alliance alliance in _commanded_nation.Alliances)
                    {
                        if (alliance.Members.Contains(nation))
                        {
                            is_allied = true;
                        }
                    }

                    if (!is_allied)
                        candidate_nations.Add(nation);
                        
                }
            }

        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            Nation new_ally = null;

            while (new_ally == null)
            {
                new_ally = candidate_nations[Main.MainLoop.RND.Next(candidate_nations.Count)];
            }

            Alliance alliance = new Alliance("Alliance of " + _commanded_nation.Name + " and " + new_ally.Name, creator);
            alliance.Members.Add(_commanded_nation);
            alliance.Members.Add(new_ally);

            _commanded_nation.Alliances.Add(alliance);
            new_ally.Alliances.Add(alliance);                     
        }


        public FormAlliance(Nation commanded_nation): base(commanded_nation)
        {
            Name = "Form Alliance: " + commanded_nation.Name;
            candidate_nations = new List<Nation>();
            compile_candidate_nations();
        }
    }
}
