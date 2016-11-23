using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using dawn_of_worlds.Names.Parser;
using System.Text.RegularExpressions;

namespace dawn_of_worlds.Names
{
    class NameGenerator
    {

        private List<NameSet> NameSets { get; set; }

        public NameGenerator()
        {
            NameSetParser parser = new NameSetParser();
            NameSets = parser.NameSets;
        }

        public string GetName(string namelist)
        {
            string name = "";
            foreach (NameSet set in NameSets)
            {
                if (set.Name == namelist)
                {
                    string template = set.Templates[Constants.RND.Next(set.Templates.Count)];
                    template = template.Substring(1, template.Length - 2);

                    while (Regex.Match(template, @"<[a-z_\\]*>").Success)
                    {
                        foreach (Match tile in Regex.Matches(template, @"<[a-z_\\]*>"))
                        {
                            template = template.Replace(tile.Value, getNameFromList(set, tile.Value.Substring(1, tile.Value.Length - 2)));
                        }
                    }

                    name = template;
                    break;
                }
            }

            return name;
        }

        private string getNameFromList(NameSet set, string namelist_name)
        {
            for (int i = 0; i < set.NameListDescriptions.Count; i++)
            {
                if (set.NameListDescriptions[i] == namelist_name)
                {
                    return set.Names[i][Constants.RND.Next(set.Names[i].Count)];
                }
            }

            return "";
        }

    }
}
