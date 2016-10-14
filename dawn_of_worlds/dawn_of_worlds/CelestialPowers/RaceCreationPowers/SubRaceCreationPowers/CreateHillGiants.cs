using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Inhabitants;

namespace dawn_of_worlds.CelestialPowers.RaceCreationPowers.SubRaceCreationPowers
{
    class CreateHillGiants : CreateSubRace
    {
        public static bool notCreatedHillGiants = true;

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            if (notCreatedHillGiants && !CreateGiants.notCreatedGiants)
                return true;
            else
                return false;
        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            bool not_found_valid_area = true;

            while (not_found_valid_area)
            {
                Area location = current_world.AreaGrid[Main.MainLoop.RND.Next(Main.MainLoop.AREA_GRID_X), Main.MainLoop.RND.Next(Main.MainLoop.AREA_GRID_Y)];

                if (location.AreaRegion.Landmass && neighbourAreaHasMainRace(location.Neighbours))
                {
                    not_found_valid_area = false;

                    Race hill_giants = new Race("Hill Giants", creator, location);
                    location.Inhabitants.Add(hill_giants);
                    hill_giants.isSubRace = true;

                    foreach (Area a in current_world.AreaGrid)
                    {
                        foreach (Race r in a.Inhabitants)
                        {
                            if (r.Name == "Giants")
                            {
                                hill_giants.MainRace = r;
                                r.SubRaces.Add(hill_giants);
                            }
                        }
                    }

                    notCreatedHillGiants = false;

                }
            }
        }

        private bool neighbourAreaHasMainRace(Area[] neighbours)
        {
            foreach (Area a in neighbours)
            {
                if (a != null)
                {
                    foreach (Race r in a.Inhabitants)
                    {
                        if (r.Name == "Giants")
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public CreateHillGiants()
        {
            Name = "Create Hill Giants";
        }
    }
}
