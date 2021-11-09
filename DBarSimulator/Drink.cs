using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBarSimulator
{
    public class Drink
    {
        public int Quantity { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public Drink(int quantity, string name, double price)
        {
            Quantity = quantity;
            Name = name;
            Price = price;
        }

        public void GiveDrinkToStudent(double givenMoney, int chosenQuantity)
        {
            if (givenMoney < this.Price*chosenQuantity)
            {
                throw new InvalidOperationException("Your money is not enough");
            }

            if (this.Quantity - chosenQuantity < 0)
            {
                throw new InvalidOperationException("There is not enough quantity of this product");
            }

            this.Quantity -= chosenQuantity;
        }
    }
}
