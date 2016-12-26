using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Main;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Log
{
    class StreamWriters
    {
        public static string OutputDirectory { get; set; }

        static StreamWriters()
        {
            OutputDirectory = Directory.GetCurrentDirectory() + @"\Output\";
        }

        private static StreamWriter DeitiesWriter { get; set; }

        private static StreamWriter GeneralRecords { get; set; }
        private static StreamWriter RecordTypeWriter { get; set; }
        private static StreamWriter AreaWriter { get; set; }
        private static StreamWriter RaceWriter { get; set; }
        private static StreamWriter NationWriter { get; set; }
        private static StreamWriter WarWriter { get; set; }

        public static void cleanDirectories()
        {
            // clean all races for each run of the programm.
            foreach (string path in Directory.EnumerateDirectories(OutputDirectory + RACE_DIRECTORY))
            {
                foreach (string file_path in Directory.EnumerateFiles(path))
                    File.Delete(file_path);
 
                Directory.Delete(path);
            }

            // clean all nations for each run of the programm.
            foreach (string path in Directory.EnumerateDirectories(OutputDirectory + NATION_DIRECTORY))
            {
                foreach (string file_path in Directory.EnumerateFiles(path))
                    File.Delete(file_path);

                Directory.Delete(path);
            }
        }

        public static void writeDeities()
        {
            int counter = 0;
            foreach (Deity deity in Program.World.Deities)
            {
                DeitiesWriter = new StreamWriter(OutputDirectory + DEITIES + "deity_" + counter + ".log");
                DeitiesWriter.Write(deity.printDeity());
                DeitiesWriter.Close();
                counter++;
            }
        }

        public static void writeRaces()
        {
            foreach (Race race in Program.World.Races)
            {
                Directory.CreateDirectory(OutputDirectory + RACE_DIRECTORY + race.Name.Singular);
                RaceWriter = new StreamWriter(OutputDirectory + RACE_DIRECTORY + race.Name.Singular + @"\general.txt");
                RaceWriter.Write(race.printRace());
                RaceWriter.Close();


                RaceWriter = new StreamWriter(OutputDirectory + RACE_DIRECTORY + race.Name.Singular + @"\territroy_history.log");
                RaceWriter.Write(Program.WorldHistory.printRecordType(RecordType.RaceSettlementMap, race));
                RaceWriter.Close();
            }
        }

        public static void writeNations()
        {
            foreach (Nation nation in Program.World.Nations)
            {
                Directory.CreateDirectory(OutputDirectory + NATION_DIRECTORY + nation.Name.Singular);
                RaceWriter = new StreamWriter(OutputDirectory + NATION_DIRECTORY + nation.Name.Singular + @"\general.txt");
                RaceWriter.Write(nation.printNation());
                RaceWriter.Close();


                RaceWriter = new StreamWriter(OutputDirectory + NATION_DIRECTORY + nation.Name.Singular + @"\settlement_history.log");
                RaceWriter.Write(Program.WorldHistory.printRecordType(RecordType.NationTerritoryMap, nation));
                RaceWriter.Close();
            }
        }

        public static void writeRecords()
        {
            GeneralRecords = new StreamWriter(OutputDirectory + ALL_RECORDS);
            GeneralRecords.Write(Program.WorldHistory.printAllRecords());
            GeneralRecords.Close();
        }

        public static void writeRecordType(RecordType type)
        {
            RecordTypeWriter = new StreamWriter(OutputDirectory + RECORDS_BY_TYPE + type.ToString() + ".log");
            RecordTypeWriter.Write(Program.WorldHistory.printRecordType(type));
            RecordTypeWriter.Close();

        }

        private const string ACTION_LOG = @"action.log";
        private const string ALL_RECORDS = @"records.log";
        private const string DEITIES = @"\Deities\";
        private const string RACE_DIRECTORY = @"\Races\";
        private const string NATION_DIRECTORY = @"\Nations\";
        private const string WAR_DIRECTORY = @"\Diplomacy\Wars\";

        private const string RECORDS_BY_TYPE = @"\RecordsByType\";



    }
}
