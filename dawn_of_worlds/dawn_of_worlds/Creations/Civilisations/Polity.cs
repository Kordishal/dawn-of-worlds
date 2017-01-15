using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Effects;

namespace dawn_of_worlds.Creations.Civilisations
{
    class Polity : Creation
    {
        public Polity(string name, Deity creator) : base(name, creator)
        {
        }

        public SocialOrganisation Organisation { get; set; }
        public PolityForm Form { get; set; }
        public PowerSource Source { get; set; }
        public RulerType Ruler { get; set; }

        public bool isNomadic { get; set; }

        public string print()
        {
            string result = "";
            result += "Organisation: " + Organisation.ToString() + "\n";
            result += "Form: " + Form.ToString() + "\n";
            result += "Source: " + Source.ToString() + "\n";
            result += "Ruler: " + Ruler.ToString() + "\n";
            return result;
        }
    }

    struct PolityDefinitions
    {
        public static Polity BandSociety { get; set; }
        public static Polity DragonBrood { get; set; }

        public static void DefinePolities()
        {
            BandSociety = new Polity("Band Society", null);
            BandSociety.Organisation = SocialOrganisation.BandSociety;
            BandSociety.Form = PolityForm.Band;
            BandSociety.Source = PowerSource.Democratic;
            BandSociety.Ruler = RulerType.Gerontocracy;
            BandSociety.isNomadic = true;

            BandSociety.Tags = new List<CreationTag>() { CreationTag.Community, CreationTag.Elderly, CreationTag.Exploration };

            DragonBrood = new Polity("Brood", null);
            DragonBrood.Organisation = SocialOrganisation.BandSociety;
            DragonBrood.Form = PolityForm.Brood;
            DragonBrood.Source = PowerSource.Autocratic;
            DragonBrood.Ruler = RulerType.Kraterocracy;
            DragonBrood.isNomadic = false;

            DragonBrood.Tags = new List<CreationTag>() { };
        }
    }

    enum SocialOrganisation
    {
        BandSociety,
        TribalSociety,
        Chiefdom,
        State,
    }

    enum PolityForm
    {
        Band,
        Pack,
        Herd,
        Brood,
    }

    enum PowerSource
    {
        Democratic,
        Oligarchic,
        Autocratic,
    }

    enum RulerType
    {
        Gerontocracy, // Old People
        Kraterocracy, // Strong People
        Aristrocracy, // Noble People
        Stratocracy, // Military People
        Plutocracy, // Rich People
        Kritarchy, // Judge People
        Magocracy, // Wizard People
        Theocracy, // Religious People
    }
}
