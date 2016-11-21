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
        private StreamWriter Writer { get; set; }
        public List<ActionLogEntry> Entries { get; set; }

        public ActionLog()
        {
            Entries = new List<ActionLogEntry>();
            Writer = new StreamWriter(@"C: \Users\Jonas Waeber\Documents\Projects\dawn_of_worlds\dawn_of_worlds\dawn_of_worlds\Log\Output\action.log");
        }

        public void Write()
        {
            foreach (ActionLogEntry entry in Entries)
            {
                Writer.Write(entry.ToString());
            }
        }

    }
}
