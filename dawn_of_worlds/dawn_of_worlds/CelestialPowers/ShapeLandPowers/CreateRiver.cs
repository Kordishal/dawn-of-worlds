using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    class CreateRiver : ShapeLand
    {

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            // Needs a mountain range to start from
            if (_location.MountainRanges == null)
                return false;

            return true;
        }

        public override int Weight(World current_world, Deity creator, int current_age)
        {
            int weight = base.Weight(current_world, creator, current_age);

            if (creator.Domains.Contains(Domain.Water))
                weight += Constants.WEIGHT_MANY_CHANGE;

            if (creator.Domains.Contains(Domain.Drought))
                weight -= Constants.WEIGHT_MANY_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            // Create the river
            River river = new River("PlaceHolder", _location, creator);
            river.BiomeType = BiomeType.PermanentRiver;
            river.Spring = _location.MountainRanges;
            river.Riverbed.Add(river.Spring.Location);
        
            // the primary direction of the river. 
            int primary_direction = Main.Constants.RND.Next(4);
            Area current_location = _location;
            Area[] current_neighbours = _location.GetNeighbours(primary_direction);
            bool not_has_found_destination = true;

            while (not_has_found_destination)
            {
                // Check if there are any lakes and/or rivers in this area, then let this new river flow into a random one.
                if (current_location.Lakes.Count > 0 || current_location.Rivers.Count > 0)
                {
                    List<Terrain> lakes_and_rivers = new List<Terrain>();
                    lakes_and_rivers.AddRange(current_location.Lakes);
                    lakes_and_rivers.AddRange(current_location.Rivers);

                    Terrain destination = lakes_and_rivers[Main.Constants.RND.Next(lakes_and_rivers.Count)];

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
                    int chance = Main.Constants.RND.Next(75);
                    // ...straight in primary direction
                    if (chance < 50)
                    {
                        if (current_neighbours[0] != null)
                            current_location = current_neighbours[0];
                        else
                            current_location = null;
                    }
                    // ... turn clock-wise from primary direction
                    else if (chance < 75)
                    {
                        if (current_neighbours[1] != null)
                            current_location = current_neighbours[1];
                        else
                            current_location = null;
                    }
                    // ... turn counter-clock-wise from primary direction
                    else if (chance < 100)
                    {
                        if (current_neighbours[2] != null)
                            current_location = current_neighbours[2];
                        else
                            current_location = null;
                    }

                    if (current_location != null)
                    {
                        // if that new area is ocean the river ends here.
                        if (!current_location.AreaRegion.Landmass)
                        {
                            river.Riverbed.Add(current_location);
                            river.Destination = current_location;
                            not_has_found_destination = false;
                        }
                        // otherwise the river runs further.
                        else
                        {
                            river.Riverbed.Add(current_location);
                            current_neighbours = current_location.GetNeighbours(primary_direction);
                        }

                    }
                    else // if current location is null then the border of the map has been found and the river ends in unknown land.
                    {
                        river.Destination = new Area(null);
                        river.Destination.Name = "Unknown Land";
                        river.Riverbed.Add(river.Destination);
                        not_has_found_destination = false;
                    }
                }                
            }

            // Add river to areas at the end in order to avoid the river ending in itself.
            foreach (Area a in river.Riverbed)
            {
                a.Rivers.Add(river);
                a.Terrain.Add(river);
                //a.UnclaimedTerritory.Add(river); rivers are currently not added as unclaimed terrain.
            }

            // Add river to deity list.
            creator.Creations.Add(river);
            creator.LastCreation = river;        
        }

        public CreateRiver(Area location) : base (location)
        {
            Name = "Create River in Area " + location.Name;
        }
    }
}
