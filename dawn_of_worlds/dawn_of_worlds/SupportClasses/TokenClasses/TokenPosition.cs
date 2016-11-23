using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Parser.TokenClasses
{
    public class TokenPosition
    {
        public int Column { get; set; }
        public int Index { get; set; }
        public int Line { get; set; }

        public TokenPosition(int c, int i, int l)
        {
            Column = c;
            Index = i;
            Line = l;
        }

        public override string ToString()
        {
            return "Line: " + Line.ToString() + " ; Column: " + Column.ToString() + " ; Index: " + Index.ToString();
        }
    }
}
