using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using dawn_of_worlds.Creations.Organisations;

namespace dawn_of_worlds.Creations.Diplomacy
{
    class Relations
    {
        public Nation Target { get; set; }
        public RelationStatus Status { get; set; }

        public Relations(Nation target)
        {
            Target = target;
            Status = RelationStatus.Unknown;
        }
    }


    enum RelationStatus
    {
        Unknown,
        Known,
        Allied,
        AtWar,
    }
}
