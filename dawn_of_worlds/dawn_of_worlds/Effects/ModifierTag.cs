using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Effects
{
    // TODO: define modifiers in json files.
    enum ModifierTag
    {
        // Leave this here as this is what variable are initialized with when not defined.
        Default,

        DomainsBegin,

        // General
        Creation,
        Nature,
        Exploration,
        Architecture,
        Community,
        Solitary,
        Metallurgy,
        Health,

        // Elements
        Earth,
        Water,
        Fire,
        Air,

        Heat,
        Cold,

        // Relations
        Conquest,
        War,
        Battle,
        Peace,

        // Catastrophes
        Drought,
        Pestilence,
        Magic,
        AntiMagic,

        DomainsEnd,
        // END DOMAINS

        // PROVINCIAL MODIFIERS
        //
        BeginProvince,
        Permafrost,
        ThePlague,

        EndProvince,
    }
}
