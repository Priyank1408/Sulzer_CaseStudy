using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseStudy2
{
    public class Item
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public Item(string name, int quantity, decimal unitPrice)
        {
            Name = name;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
    }

    public class Program
    {
        public static decimal CalculateTotalPrice(List<Item> order)
        {
            if (order == null || order.Count == 0)
            {
                return 0;
            }

            decimal subtotal = 0;
            foreach (var item in order)
            {
                decimal itemTotal = item.UnitPrice * item.Quantity;
                if (item.Quantity >= 3)
                {
                    itemTotal *= 0.9m; // Apply 10% discount for quantities of 3 or more
                }
                subtotal += itemTotal;
            }

            if (subtotal > 100)
            {
                subtotal *= 0.95m; // Apply 5% discount for orders over $100
            }

            return subtotal;
        }

        public static void Main(string[] args)
        {
            // Testing will be done using Unit test cases
        }
    }
}