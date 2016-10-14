using dawn_of_worlds.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Creations.Organisations
{
    class Organisation : Creation 
    {
        public OrganisationType Type { get; set; }
        public OrganisationPurpose Purpose { get; set; }

        public Organisation(string name, Deity creator, OrganisationType type, OrganisationPurpose purpose) : base(name, creator)
        {
            Type = type;
            Purpose = purpose;
        }
    }

    public enum OrganisationType
    {
        ReligiousOrder,
    }

    public enum OrganisationPurpose
    {
        WorshipCreator,
    }
}
