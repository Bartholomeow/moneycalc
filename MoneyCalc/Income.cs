using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyCalc
{
    public class Income
    {
        public Category Category { get; set; }
        public int Cost { get; set; }
        public Income(Category category, int cost)
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
