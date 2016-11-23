using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Parser.TokenClasses
{
    public class Token
    {
        public TokenPosition Position { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

        public Token(string type, string value, TokenPosition position)
        {
            Position = position;
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return Type + ":" + Value;
        }
    }
}
