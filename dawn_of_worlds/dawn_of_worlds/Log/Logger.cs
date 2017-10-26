using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace dawn_of_worlds.Log
{
    class Logger
    {
        public static int RecordCount = 0;

        public string BaseDataDirectory { get; set; }
        public string WorldDirectory { get; set; }
        public string ProvinceDirectory { get; set; }

        public Logger(string path)
        {
            BaseDataDirectory = path;
            WorldDirectory = @"worlds\";
            ProvinceDirectory = @"provinces\";
            CleanOutputDirectory();
            JsonRecords = new List<HistoryRecord>();
        }

        public List<HistoryRecord> JsonRecords { get; set; }

        public void WriteToJson(object source, int turn, string path)
        {
            var value = new HistoryRecord()
            {
                Identifier = RecordCount,
                Path = path,
                Turn = turn,
                Source = source
            };
            JsonRecords.Add(value);
        }

        public void StoreInFile()
        {
            Dictionary<string, List<HistoryRecord>> collector = new Dictionary<string, List<HistoryRecord>>();
            foreach (var record in JsonRecords)
            {
                List<HistoryRecord> local;
                if (collector.TryGetValue(record.Path, out local))
                    local.Add(record);
                else
                    collector.Add(record.Path, new List<HistoryRecord>() { record });
            }


            foreach (var key in collector.Keys)
            {
                var temp = new StreamWriter(BaseDataDirectory + key, true);
                temp.WriteLine(JsonConvert.SerializeObject(collector[key]));
                temp.Close();
            }
        }

        public void CleanOutputDirectory()
        {
            Queue<string> directories = new Queue<string>();
            directories.Enqueue(BaseDataDirectory);

            while (directories.Count > 0)
            {
                var next_directory = directories.Dequeue();
                foreach (string directory_path in Directory.EnumerateDirectories(next_directory))
                {
                    directories.Enqueue(directory_path);

                    foreach (string file_path in Directory.EnumerateFiles(directory_path))
                        File.Delete(file_path);
                }
            }
        }
    }

    [Serializable]
    public class HistoryRecord
    {
        public int Identifier { get; set; }
        public int Turn { get; set; }
        public string Path { get; set; }
        public object Source { get; set; }
    }
}
