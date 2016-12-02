using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations;

namespace dawn_of_worlds.Log
{
    class History
    {
        public List<Record> Records { get; set; }


        public void AddRecord(Creation creation)
        {
            Record record = new Record();

            record.Type = RecordType.Creation;
            record.Creation = creation;
            record.Year = Simulation.Time.Shuffle;

            Records.Add(record);
        }

        public History()
        {
            Records = new List<Record>();
        }

        public string printWorldHistory()
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
        public Creation Creation { get; set; }
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

        public override string ToString()
        {
            return Type.ToString() + " " + Creation.Name + " " + Year.ToString();
        }
    }

    enum RecordType
    {
        Creation,
        ClimateChange,
        RelationChange,
    }
}
