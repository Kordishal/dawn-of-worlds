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
        }



        public string printWorldHistory()
        {
            return "Hello World";
        }
    }


    class Record
    {
        public RecordType Type { get; set; }
        public Creation Creation { get; set; }
        public int Year { get; set; }
    }

    enum RecordType
    {
        Creation,
        ClimateChange,
        RelationChange,
    }
}
