using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.TagThesaurus
{
    [Serializable]
    class TagEntry
    {
        public int Identifier { get; set; }

        public string Tag { get; set; }

        public List<string> SimilarTags { get; set; }

        public List<string> OpposedTags { get; set; }

        public List<string> OppositeTags { get; set; }

        public TagEntry()
        {
            SimilarTags = new List<string>();
            OpposedTags = new List<string>();
            OppositeTags = new List<string>();
        }
    }
}
