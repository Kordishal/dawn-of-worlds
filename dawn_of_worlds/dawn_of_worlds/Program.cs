using dawn_of_worlds.Log;
using dawn_of_worlds.Main;
using dawn_of_worlds.Names;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds
{
    class Program
    {
        public static World World { get; set; }
        public static History WorldHistory { get; set; }
        public static ActionLog Log { get; set; }
        public static Simulation Simulation { get; set; }

        static void Main(string[] args)
        {
            Constants.Names = new NameGenerator();
            Log = new ActionLog();
            World = new World(Constants.Names.GetName("world_names"), 5, 5);
            WorldHistory = new History();

            Simulation = new Simulation();
            Simulation.Run();

            Log.Write(World);
            Console.WriteLine("END OF APPLICATION");
            Console.ReadKey();
        }
    }
}
