using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using System.Text;

namespace dawn_of_worlds.Names.MarkovGenerator
{
    class MarkovChain
    {
        public int Order { get; set; }
        public int[] Length { get; set; }
        public List<string> Alphabet { get; set; }
        public List<string> InitialLetters { get; set; }
        public Dictionary<string, List<string>> Observations { get; set; }

        private void createDictionary()
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

        public string generateWord()
        {
            bool no_end = true;
            List<string> values;
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

        public List<string> generateWords(int count)
        {
            List<string> words = new List<string>();

            for (int i = 0; i < count; i++)
            {
                words.Add(generateWord());
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
            createDictionary();
        }
    }
}
