using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.WorldClasses
{
    class Area
    {
        public string Name { get; set; }

        public Area North { get; set; }
        public Area East { get; set; }
        public Area South { get; set; }
        public Area West { get; set; }

        public Area()
        {
            
        }

    }
}
