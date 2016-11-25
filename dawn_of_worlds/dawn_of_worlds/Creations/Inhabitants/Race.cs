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

        public string printRace()
        {
            string result = "";
            result += "Name: " + Name + "\n";
            result += "Type: " + Type + "\n";
            result += "Habitat: " + Habitat + "\n";
            result += "Terrain: ";
            foreach (RacialPreferredHabitatTerrain terrain in PreferredTerrain)
                result += terrain.ToString() + ", ";
            result += "\n";
            result += "Climate: ";
            foreach (RacialPreferredHabitatClimate climate in PreferredClimate)
                result += climate.ToString() + ", ";
            result += "\n";
            result += "Lifespan: " + Lifespan + "\n";
            result += "Physical Traits: ";
            foreach (PhysicalTrait traits in PhysicalTraits)
                result += traits.ToString() + ", ";
            result += "\n";
            result += "Social & Cultural Traits: ";
            foreach (SocialCulturalCharacteristic social in SocialCulturalCharacteristics)
                result += social.ToString() + ", ";
            result += "\n";
            result += "Home Area: " + HomeArea + "\n";
            result += "Settled Areas: ";
            foreach (Area area in SettledAreas)
                result += area.ToString() + ", ";
            result += "\n";
            result += "Origin Order: " + OriginOrder + "\n";
            return result;
        }
    }

    enum SpeciesType
    {
        Humanoid,
        Dragonoid,
        Beasts,
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
        Territorial,
        Communal,
        Tribal,
        Nomadic,
        Sedentary,
    }

    enum RaceTags
    {
        RacialEpidemic,
    }
}
