using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Effects;

namespace dawn_of_worlds.Creations.Civilisations
{
    // TODO: Move polity definitions into files.

    [Serializable]
    class Polity : Creation
    {
        public Polity(string name, Deity creator) : base(name, creator) { }

        public SocialOrganisation Organisation { get; set; }
        public PolityForm Form { get; set; }
        public PowerSource Source { get; set; }
        public RulerType Ruler { get; set; }

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
        // Band Societies
        public static Polity BandSociety { get; set; }
        public static Polity DragonBrood { get; set; }

        // Tribal Societies
        public static Polity ClanCouncil { get; set; }
        public static Polity DespoticTribe { get; set; }
        public static Polity TribalKingdom { get; set; }

        public static void DefinePolities()
        {
            BandSociety = new Polity("Band Society", null);
            BandSociety.Organisation = SocialOrganisation.BandSociety;
            BandSociety.Form = PolityForm.Band;
            BandSociety.Source = PowerSource.Democratic;
            BandSociety.Ruler = RulerType.Gerontocracy;
            BandSociety.Tags = new List<string>() { "communty", "elderly", "exploration" };

            DragonBrood = new Polity("Brood", null);
            DragonBrood.Organisation = SocialOrganisation.BandSociety;
            DragonBrood.Form = PolityForm.Brood;
            DragonBrood.Source = PowerSource.Autocratic;
            DragonBrood.Ruler = RulerType.Kraterocracy;
            DragonBrood.Tags = new List<string>() { };

            ClanCouncil = new Polity("Clan Council", null);
            ClanCouncil.Organisation = SocialOrganisation.TribalSociety;
            ClanCouncil.Form = PolityForm.Tribe;
            ClanCouncil.Source = PowerSource.Oligarchic;
            ClanCouncil.Ruler = RulerType.Gerontocracy;
            ClanCouncil.Tags = new List<string>() { };

            DespoticTribe = new Polity("Despotic Tribe", null);
            DespoticTribe.Organisation = SocialOrganisation.TribalSociety;
            DespoticTribe.Form = PolityForm.Tribe;
            DespoticTribe.Source = PowerSource.Autocratic;
            DespoticTribe.Ruler = RulerType.Stratocracy;
            DespoticTribe.Tags = new List<string>() { };

            TribalKingdom = new Polity("Tribal Kingdom", null);
            TribalKingdom.Organisation = SocialOrganisation.TribalSociety;
            TribalKingdom.Form = PolityForm.Tribe;
            TribalKingdom.Source = PowerSource.Autocratic;
            TribalKingdom.Ruler = RulerType.Stratocracy;
            TribalKingdom.Tags = new List<string>() { };
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
        Tribe,
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
