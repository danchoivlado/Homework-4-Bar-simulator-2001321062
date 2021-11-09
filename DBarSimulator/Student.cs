using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DBarSimulator
{
    class Student
    {
        enum NightlifeActivities { Walk, VisitBar, GoHome };
        enum BarActivities { Drink, Dance, Leave };

        Random random = new Random();

        public string Name { get; set; }
        public Bar Bar { get; set; }
        public double Budget { get; set; }
        List<string> menu;
        public int Age { get; set; }

        private NightlifeActivities GetRandomNightlifeActivity()
        {
            int n = random.Next(10);
            if (n < 3) return NightlifeActivities.Walk;
            if (n < 8) return NightlifeActivities.VisitBar;
            return NightlifeActivities.GoHome;
        }

        private BarActivities GetRandomBarActivity()
        {
            int n = random.Next(10);
            if (n < 4) return BarActivities.Dance;
            if (n < 9) return BarActivities.Drink;
            return BarActivities.Leave;
        }

        private void WalkOut()
        {
            Console.WriteLine($"{Name} is walking in the streets.");
            Thread.Sleep(100);
        }

        private void VisitBar()
        {
            Console.WriteLine($"{Name} is getting in the line to enter the bar.");
            try
            {
                Bar.Enter(this);      
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            Console.WriteLine($"{Name} entered the bar!");
            bool staysAtBar = true;
            while (staysAtBar)
            {
                var nextActivity = GetRandomBarActivity();
                switch (nextActivity)
                {
                    case BarActivities.Dance:
                        Console.WriteLine($"{Name} is dancing.");
                        Thread.Sleep(100);
                        break;
                    case BarActivities.Drink:
                        this.Drink();
                        Thread.Sleep(100);
                        break;
                    case BarActivities.Leave:
                        Console.WriteLine($"{Name} is leaving the bar.");
                        Bar.Leave(this);
                        staysAtBar = false;
                        break;
                    default: throw new NotImplementedException();
                }
            }
        }
        

        public void Drink()
        {
            this.menu = this.menu == null ? this.getMenu() : this.menu;
            string selectedDrinkName = this.menu[random.Next(this.menu.Count)];
            Bar.DrinkFromBar(selectedDrinkName, this.Budget, random.Next(1,5),this);
        }

        public List<string> getMenu()
        {
            return this.Bar.drinks.Select(x => x.Name).ToList();
        }

        public void PaintTheTownRed()
        {
            WalkOut();
            bool staysOut = true;
            while (staysOut)
            {
                var nextActivity = GetRandomNightlifeActivity();
                switch (nextActivity)
                {
                    case NightlifeActivities.Walk:
                        WalkOut();
                        break;
                    case NightlifeActivities.VisitBar:
                        VisitBar();
                        staysOut = false;
                        break;
                    case NightlifeActivities.GoHome:
                        staysOut = false;
                        break;
                    default: throw new NotImplementedException();
                }
            }
            Console.WriteLine($"{Name} is going back home.");
        }

        public Student(string name, Bar bar, double budget, int age)
        {
            Name = name;
            Bar = bar;
            this.Budget = budget;
            this.Age = age;
        }
    }
}
