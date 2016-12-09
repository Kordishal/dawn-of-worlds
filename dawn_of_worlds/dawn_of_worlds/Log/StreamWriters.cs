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
        private static StreamWriter GeneralRecords { get; set; }
        private static StreamWriter RecordTypeWriter { get; set; }
        private static StreamWriter AreaWriter { get; set; }
        private static StreamWriter RaceWriter { get; set; }

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

        private const string RECORDS_BY_TYPE = @"\RecordsByType\";
        
    }
}
