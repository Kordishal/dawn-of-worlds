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
        public Dictionary<string, Pair<List<int>, List<string>>> Names { get; set; }

        public NameSet(string name_set)
        {
            Name = name_set;
            Names = new Dictionary<string, Pair<List<int>, List<string>>>();
        }

        public override string ToString()
        {
            return Name;
        }
    }

    class Pair<S, A>
    {
        public S First { get; set; }
        public A Second { get; set; }

        public Pair(S first, A second)
        {
            First = first;
            Second = second;
        }
    }
}
