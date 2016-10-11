using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Creations
{
    abstract class Creation
    {
        public string Name { get; set; }


        public Creation(string name)
        {
            Name = name;
        }
    }
}
