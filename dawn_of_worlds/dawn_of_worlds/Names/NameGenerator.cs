using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using dawn_of_worlds.Names.Parser;
using System.Text.RegularExpressions;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Names.MarkovGenerator;

namespace dawn_of_worlds.Names
{
    class NameGenerator
    {

        private List<NameSet> NameSets { get; set; }

        public NameGenerator()
        {
            NameSets = new List<NameSet>();
            NameSetParser parser = new NameSetParser();

            LinkedList<NameSet> name_sets = parser.NameSets;
            foreach (NameSet node in name_sets)
            {
                NameSets.Add(node);
            }
        }

        public string GetName(string namelist)
        {     
            foreach (NameSet set in NameSets)
            {
                if (set.Name == namelist)
                {
                    Pair<List<int>, List<string>> Templates;
                    if (set.Names.TryGetValue("templates", out Templates))
                    {
                        string template = Templates.Second[Constants.Random.Next(Templates.Second.Count)];

                        foreach (Match province in Regex.Matches(template, @"<[a-z_\\]*>"))
                        {
                            template = template.Replace(province.Value, getNameFromList(set, province.Value.Substring(1, province.Value.Length - 2)));
                        }

                        return template;
                    }
                    else
                        throw new Exception("Name Set has no defined templates: " + namelist);
                }
            }
            throw new Exception("Name Set does not exist: " + namelist);
        }

        private string getNameFromList(NameSet set, string namelist_name)
        {
            Pair<List<int>, List<string>> Names;
            if (set.Names.TryGetValue(namelist_name, out Names))
            {
                if (Names.First != null)
                {
                    MarkovChain markov = new MarkovChain(Names.First[0], Names.First[1], Names.First[2], Names.Second);
                    return markov.generateWord();
                }
                else
                {
                    return Names.Second[Constants.Random.Next(Names.Second.Count)];
                }
            }
            else
                throw new Exception("NameList " +  namelist_name + " does not exists in NameSet " + set.Name);
        }


        public string GetReligionName(Deity creator, Race race)
        {
            foreach (NameSet set in NameSets)
            {
                if (set.Name == "religions")
                {
                    Pair<List<int>, List<string>> Templates;
                    if (set.Names.TryGetValue("templates", out Templates))
                    {
                        string template = Templates.Second[Constants.Random.Next(Templates.Second.Count)];

                        Pair<List<int>, List<string>> Deities;
                        if (!set.Names.TryGetValue("deity_name", out Deities))
                        {
                            Deities = new Pair<List<int>, List<string>>(null, new List<string>());
                            set.Names.Add("deity_name", Deities);
                        }
                        Deities.Second.Add(creator.Name);

                        Pair<List<int>, List<string>> Domains;
                        if (!set.Names.TryGetValue("domain_names", out Domains))
                        {
                            Domains = new Pair<List<int>, List<string>>(null, new List<string>());
                            set.Names.Add("domain_names", Domains);
                        }
                        foreach (Domain domain in creator.Domains)
                            Domains.Second.Add(domain.ToString());

                        foreach (Match province in Regex.Matches(template, @"<[a-z_\\]*>"))
                        {
                            template = template.Replace(province.Value, getNameFromList(set, province.Value.Substring(1, province.Value.Length - 2)));
                        }

                        return template;
                    }
                    else
                        throw new Exception("Name Set has no defined templates: religions");
                }
            }
            throw new Exception("Name Set does not exist: religions");
        }

        public string GetForestName(Forest forest)
        {
            foreach (NameSet set in NameSets)
            {
                if (set.Name == "forests")
                {
                    Pair<List<int>, List<string>> Templates;
                    if (set.Names.TryGetValue("templates", out Templates))
                    {
                        string template = Templates.Second[Constants.Random.Next(Templates.Second.Count)];

                        Pair<List<int>, List<string>> Areas;
                        if (!set.Names.TryGetValue("area_name", out Areas))
                        {
                            Areas = new Pair<List<int>, List<string>>(null, new List<string>());
                            set.Names.Add("area_name", Areas);
                        }
                        Areas.Second.Add(forest.Province.Area.Name);

                        Pair<List<int>, List<string>> Provinces;
                        if (!set.Names.TryGetValue("province_name", out Provinces))
                        {
                            Provinces = new Pair<List<int>, List<string>>(null, new List<string>());
                            set.Names.Add("province_name", Provinces);
                        }
                        Provinces.Second.Add(forest.Province.Name);

                        foreach (Match province in Regex.Matches(template, @"<[a-z_\\]*>"))
                        {
                            template = template.Replace(province.Value, getNameFromList(set, province.Value.Substring(1, province.Value.Length - 2)));
                        }

                        return template;
                    }
                    else
                        throw new Exception("Name Set has no defined templates: forests");
                }
            }
            throw new Exception("Name Set does not exist: forests");
        }
    }
}
