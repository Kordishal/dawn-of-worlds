using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Inhabitants;
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
        private static StreamWriter DeitiesWriter { get; set; }

        private static StreamWriter GeneralRecords { get; set; }
        private static StreamWriter RecordTypeWriter { get; set; }
        private static StreamWriter AreaWriter { get; set; }
        private static StreamWriter RaceWriter { get; set; }

        public static void cleanDirectories()
        {
            // clean all races for each run of the programm.
            foreach (string path in Directory.EnumerateDirectories(OUTPUT_FOLDER + RACE_DIRECTORY))
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
                DeitiesWriter = new StreamWriter(OUTPUT_FOLDER + DEITIES + "deity_" + counter + ".log");
                DeitiesWriter.Write(deity.printDeity());
                DeitiesWriter.Close();
                counter++;
            }
        }

        public static void writeRaces()
        {
            foreach (Race race in Program.World.Races)
            {
                Directory.CreateDirectory(OUTPUT_FOLDER + RACE_DIRECTORY + race.Name);
                RaceWriter = new StreamWriter(OUTPUT_FOLDER + RACE_DIRECTORY + race.Name + @"\general.txt");
                RaceWriter.Write(race.printRace());
                RaceWriter.Close();


                RaceWriter = new StreamWriter(OUTPUT_FOLDER + RACE_DIRECTORY + race.Name + @"\settlement_history.log");
                RaceWriter.Write(Program.WorldHistory.printRecordType(RecordType.RaceSettlementMap, race));
                RaceWriter.Close();
            }
        }

        public static void writeRecords()
        {
            GeneralRecords = new StreamWriter(OUTPUT_FOLDER + ALL_RECORDS);
            GeneralRecords.Write(Program.WorldHistory.printAllRecords());
            GeneralRecords.Close();
        }

        public static void writeRecordType(RecordType type)
        {
            RecordTypeWriter = new StreamWriter(OUTPUT_FOLDER + RECORDS_BY_TYPE + type.ToString() + ".log");
            RecordTypeWriter.Write(Program.WorldHistory.printRecordType(type));
            RecordTypeWriter.Close();

        }

        private const string OUTPUT_FOLDER = @"C:\Users\Jonas Waeber\Documents\Projects\dawn_of_worlds\dawn_of_worlds\dawn_of_worlds\Log\Output\";
        private const string ACTION_LOG = @"action.log";
        private const string ALL_RECORDS = @"records.log";
        private const string DEITIES = @"\Deities\";
        private const string RACE_DIRECTORY = @"\Races\";

        private const string RECORDS_BY_TYPE = @"\RecordsByType\";



    }
}
