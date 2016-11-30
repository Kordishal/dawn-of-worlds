using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Inhabitants;
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

        public void Write(World current_world)
        {
            foreach (ActionLogEntry entry in Entries)
            {
                ActionLogEntryWriter.Write(entry.ToString());
            }
            ActionLogEntryWriter.Close();

            int counter = 0;
            foreach (Area area in current_world.AreaGrid)
            {
                AreaWriter = new StreamWriter(OUTPUT_FOLDER + @"Areas\Area " + counter.ToString() + " " + area.Name + ".log");
                AreaWriter.Write(area.printArea());
                AreaWriter.Close();
                counter++;
            }

            counter = 0;
            foreach (Deity deity in current_world.Deities)
            {
                DeityWriter = new StreamWriter(OUTPUT_FOLDER + @"Deities\Deity " + counter + " " + deity.Name + ".log");
                DeityWriter.Write(deity.printDeity());
                DeityWriter.Close();
                counter++;
            }

            counter = 0;
            foreach (Race race in current_world.Races)
            {
                RaceWriter = new StreamWriter(OUTPUT_FOLDER + @"Races\Race " + counter + " " + race.Name + ".log");
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
