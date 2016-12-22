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
        public static StreamWriters Log { get; set; }
        public static Simulation Simulation { get; set; }

        static void Main(string[] args)
        {
            Constants.Names = new NameGenerator();
            Log = new StreamWriters();
            World = new World(Constants.Names.GetName("world"), 5, 5);
            WorldHistory = new History();
            Simulation = new Simulation();

            WorldHistory.AddRecord(RecordType.TerrainMap, Map.generateTerrainMap(), Map.printMap);
            WorldHistory.AddRecord(RecordType.BiomeMap, Map.generateBiomeMap(), Map.printMap);
            WorldHistory.AddRecord(RecordType.ClimateMap, Map.generateClimateMap(), Map.printMap);


            Simulation.Run();

            StreamWriters.cleanDirectories();

            StreamWriters.writeDeities();

            StreamWriters.writeRecords();
            StreamWriters.writeRecordType(RecordType.TerrainMap);
            StreamWriters.writeRecordType(RecordType.BiomeMap);
            StreamWriters.writeRecordType(RecordType.ClimateMap);
            StreamWriters.writeRecordType(RecordType.CreateTerrainFeature);

            StreamWriters.writeRaces();

            Console.WriteLine("END OF APPLICATION");
            Console.ReadKey();
        }
    }
}
