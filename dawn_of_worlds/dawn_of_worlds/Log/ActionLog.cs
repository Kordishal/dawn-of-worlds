using dawn_of_worlds.Actors;
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
        private StreamWriter ActionLogEntryWriter { get; set; }
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

            foreach (Area area in current_world.AreaGrid)
            {
                AreaWriter = new StreamWriter(OUTPUT_FOLDER + @"Areas\Area " + area.Name + ".log");
                AreaWriter.Write(area.printArea());
                AreaWriter.Close();
            }

            foreach (Deity deity in current_world.Deities)
            {
                DeityWriter = new StreamWriter(OUTPUT_FOLDER + @"Deities\Deity " + deity.Name + ".log");
                DeityWriter.Write(deity.printDeity());
                DeityWriter.Close();
            }
        }

    }
}
