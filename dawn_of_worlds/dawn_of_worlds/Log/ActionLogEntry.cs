using dawn_of_worlds.Actors;
using dawn_of_worlds.CelestialPowers;
using dawn_of_worlds.Creations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Log
{
    class ActionLogEntry
    {
        public int Turn { get; set; }

        public Deity Deity { get; set; }

        public Power UsedPower { get; set; }

        public Creation CreatedObject { get; set; }

        public ActionLogEntry(int turn, Deity deity, Power power, Creation creation)
        {
            Turn = turn;
            Deity = deity;
            UsedPower = power;
            CreatedObject = creation;
        }

        public override string ToString()
        {
            string result = "";
            result += "######################### TURN " + Turn.ToString() + "##########################\n";
            result += "Deity: " + Deity.Name + "\n";
            result += "Power: " + UsedPower.Name + "\n";
            result += "Creation: " + (CreatedObject != null ? CreatedObject.Name + "\n" : "\n");
            result += "################################################################################\n\n";
            return result;
        }

    }
}
