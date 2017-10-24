using dawn_of_worlds.Creations.Civilisations;
using dawn_of_worlds.Effects;
using dawn_of_worlds.Log;
using dawn_of_worlds.Main;
using dawn_of_worlds.Names;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds
{
    class Program
    {
        public static World World { get; set; }
        public static History WorldHistory { get; set; }
        public static Logger Log { get; set; }
        public static Simulation Simulation { get; set; }
        public static NameGenerator GenerateNames { get; set; }

        static void Main(string[] args)
        {
            GenerateNames = new NameGenerator(@"C:\Users\Jonas Waeber\Documents\Projects\dawn_of_worlds\dawn_of_worlds\dawn_of_worlds\Names\NameSets", 121328);
            Log = new Logger(@"C:\Users\Jonas Waeber\Documents\Projects\dawn_of_worlds\dawn_of_worlds\dawn_of_worlds\bin\Debug\Output\");

            PolityDefinitions.DefinePolities();
            Diseases.DefineDiseases();
            World = new World(GenerateNames.GetName());
            World.initialize(5, 5);
            WorldHistory = new History();
            Simulation = new Simulation();

            WorldHistory.AddRecord(RecordType.TerrainMap, Map.generateTerrainMap(), Map.printMap);
            WorldHistory.AddRecord(RecordType.BiomeMap, Map.generateBiomeMap(), Map.printMap);
            WorldHistory.AddRecord(RecordType.ClimateMap, Map.generateClimateMap(), Map.printMap);


            Simulation.Run();

            Log.CleanOutputDirectory();
            Log.StoreInFile();

            Console.WriteLine("END OF APPLICATION");
            Console.ReadKey();
        }
    }
}
