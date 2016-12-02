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
    class ActionLog
    {
        private StreamWriter Test { get; set; }
        private StreamWriter DeityWriter { get; set; }
        private StreamWriter AreaWriter { get; set; }
        private StreamWriter RaceWriter { get; set; }
        private StreamWriter ActionLogEntryWriter { get; set; }
        private StreamWriter WorldHistoryWriter { get; set; }
        public List<ActionLogEntry> Entries { get; set; }

        private const string OUTPUT_FOLDER = @"C:\Users\Jonas Waeber\Documents\Projects\dawn_of_worlds\dawn_of_worlds\dawn_of_worlds\Log\Output\";
        private const string ACTION_LOG = @"action.log";
        

        public ActionLog()
        {
            Entries = new List<ActionLogEntry>();
            ActionLogEntryWriter = new StreamWriter(OUTPUT_FOLDER + ACTION_LOG);
        }

        public void WriteTest()
        {
            Test = new StreamWriter(OUTPUT_FOLDER + @"\Tests\names.txt");

            for (int i = 0; i < 100; i++)
            {
                Test.Write(Constants.Names.GetName("area") + " ");
                if (i % 10 == 0)
                    Test.Write(Test.NewLine);
            }
            Test.Close();
        }

        public void Write()
        {
            foreach (ActionLogEntry entry in Entries)
            {
                ActionLogEntryWriter.Write(entry.ToString());
            }
            ActionLogEntryWriter.Close();

            int counter = 0;
            foreach (Area area in Program.World.AreaGrid)
            {
                AreaWriter = new StreamWriter(OUTPUT_FOLDER + @"Areas\area_" + counter.ToString() + ".log");
                AreaWriter.Write(area.printArea());
                AreaWriter.Close();
                counter++;
            }

            counter = 0;
            foreach (Deity deity in Program.World.Deities)
            {
                DeityWriter = new StreamWriter(OUTPUT_FOLDER + @"Deities\deity " + counter + ".log");
                DeityWriter.Write(deity.printDeity());
                DeityWriter.Close();
                counter++;
            }

            counter = 0;
            foreach (Race race in Program.World.Races)
            {
                RaceWriter = new StreamWriter(OUTPUT_FOLDER + @"Races\race " + counter + ".log");
                RaceWriter.Write(race.printRace());
                RaceWriter.Close();
                counter++;
            }

            WorldHistoryWriter = new StreamWriter(OUTPUT_FOLDER + @"world_history.log");
            WorldHistoryWriter.Write(Program.WorldHistory.printWorldHistory());
            WorldHistoryWriter.Close();
        }

    }
}
