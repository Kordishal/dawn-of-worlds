using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.Main
{
    class TimeLine
    {
        public Age CurrentAge { get; set; }

        public int Turn { get; set; }
        public int[] NewAge { get; set; }

        public int CurrentTime{ get; set; }
        public int PreviousTime { get; set; }

        public void Advance()
        {
            Turn += 1;

            if (Turn == NewAge[1])
                CurrentAge = Age.Races;
            if (Turn == NewAge[2])
                CurrentAge = Age.Relations;

            PreviousTime = CurrentTime;
            switch (CurrentAge)
            {
                case Age.Creation:
                    CurrentTime += 500;
                    break;
                case Age.Races:
                    CurrentTime += 100;
                    break;
                case Age.Relations:
                    CurrentTime += 10;
                    break;

            }
        }

        public int Difference { get { return CurrentTime - PreviousTime; } }
        public int Shuffle { get { return Constants.Random.Next(PreviousTime, CurrentTime); } }

        public TimeLine()
        {
            CurrentAge = Age.Creation;
            CurrentTime = 0;
            PreviousTime = 0;
            NewAge = new int[3] { 0, 20, 40 };
            Turn = 0;
        }
    }

    enum Age
    {
        Creation,
        Races,
        Relations,
    }
}
