using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Names
{
    class NameSet
    {

        public string Name { get; set; }
        public List<string> Templates { get; set; }

        public List<string> NameListDescriptions { get; set; }
        public List<List<string>> Names { get; set; }

        public NameSet()
        {
            NameListDescriptions = new List<string>();
            Names = new List<List<string>>();
            Templates = new List<string>();
        }
    }
}
