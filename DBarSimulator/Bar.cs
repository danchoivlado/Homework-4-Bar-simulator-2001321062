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

        public Bar(List<Drink> drinks)
        {
            this.students = new List<Student>();
            this.semaphore = new Semaphore(10, 10);
            this.drinks = drinks;
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
                    Console.WriteLine($"{student.Name} drinked {drink.Name} for {drink.Price*quantity}");
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Enter(Student student)
        {
            semaphore.WaitOne();
            lock (students)
            {
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
    }
}
