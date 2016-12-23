using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.Names
{
    class Name
    {
        public string Singular { get; set; }
        public string Plural { get; set; }
        public string Adjective { get; set; }

        public override string ToString()
        {
            return Singular;
        }
    }
}
