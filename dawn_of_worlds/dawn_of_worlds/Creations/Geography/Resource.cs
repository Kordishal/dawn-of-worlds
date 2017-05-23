using System;
using System.Collections.Generic;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Main;
using dawn_of_worlds.WorldClasses;

namespace dawn_of_worlds.Creations.Geography
{

    /// <summary>
    /// A natural resource which can be found in a specific province. Each province can have several
    /// resources at the same time. The resources can increase or decrease the value of a province.
    /// 
    /// The resources are added by the deities on creation. Certain resources are restricted to specific 
    /// terrains and climates.
    /// </summary>
    class Resource : Creation
    {

        public ResourceTypes Type { get; set; }

        public double Richness { get; set; }
        public bool isDepleted { get; set; }

        public Province Province { get; set; }

        public Resource(string name, Deity creator, Province province, ResourceTypes type, double richness) : base(name, creator)
        {
            Province = province;
            Type = type;
            Richness = richness;
            isDepleted = false;
        }
    }

    enum ResourceTypes
    {
        Minerals,
        Vegetation,
        AnimalPopulations,
    }
}
