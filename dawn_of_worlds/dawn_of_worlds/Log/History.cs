using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations;
using dawn_of_worlds.Creations.Geography;
using System.IO;

namespace dawn_of_worlds.Log
{
    class History
    {
        private StreamWriter writer { get; set; }
        public List<Record> Records { get; set; }


        public void AddRecord(TerrainFeatures terrain)
        {
            Record record = new Record();

            record.Type = RecordType.CreateTerrain;
            record.Terrain = terrain;
            record.Year = Simulation.Time.Shuffle;

            Records.Add(record);
        }

        public History()
        {
            Records = new List<Record>();
        }

        public void printWorldHistory(string file_name)
        {
            Records.Sort(Record.CompareTo);

            writer = new StreamWriter(Constants.HISTORY_FOLDER + file_name);

            foreach (Record record in Records)
                writer.WriteLine(record.printRecord());

            writer.Close();
        }


        public override string ToString()
        {
            Records.Sort(Record.CompareTo);

            string records = "";
            foreach (Record record in Records)
            {
                records += record.ToString() + "\n";
            }
            return records;
        }

    }


    class Record
    {
        public RecordType Type { get; set; }
        public TerrainFeatures Terrain { get; set; }

        public int Year { get; set; }

        static public int CompareTo(Record record_one, Record record_two)
        {
            if (record_one.Year > record_two.Year)
                return 1;
            else if (record_one.Year == record_two.Year)
                return 0;
            else // Year < record.Year
                return -1;
        }

        public string printRecord()
        {
            string result = "";
            switch (Type)
            {
                case RecordType.CreateTerrain:
                    result += "In " + Year.ToString() + " " + Terrain.Creator.Name + " created the " + Terrain.Name + " in " + Terrain.Location.Name;
                    break;
            }

            return result;
        }

        public override string ToString()
        {
            switch (Type)
            {
                case RecordType.CreateTerrain:
                    return Type.ToString() + " " + Year.ToString() + " " + Terrain.Name;
                case RecordType.ClimateChange:
                    return Type.ToString() + " " + Year.ToString();
                case RecordType.RelationChange:
                    return Type.ToString() + " " + Year.ToString();
                default:
                    return "";
            }
        }
    }

    enum RecordType
    {
        CreateTerrain,
        ClimateChange,
        RelationChange,
    }
}
