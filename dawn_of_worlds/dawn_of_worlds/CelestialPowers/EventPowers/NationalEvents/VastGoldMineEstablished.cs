﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.CelestialPowers.EventPowers.NationalEvents
{
    class VastGoldMineEstablised : NationalEvent
    {
        public VastGoldMineEstablised(Nation nation) : base(nation)
        {
            Name = "National Event: Vast Gold Vein Found";
        }

        public override int Weight(World current_world, Deity creator, int current_age)
        {
            int weight = base.Weight(current_world, creator, current_age);

            if (creator.Domains.Contains(Domain.Exploration))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            if (creator.Domains.Contains(Domain.Metallurgy))
                weight += Constants.WEIGHT_STANDARD_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            _nation.Tags.Add(NationalTags.VeryRich);
            _nation.Tags.Add(NationalTags.GoldMine);

            creator.LastCreation = _nation;
        }
    }
}