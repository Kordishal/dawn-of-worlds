using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dawn_of_worlds.Parser.TokenClasses
{
    public class TokenDefinition
    {
        public bool IsIgnored { get; set; }
        public Regex Regex { get; set; }
        public string Type { get; set; }

        public TokenDefinition(Regex regex, string type, bool isignored = false)
        {
            Regex = regex;
            Type = type;
            IsIgnored = isignored;
        }
    }
}
