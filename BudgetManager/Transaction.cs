using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager
{
    public class Transaction
    {
        public Transaction(Date date, int type, Category category, double cost)
        {
            Date = date;
            Type = type;
            Category = category;
            Cost = cost;
        }

        public Transaction(string transaction)
        {
            var dataStrings = transaction.Split(' ');
            Date = new Date(dataStrings[0]);
            Type = int.Parse(dataStrings[1]);
            Category = new Category(dataStrings[2]);
            Cost = double.Parse(dataStrings[3]);
        }

        public override string ToString()
        {
            return Category + " " + Cost;
        }

        public string GetFullString()
        {
            return Date + " " + Type + " " + Category + " " + Cost;
        }

        public Date Date { get; set; }
        public int Type { get; set; }
        public Category Category { get; set; }
        public double Cost { get; set; }
    }
}
