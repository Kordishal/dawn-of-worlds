using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Modifiers;
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
        public int YearofCreation { get; set; }

        public List<Modifier> Modifiers { get; set; }

        public List<CreationTag> Tags { get; set; }

        public SpeciesType Type { get; set; }
        public RacialHabitat Habitat { get; set; }
        public List<RacialPreferredHabitatTerrain> PreferredTerrain { get; set; }
        public List<RacialPreferredHabitatClimate> PreferredClimate { get; set; }
        public RacialLifespan Lifespan { get; set; }
        public List<PhysicalTrait> PhysicalTraits { get; set; }
        public List<SocialCulturalCharacteristic> SocialCulturalCharacteristics { get; set; }

        public Province HomeProvince { get { return SettledProvinces[0]; } }
        public List<Province> SettledProvinces { get; set; }

        public Order OriginOrder { get; set; }

        public Race(string name, Deity creator) : base(name, creator)
        {
            Modifiers = new List<Modifier>();
            Tags = new List<CreationTag>();
            SettledProvinces = new List<Province>();

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
            result += "Name: " + Name.ToString() + "\n";
            result += "Year of Creation: " + YearofCreation + "\n";
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
            result += "Home Area: " + HomeProvince + "\n";
            result += "Settled Areas: ";
            foreach (Province terrain in SettledProvinces)
                result += terrain.ToString() + ", ";
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
        GrasslandDwellers,
    }

    enum RacialPreferredHabitatClimate
    {
        Arctic,
        Temperate,
        Tropical,
        Subarctic,
        Subtropical,
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
        ImmunityFire,
    }

    enum SocialCulturalCharacteristic
    {
        Territorial,
        Communal,
        Tribal,
        Nomadic,
        Sedentary,
        Elitist,
    }

    enum RaceTags
    {
        RacialEpidemic,
    }
}
