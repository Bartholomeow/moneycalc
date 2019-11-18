using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyCalc
{
    public class Expense 
    {
        public Category Category { get; set; }
        public int Cost { get; set; }
        public Expense(Category category, int cost)
        {
            Category = category;
            Cost = cost;
        }
        public override string ToString()
        {
            return Category + " " + Cost;
        }
    }
}
