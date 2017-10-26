using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using System.IO;
using Newtonsoft.Json;

namespace dawn_of_worlds.Creations.Inhabitants
{
    class DefinedRaces
    {
        public static List<Race> DefinedRacesList { get; set; }

        public static Race Humans { get; set; }
        public static Race Norse { get; set; }

        public static Race HighElves { get; set; }
        public static Race DarkElves { get; set; }
        public static Race WoodElves { get; set; }
        public static Race WildElves { get; set; }

        public static Race Eladrins { get; set; }

        public static Race ProtoDragons { get; set; }
        public static Race FireDragons { get; set; }
        public static Race WaterDragons { get; set; }
        public static Race AirDragons { get; set; }
        public static Race EarthDragons { get; set; }
        public static Race LighningDragons { get; set; }
        public static Race CloudDragons { get; set; }
        public static Race StromDragons { get; set; }
        public static Race FrostDragons { get; set; }
        public static Race SnowDragons { get; set; }
        public static Race SandDragons { get; set; }
        public static Race SwampDragons { get; set; }
        public static Race RedDragons { get; set; }
        public static Race BlackDragons { get; set; }
        public static Race WhiteDragons { get; set; }
        public static Race BlueDragons { get; set; }
        public static Race GreenDragons { get; set; }
        public static Race AdamantineDragons { get; set; }
        public static Race GoldDragons { get; set; }
        public static Race SilverDragons { get; set; }
        public static Race IronDragons { get; set; }
        public static Race BronzeDragons { get; set; }
        public static Race CopperDragons { get; set; }
        public static Race SerpentDragons { get; set; }

        public static Race MountainDwarves { get; set; }
        public static Race HillDwarves { get; set; }
        public static Race Azers { get; set; }
        public static Race DeepDwarves { get; set; }
        public static Race GoldDwarves { get; set; }
        public static Race Duergar { get; set; }

        public static Race Gnomes { get; set; }

        public static Race Halflings { get; set; }

        public static Race Harpies { get; set; }
        public static Race Lizardfolk { get; set; }
        public static Race Minotaurs { get; set; }
        public static Race Nagas { get; set; }
        public static Race Rakshasas { get; set; }
        public static Race Satyrs { get; set; }
        public static Race Troglodytes { get; set; }
        public static Race YuanTis { get; set; }
        public static Race Centaurs { get; set; }
        public static Race Drakkoths { get; set; }
        public static Race Firbolgs { get; set; }
        public static Race Goliaths { get; set; }

        public static Race Djinns { get; set; }

        public static Race Trolls { get; set; }
        public static Race IceTrolls { get; set; }
        public static Race SwampTrolls { get; set; }
        public static Race StoneTrolls { get; set; }

        public static Race Giants { get; set; }
        public static Race HillGiants { get; set; }
        public static Race StormGiants { get; set; }
        public static Race FireGiants { get; set; }
        public static Race StoneGiants { get; set; }
        public static Race DeathGiants { get; set; }
        public static Race FrostGiants { get; set; }
        public static Race EldritchGiant { get; set; }

        public static Race Titans { get; set; }
        public static Race EarthTitans { get; set; }
        public static Race StormTitans { get; set; }
        public static Race FireTitans { get; set; }
        public static Race StoneTitans { get; set; }
        public static Race DeathTitans { get; set; }
        public static Race FrostTitans { get; set; }
        public static Race EldritchTitans { get; set; }
        
        public static Race Cyclops { get; set; }
        public static Race Ettin { get; set; }
        public static Race Fomorians { get; set; }

        public static Race Dryads { get; set; }
        public static Race Treants { get; set; }
        public static Race Ents { get; set; }

        public static Race Orcs { get; set; }

        public static Race Goblins { get; set; }
        public static Race Hobgoblins { get; set; }
        public static Race Bugbears { get; set; }

        public static Race Kobolds { get; set; }
        public static Race Gnolls { get; set; }
        public static Race Ogres { get; set; }

        public static Race DireWolves { get; set; }
        public static Race DireBears { get; set; }
        public static Race DireBoars { get; set; }
        public static Race DireTigers { get; set; }
        public static Race DireLions { get; set; }
        public static Race DirePanthers { get; set; }
        public static Race DireHyenas { get; set; }
        public static Race DireCorcodiles { get; set; }
        public static Race DireRats { get; set; }

        public static Race GiantWolves { get; set; }
        public static Race GiantBears { get; set; }
        public static Race GiantBoars { get; set; }
        public static Race GiantLynxes { get; set; }
        public static Race GiantTigers { get; set; }
        public static Race GiantLions { get; set; }
        public static Race GiantHyenas { get; set; }
        public static Race GiantPanthers { get; set; }
        public static Race GiantCrocodiles { get; set; }
        public static Race GiantEagles { get; set; }
        public static Race GiantOwls { get; set; }
        public static Race GiantHawks { get; set; }
        public static Race GiantBats { get; set; }
        public static Race GiantRats { get; set; }
        public static Race GiantScropions { get; set; }
        public static Race GiantSnakes { get; set; }
        public static Race GiantSpiders { get; set; }
        public static Race GiantAnts { get; set; }

        public static Race Rocs { get; set; }
        public static Race Pheonix { get; set; }
        public static Race ThunderHawk { get; set; }
        public static Race Unicorns { get; set; }
        public static Race Griffons { get; set; }
        public static Race Hydras { get; set; }
        public static Race Sphinxes { get; set; }
        public static Race Wyverns { get; set; }
        public static Race WinterWolves { get; set; }

        public static Race Dragonborns { get; set; }
        public static Race Doppelgangers { get; set; }

        public static Race EarthElementals { get; set; }
        public static Race WaterElementals { get; set; }
        public static Race FireElementals { get; set; }
        public static Race AirElementals { get; set; }


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
            defineOrcishRaces();
            defineGiantRaces();

            foreach (var race in DefinedRacesList)
            {
                var writer = new StreamWriter(race.Name + ".json");
                writer.Write(JsonConvert.SerializeObject(race, Formatting.Indented));
                writer.Close();
            }
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
            Norse.Type = SpeciesType.Humanoid;
            Norse.Habitat = RacialHabitat.Terranean;
            Norse.Lifespan = RacialLifespan.Average;
            Norse.PreferredTerrain.Add(RacialPreferredHabitatTerrain.PlainDwellers);
            Norse.PreferredClimate.Add(RacialPreferredHabitatClimate.Arctic);
            Norse.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Nomadic);
            Norse.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Communal);

            DefinedRacesList.Add(Norse);
        }
        private static void defineElfishRaces()
        {
            HighElves = new Race("High Elves", null);
            HighElves.Type = SpeciesType.Humanoid;
            HighElves.Habitat = RacialHabitat.Terranean;
            HighElves.Lifespan = RacialLifespan.Venerable;
            HighElves.PreferredClimate.Add(RacialPreferredHabitatClimate.Temperate);
            HighElves.PreferredClimate.Add(RacialPreferredHabitatClimate.Subtropical);
            HighElves.PreferredClimate.Add(RacialPreferredHabitatClimate.Tropical);
            HighElves.PreferredTerrain.Add(RacialPreferredHabitatTerrain.ForestDwellers);
            HighElves.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Communal);
            HighElves.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Sedentary);

            DefinedRacesList.Add(HighElves);

            DarkElves = new Race("Dark Elves", null);
            DarkElves.Type = SpeciesType.Humanoid;
            DarkElves.Habitat = RacialHabitat.Subterranean;
            DarkElves.Lifespan = RacialLifespan.Venerable;
            DarkElves.PreferredTerrain.Add(RacialPreferredHabitatTerrain.CaveDwellers);
            DarkElves.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Communal);
            DarkElves.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Sedentary);

            DefinedRacesList.Add(DarkElves);

            WoodElves = new Race("Wood Elves", null);
            WoodElves.Type = SpeciesType.Humanoid;
            WoodElves.Habitat = RacialHabitat.Terranean;
            WoodElves.Lifespan = RacialLifespan.Venerable;
            WoodElves.PreferredClimate.Add(RacialPreferredHabitatClimate.Temperate);
            WoodElves.PreferredClimate.Add(RacialPreferredHabitatClimate.Subtropical);
            WoodElves.PreferredClimate.Add(RacialPreferredHabitatClimate.Tropical);
            WoodElves.PreferredTerrain.Add(RacialPreferredHabitatTerrain.ForestDwellers);
            WoodElves.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Communal);
            WoodElves.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Sedentary);

            DefinedRacesList.Add(WoodElves);

            WildElves = new Race("Wild Elves", null);
            WildElves.Type = SpeciesType.Humanoid;
            WildElves.Habitat = RacialHabitat.Terranean;
            WildElves.Lifespan = RacialLifespan.Venerable;
            WildElves.PreferredTerrain.Add(RacialPreferredHabitatTerrain.ForestDwellers);
            WildElves.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Communal);
            WildElves.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Sedentary);

            DefinedRacesList.Add(WildElves);
        }
        private static void defineDragonicRaces()
        {
            ProtoDragons = new Race("Protodragons", null);
            ProtoDragons.Type = SpeciesType.Dragon;
            ProtoDragons.Habitat = RacialHabitat.Terranean;
            ProtoDragons.Lifespan = RacialLifespan.EternalLife;
            ProtoDragons.PhysicalTraits.Add(PhysicalTrait.Winged);
            ProtoDragons.PhysicalTraits.Add(PhysicalTrait.Strong);
            ProtoDragons.PhysicalTraits.Add(PhysicalTrait.NaturalArmour);
            ProtoDragons.PhysicalTraits.Add(PhysicalTrait.NaturalWeapons);
            ProtoDragons.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Nomadic);

            DefinedRacesList.Add(ProtoDragons);

            FireDragons = new Race("Fire Dragons", null);
            FireDragons.Type = SpeciesType.Dragon;
            FireDragons.Habitat = RacialHabitat.Terranean;
            FireDragons.Lifespan = RacialLifespan.EternalLife;
            FireDragons.PreferredClimate.Add(RacialPreferredHabitatClimate.Tropical);
            FireDragons.PhysicalTraits.Add(PhysicalTrait.Winged);
            FireDragons.PhysicalTraits.Add(PhysicalTrait.Strong);
            FireDragons.PhysicalTraits.Add(PhysicalTrait.NaturalArmour);
            FireDragons.PhysicalTraits.Add(PhysicalTrait.NaturalWeapons);
            FireDragons.PhysicalTraits.Add(PhysicalTrait.ImmunityFire);
            FireDragons.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Nomadic);

            DefinedRacesList.Add(FireDragons);

            GoldDragons = new Race("Gold Dragons", null);
            GoldDragons.Type = SpeciesType.Dragon;
            GoldDragons.Habitat = RacialHabitat.Terranean;
            GoldDragons.Lifespan = RacialLifespan.EternalLife;
            GoldDragons.PhysicalTraits.Add(PhysicalTrait.Winged);
            GoldDragons.PhysicalTraits.Add(PhysicalTrait.Strong);
            GoldDragons.PhysicalTraits.Add(PhysicalTrait.NaturalArmour);
            GoldDragons.PhysicalTraits.Add(PhysicalTrait.NaturalWeapons);
            GoldDragons.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Nomadic);

            DefinedRacesList.Add(GoldDragons);
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
            HillDwarves.Type = SpeciesType.Humanoid;
            HillDwarves.Habitat = RacialHabitat.Terranean;
            HillDwarves.Lifespan = RacialLifespan.Enduring;
            HillDwarves.PreferredTerrain.Add(RacialPreferredHabitatTerrain.HillDwellers);
            HillDwarves.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Communal);
            HillDwarves.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Sedentary);

            DefinedRacesList.Add(HillDwarves);
        }
        private static void defineGiantRaces()
        {
            Giants = new Race("Giant", null);
            Giants.Type = SpeciesType.Humanoid;
            Giants.Habitat = RacialHabitat.Terranean;
            Giants.Lifespan = RacialLifespan.Enduring;
            Giants.PhysicalTraits.Add(PhysicalTrait.Strong);

            DefinedRacesList.Add(Giants);

            HillGiants = new Race("Hill Giants", null);
            HillGiants.Type = SpeciesType.Humanoid;
            HillGiants.Habitat = RacialHabitat.Terranean;
            HillGiants.Lifespan = RacialLifespan.Enduring;
            HillGiants.PhysicalTraits.Add(PhysicalTrait.Strong);
            HillGiants.PreferredTerrain.Add(RacialPreferredHabitatTerrain.HillDwellers);

            DefinedRacesList.Add(HillGiants);

            StormGiants = new Race("Storm Giants", null);
            StormGiants.Type = SpeciesType.Humanoid;
            StormGiants.Habitat = RacialHabitat.Terranean;
            StormGiants.Lifespan = RacialLifespan.Enduring;
            StormGiants.PhysicalTraits.Add(PhysicalTrait.Strong);
            StormGiants.PreferredTerrain.Add(RacialPreferredHabitatTerrain.MountainDwellers);

            DefinedRacesList.Add(StormGiants);
        }
        private static void defineOrcishRaces()
        {
            Orcs = new Race("Orcs", null);
            Orcs.Type = SpeciesType.Humanoid;
            Orcs.Habitat = RacialHabitat.Terranean;
            Orcs.Lifespan = RacialLifespan.Average;
            Orcs.PhysicalTraits.Add(PhysicalTrait.Strong);
            Orcs.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Tribal);

            DefinedRacesList.Add(Orcs);
        }
        private static void defineGoblinoidRaces()
        {
            Goblins = new Race("Goblin", null);
            Goblins.Type = SpeciesType.Humanoid;
            Goblins.Habitat = RacialHabitat.Terranean;
            Goblins.Lifespan = RacialLifespan.Fleeting;
            Goblins.PhysicalTraits.Add(PhysicalTrait.Weak);
            Goblins.PreferredClimate.Add(RacialPreferredHabitatClimate.Temperate);
            Goblins.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Tribal);

            DefinedRacesList.Add(Goblins);

            Hobgoblins = new Race("Hobgoblin", null);
            Hobgoblins.Type = SpeciesType.Humanoid;
            Hobgoblins.Habitat = RacialHabitat.Terranean;
            Hobgoblins.Lifespan = RacialLifespan.Average;
            Hobgoblins.PhysicalTraits.Add(PhysicalTrait.Strong);
            Hobgoblins.PreferredClimate.Add(RacialPreferredHabitatClimate.Temperate);
            Hobgoblins.SocialCulturalCharacteristics.Add(SocialCulturalCharacteristic.Tribal);

            DefinedRacesList.Add(Hobgoblins);

            Bugbears = new Race("Bugbear", null);
            Bugbears.Type = SpeciesType.Humanoid;
            Bugbears.Habitat = RacialHabitat.Terranean;
            Bugbears.Lifespan = RacialLifespan.Average;
            Bugbears.PhysicalTraits.Add(PhysicalTrait.Strong);
            Bugbears.PreferredClimate.Add(RacialPreferredHabitatClimate.Temperate);

            DefinedRacesList.Add(Bugbears);
        }
        private static void defineGiantAnimals()
        {
            GiantBears = new Race("Bear", null);
            GiantBears.Type = SpeciesType.Beasts;
            GiantBears.Habitat = RacialHabitat.Terranean;
            GiantBears.Lifespan = RacialLifespan.Enduring;
            GiantBears.PhysicalTraits.Add(PhysicalTrait.Strong);
            GiantBears.PhysicalTraits.Add(PhysicalTrait.NaturalWeapons);
        }


    }
}
