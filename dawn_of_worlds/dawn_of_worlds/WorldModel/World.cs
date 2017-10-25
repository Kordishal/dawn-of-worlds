using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Main;
using System;
using System.Collections.Generic;

namespace dawn_of_worlds.WorldModel
{

    // TODO: Extract world generation from model class into generator class.
    // TODO: Simplyfy world generation/change algorithm.
    // TODO: Improve world naming.
    // TODO: Create a Graph for the world.

    [Serializable]
    class World
    {
        public string Name { get; set; }

        public List<Region> Regions { get; set; }

        public World(string world_name)
        {
            Name = world_name;
        }
    }
}
