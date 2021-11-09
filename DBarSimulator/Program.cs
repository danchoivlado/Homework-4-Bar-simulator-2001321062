using System;
using System.Collections.Generic;
using System.Threading;

namespace DBarSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            List<Drink> drinks = new List<Drink>()
            {
                new Drink(12,"vodka",12),
                new Drink(12,"whiskey",18),
                new Drink(12,"water",3),
                new Drink(12,"redbull",6),
                new Drink(12,"beer",10)
            };

            Bar bar = new Bar(drinks);
            List<Thread> studentThreads = new List<Thread>();
            for (int i = 1; i <50; i++)
            {       
                var student = new Student(i.ToString(), bar,100,random.Next(1,80));
                var thread = new Thread(student.PaintTheTownRed);
                thread.Start();
                studentThreads.Add(thread);

                int chance = random.Next(0, 100);
                if (chance < 5)
                {
                    bar.Close();
                }
            }

            foreach (var t in studentThreads) t.Join();
            Console.WriteLine();
            Console.WriteLine("The party is over.");
            Console.ReadLine();
        }
    }
}
