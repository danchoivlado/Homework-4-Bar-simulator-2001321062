using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DBarSimulator
{
    class Bar
    {
        List<Student> students;
        Semaphore semaphore;
        public List<Drink> drinks;
        private Dictionary<Drink, int> reportBook;
        public bool isClosed { get; set; }

        public Bar(List<Drink> drinks)
        {
            this.students = new List<Student>();
            this.semaphore = new Semaphore(10, 10);
            this.drinks = drinks;
            this.isClosed = false;
            this.reportBook = new Dictionary<Drink, int>();
        }

        public void DrinkFromBar(string chosenDrinkName, double givenMoney, int quantity, Student student)
        {
            try
            {
                lock (drinks)
                {
                    Drink drink = drinks.FirstOrDefault(x => x.Name == chosenDrinkName);
                    if (drink == default(Drink))
                    {
                        Console.WriteLine("Drink with that name does not exist");
                        return;
                    }

                    drink.GiveDrinkToStudent(givenMoney, quantity);
                    student.Budget -= drink.Price * quantity;
                    this.addToReportBook(drink, quantity);
                    Console.WriteLine($"{student.Name} drinked {drink.Name} for {drink.Price*quantity}");
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void addToReportBook(Drink drink, int chosenQuantity)
        {
            if (!this.reportBook.ContainsKey(drink))
            {
                this.reportBook.Add(drink, 0);
            }

            this.reportBook[drink] += chosenQuantity;
        }

        public void Enter(Student student)
        {
            semaphore.WaitOne();
            lock (students)
            {
                if (this.isClosed)
                {
                    throw new InvalidOperationException("The bar is already closed!");
                }
                if (student.Age < 18)
                {
                    throw new InvalidOperationException($"Student {student.Name} is not old enough to ente the bar");
                }
                students.Add(student);
            }
        }

        public void Leave(Student student)
        {
            lock (students)
            {
                students.Remove(student);
            }
            semaphore.Release();
        }

        public void Close()
        {
            this.isClosed = true;
            lock (students) { 
                foreach (var student in this.students.ToArray().ToList())
                {
                    this.Leave(student);
                    Console.WriteLine($"Student {student.Name} is kicked reason:bar is closing");
                }
            }
        }

        public void Report()
        {
            foreach (var report in this.reportBook)
            {
                var drink = report.Key;
                var soldTimes = report.Value;
                Console.WriteLine($"Drink {drink.Name} was sold {soldTimes} and has quantity of {drink.Quantity}");
            }
        }
    }
}
