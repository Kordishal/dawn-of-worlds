using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Main
{
    class MainLoop
    {

        public const int AREA_GRID_X = 5;
        public const int AREA_GRID_Y = 5;
        static public Random RND = new Random(100);

        public int CurrentAge { get; set; }

        public World MainWorld { get; set; }

        public List<Deity> Deities { get; set; }

        public MainLoop() { }

        public void Initialize()
        {
            MainWorld = new World("New World", 5, 5);
           
            Deities = new List<Deity>();

            for (int i = 0; i < RND.Next(5, 10); i++)
            {
                Deities.Add(new Deity());
            }
        }

        public void Run()
        {
            CurrentAge = 1;
            for (int i = 0; i < 30; i++)
            {
                foreach (Deity deity in Deities)
                {
                    deity.Turn(MainWorld, CurrentAge);
                }

                if (i == 10)
                    CurrentAge += 1;

                if (i == 20)
                    CurrentAge += 1;
            
            }
        }

    }
}
