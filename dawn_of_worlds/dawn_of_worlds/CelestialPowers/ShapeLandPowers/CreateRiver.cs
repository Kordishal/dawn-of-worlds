using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Geography;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    class CreateRiver : ShapeLand
    {

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            // Needs at least one Mountainrange to exist.
            foreach (Area a in current_world.AreaGrid)
            {
                if (a.MountainRanges != null)
                {
                    return true;
                }
            }

            return false;
        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            bool not_found_valid_area = true;

            while (not_found_valid_area)
            {
                Area location = current_world.AreaGrid[Main.MainLoop.RND.Next(Main.MainLoop.AREA_GRID_X), Main.MainLoop.RND.Next(Main.MainLoop.AREA_GRID_Y)];

                if (location.MountainRanges != null)
                {
                    not_found_valid_area = false;
                    River river = new River("The River", location, creator);

                    river.Spring = location.MountainRanges;
                    river.Riverbed.Add(river.Spring.Location);
        
                    int primary_direction = Main.MainLoop.RND.Next(4);
                    Area current_location = location;
                    Area[] current_neighbours = location.GetNeighbours(primary_direction);
                    bool not_has_found_destination = true;

                    while (not_has_found_destination)
                    {
                        // Check if there are any lakes and/or rivers in this area, then let this new river flow into a random one.
                        if (current_location.Lakes.Count > 0 || current_location.Rivers.Count > 0)
                        {
                            List<GeographicalFeature> lakes_and_rivers = new List<GeographicalFeature>();
                            lakes_and_rivers.AddRange(current_location.Lakes);
                            lakes_and_rivers.AddRange(current_location.Rivers);

                            GeographicalFeature destination = lakes_and_rivers[Main.MainLoop.RND.Next(lakes_and_rivers.Count)];

                            if (typeof(Lake) == destination.GetType())
                            {
                                river.DestinationLake = (Lake)destination;
                                ((Lake)destination).SourceRivers.Add(river);
                            }
                            else
                            {
                                river.DestinationRiver = (River)destination;
                                ((River)destination).SourceRivers.Add(river);
                            }

                            not_has_found_destination = false;
                        }
                        else
                        {
                            // else go towards area...
                            int chance = Main.MainLoop.RND.Next(100);
                            if (0 <= chance && chance < 50 && current_neighbours[0] != null) // ...straight in primary direction
                            {
                                current_location = current_neighbours[0];
                                river.Riverbed.Add(current_location);
                                current_neighbours = current_location.GetNeighbours(primary_direction);

                            }
                            else if (50 <= chance && chance < 75 && current_neighbours[1] != null) // ... turn clock-wise from primary direction
                            {
                                current_location = current_neighbours[1];
                                river.Riverbed.Add(current_location);
                                current_neighbours = current_location.GetNeighbours(primary_direction);
                            }
                            else if (75 <= chance && chance < 100 && current_neighbours[2] != null) // ... turn counter-clock-wise from primary direction
                            {
                                current_location = current_neighbours[2];
                                river.Riverbed.Add(current_location);
                                current_neighbours = current_location.GetNeighbours(primary_direction);
                            }
                            else
                            {
                                river.Destination = new Area(null);
                                river.Destination.Name = "Unknown Land";
                                river.Riverbed.Add(river.Destination);
                                not_has_found_destination = false;
                                break;
                            }

                            // if that new area is ocean the river ends here.
                            if (!current_location.AreaRegion.Landmass)
                            {
                                river.Destination = current_location;
                                not_has_found_destination = false;
                            }
                        }                
                    }

                    // Add river to areas at the end in order to avoid the river ending in itself.
                    foreach (Area a in river.Riverbed)
                    {
                        a.Rivers.Add(river);
                        a.GeographicalFeatures.Add(river);
                        a.UnclaimedTerritory.Add(river);
                    }

                    creator.Creations.Add(river);
                }
            }
        }

        public CreateRiver()
        {
            Name = "Create River";
        }
    }
}
