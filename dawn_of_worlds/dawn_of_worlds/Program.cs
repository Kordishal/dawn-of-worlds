using dawn_of_worlds.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds
{
    class Program
    {


        static void Main(string[] args)
        {
            MainLoop loop = new MainLoop();
            loop.Initialize();
            loop.Run();


            Console.WriteLine("END OF APPLICATION");
            Console.ReadKey();
        }
    }
}
