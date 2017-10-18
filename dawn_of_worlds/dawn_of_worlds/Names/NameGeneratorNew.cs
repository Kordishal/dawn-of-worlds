using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json;

namespace dawn_of_worlds.Names
{
    class NameGeneratorNew
    {
        class NameGenerator
        {
            private string NameListDirectory { get; set; }
            private List<string> FileNames { get; set; }

            private List<NameSet> NameSets { get; set; }

            public static Random Random { get; set; }

            public NameGenerator(string base_directory_names, int seed)
            {
                Random = new Random(seed);
                NameListDirectory = base_directory_names;
                FileNames = new List<string>();
                Queue<string> directory_paths = new Queue<string>();
                directory_paths.Enqueue(NameListDirectory);
                do
                {
                    string path = directory_paths.Dequeue();

                    foreach (var directory in Directory.EnumerateDirectories(path))
                        directory_paths.Enqueue(directory);

                    FileNames.AddRange(Directory.EnumerateFiles(path));

                } while (directory_paths.Count > 0);

                NameSets = new List<NameSet>();

                foreach (string path in FileNames)
                {
                    var reader = new StreamReader(path);
                    string json = reader.ReadToEnd();
                    NameSets.Add(JsonConvert.DeserializeObject<NameSet>(json));
                }
            }

            public List<string> GetNameSetTags()
            {
                List<string> name_set_tags = new List<string>();
                foreach (var set in NameSets)
                    name_set_tags.Add(set.Tag);
                return name_set_tags;
            }

            public List<Template> GetNameSetTemplates(string tag)
            {
                return NameSets.Find(x => x.Tag == tag).Templates;
            }

            public string GetName(string name_set_tag, Template template)
            {
                return NameSets.Find(x => x.Tag == name_set_tag).GetNameWithTemplate(template);
            }

            public string GetName()
            {
                return NameSets[Random.Next(0, NameSets.Count)].GetName();
            }
        }

        [Serializable]
        class NameSet
        {
            public string Name { get; set; }
            public string Tag { get; set; }

            public List<Template> Templates { get; set; }

            public List<NameList> NameLists { get; set; }

            public string GetNameWithTemplate(Template template)
            {
                var name = template.Content;
                foreach (Match match in Regex.Matches(name, @"<[a-z_]*>"))
                    name = name.Replace(match.Value, GetNameFromList(match.Value));
                return name;
            }

            public Template GetRandomTemplate()
            {
                if (Templates.Count == 0)
                    throw new Exception("No valid template in name set " + Name + "!");

                if (Templates.Count == 1)
                    return Templates[0];

                int total_weight = 0;

                foreach (Template t in Templates)
                    total_weight += t.Weight;

                int chance = NameGenerator.Random.Next(total_weight);
                int prev_weight = 0, current_weight = 0;
                foreach (Template template in Templates)
                {
                    current_weight += template.Weight;
                    if (prev_weight <= chance && chance < current_weight)
                    {
                        return template;
                    }
                    prev_weight += template.Weight;
                }

                return Templates[0];
            }

            public string GetNameFromList(string list_tag)
            {
                var stripped_tag = list_tag.Trim(new char[] { '<', '>' });

                foreach (var name_list in NameLists)
                    if (name_list.Tag == stripped_tag)
                        return name_list.Names[NameGenerator.Random.Next(name_list.Names.Count)];

                throw new Exception("No such name list could be found: " + stripped_tag);
            }

            public string GetName()
            {
                return NameLists[NameGenerator.Random.Next(0, NameLists.Count)].GetName();
            }
        }


        [Serializable]
        class Template
        {
            public string Content { get; set; }
            public int Weight { get; set; }
        }

        [Serializable]
        class NameList
        {
            public string Name { get; set; }
            public string Tag { get; set; }

            public MarkovProperties MarkovProperties { get; set; }

            public List<string> Names { get; set; }

            public string GetName()
            {
                return Names[NameGenerator.Random.Next(0, Names.Count)];
            }
        }

        [Serializable]
        class MarkovProperties
        {
            public int Order { get; set; }
            public int[] Length { get; set; }
        }
    }

}

