
using System;
using System.Diagnostics;

namespace TestLibrary
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            TestKiwi.Test();
            TestKiwiTs.Test();
            //TestKiwiTs.TestSimpleConstraint();
            TestCassowaryNET.Test();
            
            Console.ReadKey();
        }
    }
}

