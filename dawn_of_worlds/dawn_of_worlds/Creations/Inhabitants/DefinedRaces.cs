using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.Creations.Inhabitants
{
    class DefinedRaces
    {
        public static List<Race> DefinedRacesList { get; set; }

        public static Race Humans { get; set; }
        public static Race Norse { get; set; }

        public static Race HighElves { get; set; }
        public static Race DarkElves { get; set; }

        public static Race ProtoDragons { get; set; }
        public static Race FireDragons { get; set; }

        public static Race MountainDwarves { get; set; }
        public static Race HillDwarves { get; set; }

        public static Race Giants { get; set; }
        public static Race HillGiants { get; set; }

        public DefinedRaces()
        {
            
        }

        public static void defineRaces()
        {
            DefinedRacesList = new List<Race>();
            defineHumanRaces();
            defineElfishRaces();
            defineDragonicRaces();
            defineDwarvenRaces();
        }


        private static void defineHumanRaces()
        {
            Humans = new Race("Humans", null);
            Humans.Type = SpeciesType.Humanoid;
            Humans.Habitat = RacialHabitat.Terranean;
            Humans.Lifespan = RacialLifespan.Average;
            Humans.PreferredTerrain.Add(RacialPreferredHabitatTerrain.PlainDwellers);
            Humans.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Nomadic);
            Humans.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Communal);

            DefinedRacesList.Add(Humans);

            Norse = new Race("Norse", null);
            Norse.isSubRace = true;
            Norse.MainRace = Humans;
            Norse.Type = SpeciesType.Humanoid;
            Norse.Habitat = RacialHabitat.Terranean;
            Norse.Lifespan = RacialLifespan.Average;
            Norse.PreferredTerrain.Add(RacialPreferredHabitatTerrain.PlainDwellers);
            Norse.PreferredClimate.Add(RacialPreferredHabitatClimate.ColdAcclimated);
            Norse.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Nomadic);
            Norse.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Communal);

            DefinedRacesList.Add(Norse);

            Humans.PossibleSubRaces.Add(Norse);
        }

        private static void defineElfishRaces()
        {
            HighElves = new Race("High Elves", null);
            HighElves.Type = SpeciesType.Humanoid;
            HighElves.Habitat = RacialHabitat.Terranean;
            HighElves.Lifespan = RacialLifespan.Venerable;
            HighElves.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Communal);
            HighElves.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Sedentary);

            DefinedRacesList.Add(HighElves);

            DarkElves = new Race("Dark Elves", null);
            DarkElves.isSubRace = true;
            DarkElves.MainRace = HighElves;
            DarkElves.Type = SpeciesType.Humanoid;
            DarkElves.Habitat = RacialHabitat.Subterranean;
            DarkElves.Lifespan = RacialLifespan.Venerable;
            DarkElves.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Communal);
            DarkElves.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Sedentary);

            DefinedRacesList.Add(DarkElves);

            HighElves.PossibleSubRaces.Add(DarkElves);
        }

        private static void defineDragonicRaces()
        {
            ProtoDragons = new Race("Protodragons", null);
            ProtoDragons.Type = SpeciesType.Dragonoid;
            ProtoDragons.Habitat = RacialHabitat.Terranean;
            ProtoDragons.Lifespan = RacialLifespan.EternalLife;
            ProtoDragons.PhysicalTraits.Add(PhysicalTrait.Winged);
            ProtoDragons.PhysicalTraits.Add(PhysicalTrait.Strong);
            ProtoDragons.PhysicalTraits.Add(PhysicalTrait.NaturalArmour);
            ProtoDragons.PhysicalTraits.Add(PhysicalTrait.NaturalWeapons);
            ProtoDragons.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Nomadic);

            DefinedRacesList.Add(ProtoDragons);

            FireDragons = new Race("Fire Dragons", null);
            FireDragons.isSubRace = true;
            FireDragons.MainRace = ProtoDragons;
            FireDragons.Type = SpeciesType.Dragonoid;
            FireDragons.Habitat = RacialHabitat.Terranean;
            FireDragons.Lifespan = RacialLifespan.EternalLife;
            FireDragons.PreferredClimate.Add(RacialPreferredHabitatClimate.HeatAcclimated);
            FireDragons.PhysicalTraits.Add(PhysicalTrait.Winged);
            FireDragons.PhysicalTraits.Add(PhysicalTrait.Strong);
            FireDragons.PhysicalTraits.Add(PhysicalTrait.NaturalArmour);
            FireDragons.PhysicalTraits.Add(PhysicalTrait.NaturalWeapons);
            FireDragons.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Nomadic);

            DefinedRacesList.Add(FireDragons);

            ProtoDragons.PossibleSubRaces.Add(FireDragons);
        }

        private static void defineDwarvenRaces()
        {
            MountainDwarves = new Race("Mountain Dwarves", null);
            MountainDwarves.Type = SpeciesType.Humanoid;
            MountainDwarves.Habitat = RacialHabitat.Terranean;
            MountainDwarves.Lifespan = RacialLifespan.Enduring;
            MountainDwarves.PreferredTerrain.Add(RacialPreferredHabitatTerrain.MountainDwellers);
            MountainDwarves.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Communal);
            MountainDwarves.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Sedentary);

            DefinedRacesList.Add(MountainDwarves);

            HillDwarves = new Race("Hill Dwarves", null);
            HillDwarves.isSubRace = true;
            HillDwarves.MainRace = MountainDwarves;
            HillDwarves.Type = SpeciesType.Humanoid;
            HillDwarves.Habitat = RacialHabitat.Terranean;
            HillDwarves.Lifespan = RacialLifespan.Enduring;
            HillDwarves.PreferredTerrain.Add(RacialPreferredHabitatTerrain.HillDwellers);
            HillDwarves.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Communal);
            HillDwarves.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Sedentary);

            DefinedRacesList.Add(HillDwarves);

            MountainDwarves.PossibleSubRaces.Add(HillDwarves);
        }

        private void defineGiantRaces()
        {
            Giants = new Race("Giants", null);
            Giants.Type = SpeciesType.Humanoid;
            Giants.Habitat = RacialHabitat.Terranean;
            Giants.Lifespan = RacialLifespan.Enduring;
            Giants.PhysicalTraits.Add(PhysicalTrait.Strong);

            DefinedRacesList.Add(Giants);

            HillGiants = new Race("Hill Giants", null);
            HillGiants.isSubRace = true;
            HillGiants.MainRace = Giants;
            HillGiants.Type = SpeciesType.Humanoid;
            HillGiants.Habitat = RacialHabitat.Terranean;
            HillGiants.Lifespan = RacialLifespan.Enduring;
            HillGiants.PhysicalTraits.Add(PhysicalTrait.Strong);
            HillGiants.PreferredTerrain.Add(RacialPreferredHabitatTerrain.HillDwellers);

            DefinedRacesList.Add(HillGiants);

            Giants.PossibleSubRaces.Add(HillGiants);
        }
    }
}
