using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager
{
    public class Transaction
    {
        public Transaction(Date date, TypeOfTransaction type, Category category, double cost)
        {
            Date = date;
            Type = type;
            Category = category;
            Cost = cost;
        }

        public Transaction(Category category, double cost)
        {
            Category = category;
            Cost = cost;
        }
        public Transaction(string transaction)
        {
            var dataStrings = transaction.Split(' ');
            Date = new Date(dataStrings[0]);
            switch (dataStrings[1])
            {
                case "Доход":
                    Type = TypeOfTransaction.Доход;
                    break;
                case "Расход":
                    Type = TypeOfTransaction.Расход;
                    break;
            }
            Category = new Category(dataStrings[2]);
            Cost = double.Parse(dataStrings[3]);
        }

        public override string ToString()
        {
            return Category + " " + Cost;
        }

        public string GetFullString()
        {
            return Date + " " + Type.ToString("g") + " " + Category + " " + Cost;
        }

        public Date Date { get; set; }
        public TypeOfTransaction Type { get; set; }
        public Category Category { get; set; }
        public double Cost { get; set; }
    } 
    public enum TypeOfTransaction
    {
        Доход = 1, Расход = -1
    }
}
