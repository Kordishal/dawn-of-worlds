using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using System.IO;
using Newtonsoft.Json;

namespace dawn_of_worlds.TagThesaurus
{
    // TODO: Implement a renaming mechanism. Add field FormerNames, which is check for whenever the thesaurus searches for new tags.

    class Thesaurus
    {
        private string ThesaurusPath { get; set; }

        private List<string> ThesaurusLog { get; set; }

        public List<TagEntry> Tags { get; set; }

        public Thesaurus(string path)
        {
            Tags = new List<TagEntry>();
            ThesaurusLog = new List<string>();
            ThesaurusPath = path;
        }

        // TODO: dublication test, all exists test -> otherwise delete entries or create new ones.
        public void CheckForDublicates()
        {
            foreach (var entry in Tags)
            {
                foreach (var entry_2 in Tags)
                {
                    if (entry.Identifier == entry_2.Identifier)
                        continue;

                    if (entry == entry_2)
                    {
                        ThesaurusLog.Add("WARNING: Found duplicate: " + entry.Tag);
                        Tags.Remove(entry_2);
                    }
                }
            }
        }

        public void LoadTags()
        {
            var queue = new Queue<string>();
            queue.Enqueue(ThesaurusPath);

            do
            {
                var next_path = queue.Dequeue();

                foreach (string path in Directory.EnumerateDirectories(next_path))
                    queue.Enqueue(path);
                foreach (string file_path in Directory.EnumerateFiles(next_path))
                    Tags.AddRange(JsonConvert.DeserializeObject<List<TagEntry>>(new StreamReader(file_path).ReadToEnd()));
            } while (queue.Count > 0);

            var count = 0;
            foreach (var t in Tags)
            {
                t.Identifier = count;
                count += 1;
            }
               
            ThesaurusLog.Add("INFO: 'Loaded " + Tags.Count + " Tags.");
        }

        public List<TagEntry> NewTags { get; set; }

        public void CreateEntriesForNewTags()
        {

        }

        public void PrintLog()
        {
            var writer = new StreamWriter(@"C:\Users\Jonas Waeber\Documents\Projects\dawn_of_worlds\dawn_of_worlds\dawn_of_worlds\bin\Debug\Output\thesaurus.log");
            foreach (var l in ThesaurusLog)
                writer.WriteLine(l);
            writer.Close();

        }

    }
}
