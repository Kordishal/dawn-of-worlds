using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldModel;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;
using dawn_of_worlds.Effects;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    class CreateRiver : ShapeLand
    {
        protected override void initialize()
        {
            base.initialize();
            Name = "Create River";
            isPrimary = false;
            Tags = new List<CreationTag>() { CreationTag.Creation, CreationTag.Water, CreationTag.Trade };
        }

        protected override bool selectionModifier(Province province)
        {
            if (!(province.Type == TerrainType.MountainRange))
                return false;
            else
                return true;
        }

        public override int Effect(Deity creator)
        {
            // Create the river
            River river = new River(Program.GenerateNames.GetName("river_names"), SelectedProvince, creator);
            river.BiomeType = BiomeType.PermanentRiver;
            river.Spring = (MountainRange)SelectedProvince.PrimaryTerrainFeature;
            river.Riverbed.Add(river.Spring.Province);

            // the primary direction of the river. 
            Array directions = Enum.GetValues(typeof(Direction));
            Direction primary_direction = (Direction)directions.GetValue(Constants.Random.Next(directions.Length));
            Direction[] other_directions = new Direction[2];
            switch (primary_direction)
            {
                case Direction.North:
                    other_directions[0] = Direction.East;
                    other_directions[1] = Direction.West;
                    break;
                case Direction.East:
                    other_directions[0] = Direction.South;
                    other_directions[1] = Direction.North;
                    break;
                case Direction.South:
                    other_directions[0] = Direction.East;
                    other_directions[1] = Direction.West;
                    break;
                case Direction.West:
                    other_directions[0] = Direction.North;
                    other_directions[1] = Direction.South;
                    break;
            }

            bool[] not_taken_other_direction = new bool[2] { true, true };

            Province current_location = SelectedProvince;
            bool not_has_found_destination = true;

            while (not_has_found_destination)
            {
                // Check if there are any lakes and/or rivers in this area, then let this new river flow into a random one.
                if (current_location.SecondaryTerrainFeatures.Exists(x => x.GetType() == typeof(Lake)) ||
                    current_location.SecondaryTerrainFeatures.Exists(y => y.GetType() == typeof(River)))
                {
                    List<TerrainFeatures> lakes_and_rivers = current_location.SecondaryTerrainFeatures.FindAll(x => x.GetType() == typeof(Lake) || x.GetType() == typeof(River));
                    TerrainFeatures destination = lakes_and_rivers[Constants.Random.Next(lakes_and_rivers.Count)];

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
                    // else go towards terrain...
                    int chance = Constants.Random.Next(100);
                    Direction next_direction = Direction.North;
                    // ...straight in primary direction
                    if (chance < 50)
                    {
                        next_direction = primary_direction;
                        not_taken_other_direction[0] = true;
                        not_taken_other_direction[1] = true;
                    }
                    // ... turn clock-wise from primary direction
                    else if (chance < 75 && not_taken_other_direction[0])
                    {
                        next_direction = other_directions[0];
                        not_taken_other_direction[1] = false;
                    }
                    // ... turn counter-clock-wise from primary direction
                    else if (chance < 100 && not_taken_other_direction[1])
                    {
                        next_direction = other_directions[1];
                        not_taken_other_direction[0] = false;
                    }

                    SystemCoordinates coords = null;
                    switch (next_direction)
                    {
                        case Direction.North:
                            coords = current_location.Coordinates.North;
                            break;
                        case Direction.East:
                            coords = current_location.Coordinates.East;
                            break;
                        case Direction.South:
                            coords = current_location.Coordinates.South;
                            break;
                        case Direction.West:
                            coords = current_location.Coordinates.West;
                            break;
                    }


                    if (coords.isInTileGridBounds())
                    {
                        // Assign next location
                        current_location = Program.State.ProvinceGrid[coords.X, coords.Y];
                    }
                    else
                        current_location = null;

                    if (current_location != null)
                    {
                        // if that new area is ocean the river ends here.
                        if (current_location.Type == TerrainType.Ocean || current_location.Type == TerrainType.Island)
                        {
                            river.Riverbed.Add(current_location);
                            river.Destination = current_location;
                            not_has_found_destination = false;
                        }
                        // otherwise the river runs further.
                        else
                        {
                            river.Riverbed.Add(current_location);
                        }

                    }
                    else // if current location is null then the border of the map has been found and the river ends in unknown land.
                    {
                        river.Destination = new Province(null, null);
                        river.Destination.Type = TerrainType.Unknown;
                        river.Destination.Name = "Unknown Land";
                        river.Riverbed.Add(river.Destination);
                        not_has_found_destination = false;
                    }
                }
            }

            // Add river to terrains at the end in order to avoid the river ending in itself.
            foreach (Province province in river.Riverbed)
            {
                if (province.Type != TerrainType.Unknown)
                    province.SecondaryTerrainFeatures.Add(river);
            }

            river.Modifiers.NaturalDefenceValue += 1;

            // Add river to deity list.
            creator.TerrainFeatures.Add(river);
            creator.LastCreation = river;

            Program.WorldHistory.AddRecord(river, river.printTerrainFeature);

            return 0;
        }

        enum Direction
        {
            North,
            East,
            South,
            West,
        }
    }
}
