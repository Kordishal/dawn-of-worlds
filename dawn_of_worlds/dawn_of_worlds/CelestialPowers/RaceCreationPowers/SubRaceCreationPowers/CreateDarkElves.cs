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
    class CreateDarkElves : CreateSubRace
    {
        public static bool notCreatedDarkElves = true;

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            if (notCreatedDarkElves && !CreateElves.notCreatedElves)
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

                    Race dark_elves = new Race("Dark Elves", creator, location);
                    location.Inhabitants.Add(dark_elves);
                    dark_elves.isSubRace = true;

                    foreach (Area a in current_world.AreaGrid)
                    {
                        foreach (Race r in a.Inhabitants)
                        {
                            if (r.Name == "Elves")
                            {
                                dark_elves.MainRace = r;
                                r.SubRaces.Add(dark_elves);
                            }
                        }
                    }
                     
                    notCreatedDarkElves = false;

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
                        if (r.Name == "Elves")
                        {
                            return true;
                        }
                    }
                }     
            }

            return false;
        }

        public CreateDarkElves()
        {
            Name = "Create Dark Elves";
        }
    }
}
