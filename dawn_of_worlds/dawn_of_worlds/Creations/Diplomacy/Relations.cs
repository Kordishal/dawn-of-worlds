using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using dawn_of_worlds.Creations.Civilisations;

namespace dawn_of_worlds.Creations.Diplomacy
{
    [Serializable]
    class Relations
    {
        public Civilisation Target { get; set; }
        public RelationStatus Status { get; set; }

        public Relations(Civilisation target)
        {
            Target = target;
            Status = RelationStatus.Unknown;

        }

        public override string ToString()
        {
            return Target.Name + ": " + Status.ToString();
        }
    }


    enum RelationStatus
    {
        None,
        Self,
        Unknown,
        Known,
        Allied,
        AtWar,
    }
}
