using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace dawn_of_worlds.Names
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
            if (NameSets.Exists(x => x.Tag == name_set_tag))
                return NameSets.Find(x => x.Tag == name_set_tag).GetNameWithTemplate(template);
            else
                throw new Exception("No NameSet with tag '" + name_set_tag + "'");
        }

        public string GetName(string name_set_tag)
        {
            if (NameSets.Exists(x => x.Tag == name_set_tag))
            {
                var temp = NameSets.Find(x => x.Tag == name_set_tag);
                if (temp.Active)
                    return temp.GetName();
                else
                    throw new Exception("The name set " + name_set_tag + " is not active.");
            }             
            else
                throw new Exception("No NameSet with tag '" + name_set_tag + "'");
        }

        public string GetName()
        {
            NameSet temp;
            do
            {
                temp = NameSets[Random.Next(0, NameSets.Count)];
            } while (!temp.Active);
            
            return temp.GetName();
        }
    }

    [Serializable]
    class NameSet
    {
        public string Name { get; set; }
        public string Tag { get; set; }

        public bool Active { get; set; }

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
            // TODO: Use markov chain to generate a new name if markov properties are not null.
            // TODO: Only use name lists randomized if they are full names. Add a property to name lists.
            if (Names == null || Names.Count == 0)
                throw new Exception("No names defined in NameList '" + Name + "'!");
            else
                return Names[NameGenerator.Random.Next(0, Names.Count)];
        }
    }

    class MarkovChain
    {
        public int Order { get; set; }
        public int[] Length { get; set; }
        public List<string> Alphabet { get; set; }
        public List<string> InitialLetters { get; set; }
        public Dictionary<string, List<string>> Observations { get; set; }

        private void CreateDictionary()
        {
            List<char> values = new List<char>();
            string key = "";
            string value = "";

            foreach (string word in Alphabet)
            {
                if (Order > word.Length)
                    continue;

                for (int i = 0; i < word.Length; i++)
                {
                    if (i == 0)
                        InitialLetters.Add(word.Substring(0, Order));

                    values.Clear();
                    if (Order + i <= word.Length)
                        key = word.Substring(i, Order);
                    else
                        continue;

                    if (i + Order < word.Length)
                        value = word[i + Order].ToString();
                    else
                        value = "";
                    List<string> temp;
                    if (!Observations.TryGetValue(key, out temp))
                    {
                        temp = new List<string>();
                        Observations.Add(key, temp);
                    }
                    temp.Add(value);
                }
            }
        }

        public string GenerateWord()
        {
            List<string> values;
            bool no_end = true;
            string last_key = InitialLetters[Constants.Random.Next(InitialLetters.Count)].ToString();
            StringBuilder builder = new StringBuilder();
            builder.Append(last_key);
            while (no_end)

            {
                if (Observations.TryGetValue(last_key, out values))
                {
                    string value = "";
                    // Name cannot be shorter than Length[0].
                    value = values[Constants.Random.Next(values.Count)];
                    while (builder.Length < Length[0] && value == "")
                        value = values[Constants.Random.Next(values.Count)];

                    // When empty value end the name.
                    if (value == "")
                        no_end = false;
                    else
                    {
                        last_key = last_key.Substring(1) + value;
                        builder.Append(value);
                    }
                }

                // Name may not be longer than Length[1].
                if (builder.Length > Length[1])
                    no_end = false;
            }

            return builder.ToString();
        }

        public List<string> GenerateWords(int count)
        {
            List<string> words = new List<string>();

            for (int i = 0; i < count; i++)
            {
                words.Add(GenerateWord());
            }

            return words;
        }


        public MarkovChain(int order, int min_length, int max_length, List<string> alphabet)
        {
            Order = order;
            Length = new int[2] { min_length, max_length };
            Alphabet = alphabet;
            InitialLetters = new List<string>();
            Observations = new Dictionary<string, List<string>>();
            CreateDictionary();
        }
    }

    [Serializable]
    class MarkovProperties
    {
        public int Order { get; set; }
        public int[] Length { get; set; }
    }
}

