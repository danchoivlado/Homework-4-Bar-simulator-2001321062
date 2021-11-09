using System;
using System.Collections.Generic;
using System.Threading;

namespace DBarSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Bar bar = new Bar();
            List<Thread> studentThreads = new List<Thread>();
            for (int i = 1; i < 2; i++)
            {
                var student = new Student(i.ToString(), bar);
                var thread = new Thread(student.PaintTheTownRed);
                thread.Start();
                studentThreads.Add(thread);
            }

            foreach (var t in studentThreads) t.Join();
            Console.WriteLine();
            Console.WriteLine("The party is over.");
            Console.ReadLine();
        }
    }
}
