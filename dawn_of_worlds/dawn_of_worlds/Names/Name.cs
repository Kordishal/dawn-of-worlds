using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.Names
{
    [Serializable]
    class Name
    {
        public Name(string singular, string plural, string adjective)
        {
            Singular = singular;
            Plural = plural;
            Adjective = adjective;
        }

        public string Singular { get; set; }
        public string Plural { get; set; }

        public string Adjective { get; set; }

    }
}
