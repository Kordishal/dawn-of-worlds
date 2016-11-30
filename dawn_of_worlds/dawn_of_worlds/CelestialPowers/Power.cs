using dawn_of_worlds.Actors;
using dawn_of_worlds.Main;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.CelestialPowers
{
    abstract class Power
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }


        virtual public bool isObsolete
        {
            get
            {
                return false;
            }
        }

        virtual public int Cost()
        {
            return 10;
        }

        virtual public int Weight(Deity creator)
        {
            return 0;
        }

        virtual public bool Precondition(Deity creator)
        {
            return true;
        }

        abstract public void Effect(Deity creator);

    }
}
