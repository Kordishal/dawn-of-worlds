using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Creations.Inhabitants
{
    class Race : Creation
    {
        public List<RaceTags> Tags { get; set; }

        public SpeciesType Type { get; set; }
        public RacialHabitat Habitat { get; set; }
        public List<RacialPreferredHabitatTerrain> PreferredTerrain { get; set; }
        public List<RacialPreferredHabitatClimate> PreferredClimate { get; set; }
        public RacialLifespan Lifespan { get; set; }
        public List<PhysicalTrait> PhysicalTraits { get; set; }
        public List<SocialCulturalCharacteristic> SocialCulturalCharacteristics { get; set; }

        public Area HomeArea { get; set; }
        public List<Area> SettledAreas { get; set; }

        public bool isSubRace { get; set; }
        public Race MainRace { get; set; }

        public List<Race> SubRaces { get; set; }
        public List<Race> PossibleSubRaces { get; set; }

        public Order OriginOrder { get; set; }

        public Race(string name, Deity creator) : base(name, creator)
        {
            SubRaces = new List<Race>();
            PossibleSubRaces = new List<Race>();
            SettledAreas = new List<Area>();
            Tags = new List<RaceTags>();

            PreferredTerrain = new List<RacialPreferredHabitatTerrain>();
            PreferredClimate = new List<RacialPreferredHabitatClimate>();
            PhysicalTraits = new List<PhysicalTrait>();
            SocialCulturalCharacteristics = new List<SocialCulturalCharacteristic>();
        }


        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return GetHashCode() == obj.GetHashCode();
        }
    }

    enum SpeciesType
    {
        Humanoid,
        Dragonoid,
    }

    enum RacialHabitat
    {
        Aviatic,
        Aquatic,
        Amphibious,
        Terranean,
        Subterranean
    }

    enum RacialPreferredHabitatTerrain
    {
        MountainDwellers,
        CaveDwellers,
        ForestDwellers,
        HillDwellers,
        PlainDwellers,
        DesertDwellers,
    }

    enum RacialPreferredHabitatClimate
    {
        ColdAcclimated,
        TemperateAcclimated,
        HeatAcclimated,
    }

    enum RacialLifespan
    {
        Immortal,
        EternalLife,
        Venerable,
        Enduring,
        Average,
        Fleeting,
    }

    enum PhysicalTrait
    {
        Winged,
        Strong,
        Weak,
        NaturalArmour,
        NaturalWeapons,
    }

    enum SocialCulturalCharacteristic
    {
        Communal,
        Nomadic,
        Sedentary,
    }

    enum RaceTags
    {
        RacialEpidemic,
    }
}
