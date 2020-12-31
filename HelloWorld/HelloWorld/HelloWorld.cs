using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thisisCSharp
{
    class HelloWorld
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("사용법 : HelloWorld.exe <이름>");
                return;
            }
            Console.WriteLine("Hello, {0}!", args[0]);
        }
    }
}