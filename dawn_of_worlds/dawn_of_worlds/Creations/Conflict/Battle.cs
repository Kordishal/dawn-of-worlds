using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Organisations;

namespace dawn_of_worlds.Creations.Conflict
{
    class Battle : Creation
    {

        public Army AttackerArmy { get; set; }
        public Army DefendingArmy { get; set; }


        public void Fight()
        {
            int attacker_strenght = Main.Constants.Random.Next(2, 13) + AttackerArmy.ArmyStrenghtBonus;
            int defender_strenght = Main.Constants.Random.Next(2, 13) + AttackerArmy.ArmyStrenghtBonus;

            // defender wins
            if (defender_strenght >= attacker_strenght)
            {
                AttackerArmy.isScattered = true;
            }
            // attacker wins
            else
            {
                DefendingArmy.isScattered = true;
            }
        }



        public Battle(string name, Deity creator, Army attacker, Army defender) : base(name, creator)
        {
            AttackerArmy = attacker;
            DefendingArmy = defender;
        }
    }
}
