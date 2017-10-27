using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.CreationData
{
    [Serializable]
    class CreationAttribute
    {
        public int Identifier { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }
        
        public bool UniqueCategory { get; set; }

        public bool MandatoryCategory { get; set; }

        public List<string> Tags { get; set; }
    }
}
