using dawn_of_worlds.Actors;
using dawn_of_worlds.CelestialPowers;
using dawn_of_worlds.Creations.Diplomacy;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Log;
using dawn_of_worlds.Names;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Main
{
    class Simulation
    {
        public static TimeLine Time { get; set; }

        public Simulation()
        {
            Time = new TimeLine();
        }

        public void Run()
        {
            for (int i = 0; i < 40; i++)
            {
                Time.Advance();
                Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@  TURN " + i.ToString() + "  @@@@@@@@@@@@@@@@@@@@@@@@@@@");
                foreach (Deity deity in Program.World.Deities)
                {
                    deity.AddPowerPoints();

                    foreach (City city in deity.FoundedCities)
                    {
                        city.not_hasRaisedArmy = true;
                    }
                }


                for (int j = 0; j < 10; j++)
                {
                    foreach (Deity deity in Program.World.Deities)
                    {
                        deity.Turn();

                        Program.Log.Entries.Add(new ActionLogEntry(i, deity, deity.LastUsedPower, deity.LastCreation));
                    }
                }


                foreach (Deity deity in Program.World.Deities)
                {
                    for (int j = 0; j < deity.Powers.Count; j++)
                    {
                        if (deity.Powers[j].isObsolete)
                        {
                            deity.Powers.Remove(deity.Powers[j]);
                            j -= 1;
                        }
                    }
                }

            
            }
        }
    }
}
