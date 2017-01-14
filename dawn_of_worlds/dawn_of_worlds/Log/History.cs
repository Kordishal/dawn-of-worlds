using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations;
using dawn_of_worlds.Creations.Geography;
using System.IO;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Creations.Diplomacy;
using dawn_of_worlds.Creations.Conflict;
using dawn_of_worlds.Creations.Civilisations;

namespace dawn_of_worlds.Log
{
    delegate string PrintFunction(Record record);

    class History
    {
        private StreamWriter writer { get; set; }
        public List<Record> Records { get; set; }


        public void AddRecord(TerrainFeatures terrain, PrintFunction print)
        {
            Record record = new Record();

            record.Type = RecordType.CreateTerrainFeature;
            record.Terrain = terrain;

            record.Turn = Simulation.Time.Turn;
            record.Year = Simulation.Time.Shuffle;

            record.printFunction = print;
            Records.Add(record);
        }

        public void AddRecord(RecordType type, char[,] map, PrintFunction print)
        {
            Record record = new Record();
            record.Type = type;
            record.Map = map;

            record.Turn = Simulation.Time.Turn;
            record.Year = Simulation.Time.Shuffle;

            record.printFunction = print;
            Records.Add(record);
        }

        public void AddRecord(RecordType type, Race race, char[,] map, PrintFunction print)
        {
            Record record = new Record();
            record.Type = type;
            record.Race = race;
            record.Map = map;

            record.Turn = Simulation.Time.Turn;
            record.Year = Simulation.Time.Shuffle;

            record.printFunction = print;
            Records.Add(record);
        }

        public void AddRecord(RecordType type, Civilisation nation, char[,] map, PrintFunction print)
        {
            Record record = new Record();
            record.Type = type;
            record.Nation = nation;
            record.Map = map;

            record.Turn = Simulation.Time.Turn;
            record.Year = Simulation.Time.Shuffle;

            record.printFunction = print;
            Records.Add(record);
        }

        public void AddRecord(RecordType type, War war, PrintFunction print)
        {
            Record record = new Record();
            record.Type = type;
            record.War = war;

            record.Turn = Simulation.Time.Turn;
            record.Year = war.Begin;

            record.printFunction = print;
            Records.Add(record);
        }

        public void AddRecord(RecordType type, War war, Battle battle, PrintFunction print)
        {
            Record record = new Record();
            record.Type = type;
            record.War = war;
            record.Battle = battle;

            record.Turn = Simulation.Time.Turn;
            record.Year = war.Begin;

            record.printFunction = print;
            Records.Add(record);
        }

        public History()
        {
            Records = new List<Record>();
        }


        public string printAllRecords()
        {
            Records.Sort(Record.CompareTo);

            string records = "";
            foreach (Record record in Records)
            {
                records += record.printRecord() + "\n";
            }
            return records;
        }
        public string printRecordType(RecordType type)
        {
            List<Record> selected_records = Records.FindAll(x => x.Type == type);
            selected_records.Sort(Record.CompareTo);

            string records = "";
            foreach (Record record in selected_records)
            {
                records += record.printRecord() + "\n";
            }
            return records;
        }
        public string printRecordType(RecordType type, Race race)
        {
            List<Record> selected_records = Records.FindAll(x => x.Race != null && x.Type == type && x.Race.Equals(race));
            selected_records.Sort(Record.CompareTo);

            string records = "";
            foreach (Record record in selected_records)
            {
                records += record.printRecord() + "\n";
            }
            return records;
        }
        public string printRecordType(RecordType type, Civilisation nation)
        {
            List<Record> selected_records = Records.FindAll(x => x.Nation != null && x.Type == type && x.Nation.Equals(nation));
            selected_records.Sort(Record.CompareTo);

            string records = "";
            foreach (Record record in selected_records)
            {
                records += record.printRecord() + "\n";
            }
            return records;
        }

        public string printRecordType(RecordType type, War war)
        {
            List<Record> selected_records = Records.FindAll(x => x.War != null && x.Type == type && x.War.Equals(war));
            selected_records.Sort(Record.CompareTo);

            string records = "";
            foreach (Record record in selected_records)
            {
                records += record.printRecord() + "\n";
            }
            return records;
        }

        public string printRecordType(RecordType type, Battle battle)
        {
            List<Record> selected_records = Records.FindAll(x => x.Battle != null && x.Type == type && x.Battle.Equals(battle));
            selected_records.Sort(Record.CompareTo);

            string records = "";
            foreach (Record record in selected_records)
            {
                records += record.printRecord() + "\n";
            }
            return records;
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
        public Race Race { get; set; }
        public Civilisation Nation { get; set; }
        public War War { get; set; }
        public Battle Battle { get; set; }
        public char[,] Map { get; set; }

        public int Turn { get; set; }
        public int Year { get; set; }


        public PrintFunction printFunction { get; set; }

        public string printRecord()
        {
            return printFunction(this);
        }

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
            switch (Type)
            {
                case RecordType.CreateTerrainFeature:
                    return Type.ToString() + " " + Year.ToString() + " " + Terrain.Name;
                default:
                    return "";
            }
        }
    }

    enum RecordType
    {
        CreateTerrainFeature,
        TerrainMap,
        BiomeMap,
        ClimateMap,

        RaceSettlementMap,
        NationTerritoryMap,
        GlobalTerritoryMap,

        WarReport,
        BattleReport,
    }
}
