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
    class CreateFireDragons : CreateSubRace
    {
        public static bool notCreatedFireDragons = true;

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            if (notCreatedFireDragons && !CreateDragons.notCreatedDragons)
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

                    Race fire_dragons = new Race("Fire Dragons", creator, location);
                    location.Inhabitants.Add(fire_dragons);
                    fire_dragons.isSubRace = true;

                    foreach (Area a in current_world.AreaGrid)
                    {
                        foreach (Race r in a.Inhabitants)
                        {
                            if (r.Name == "Dragons")
                            {
                                fire_dragons.MainRace = r;
                                r.SubRaces.Add(fire_dragons);
                            }
                        }
                    }

                    notCreatedFireDragons = false;

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
                        if (r.Name == "Dragons")
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public CreateFireDragons()
        {
            Name = "Create Fire Dragons";
        }
    }
}
