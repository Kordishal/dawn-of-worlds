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

        public Name(string singular, string plural, string adjective)
        {
            Singular = singular;
            Plural = plural;
            Adjective = adjective;
        }

        public override string ToString()
        {
            return Singular;
        }
    }
}
