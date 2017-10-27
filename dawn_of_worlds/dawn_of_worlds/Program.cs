using dawn_of_worlds.Creations.Civilisations;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Effects;
using dawn_of_worlds.Log;
using dawn_of_worlds.Main;
using dawn_of_worlds.Names;
using dawn_of_worlds.TagThesaurus;
using dawn_of_worlds.Utility;
using dawn_of_worlds.WorldModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds
{
    class Program
    {
        private static TemporaryClass temp { get; set; }

        public static Thesaurus Thesaurus { get; set; }

        public static GameState State { get; set; }
        public static History WorldHistory { get; set; }
        public static Logger Log { get; set; }
        public static Simulation Simulation { get; set; }
        public static NameGenerator GenerateNames { get; set; }

        static void Main(string[] args)
        {
            temp = new TemporaryClass();

            Thesaurus = new Thesaurus(@"C:\Users\Jonas Waeber\Documents\Projects\dawn_of_worlds\dawn_of_worlds\dawn_of_worlds\bin\Debug\Input\tag_thesaurus");
            Thesaurus.LoadTags();
            Thesaurus.CheckForDublicates();

            GenerateNames = new NameGenerator(@"C:\Users\Jonas Waeber\Documents\Projects\dawn_of_worlds\dawn_of_worlds\dawn_of_worlds\Names\NameSets", 121328);
            Log = new Logger(@"C:\Users\Jonas Waeber\Documents\Projects\dawn_of_worlds\dawn_of_worlds\dawn_of_worlds\bin\Debug\Output\");

            PolityDefinitions.DefinePolities();
            Diseases.DefineDiseases();
            DefinedRaces.defineRaces();

            State = new GameState();

            WorldGeneration.BasicWorldGeneration basic_world = new WorldGeneration.BasicWorldGeneration(10002212);
            basic_world.Initialize(5, 5);
            State.World = basic_world.World;

            Generators.DeityGenerator deity_generation = new Generators.DeityGenerator(100202);
            deity_generation.BasicGeneration();
            State.Deities = deity_generation.GeneratedDeities;

            WorldHistory = new History();
            Simulation = new Simulation();

            WorldHistory.AddRecord(RecordType.TerrainMap, Map.generateTerrainMap(), Map.printMap);
            WorldHistory.AddRecord(RecordType.BiomeMap, Map.generateBiomeMap(), Map.printMap);
            WorldHistory.AddRecord(RecordType.ClimateMap, Map.generateClimateMap(), Map.printMap);

            Simulation.Run();

            Log.CleanOutputDirectory();
            Log.StoreInFile();

            Thesaurus.PrintLog();

            Console.WriteLine("END OF APPLICATION");
            Console.ReadKey();
        }
    }
}
