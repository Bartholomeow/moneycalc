using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetManager
{
    public class Transaction
    {
        public Transaction(Date дата, TypeOfTransaction тип, Category категория, double стоимость)
        {
            Дата = дата;
            Тип = тип;
            Категория = категория;
            Стоимость = стоимость;
        }

        public Transaction(Category категория, double стоимость)
        {
            Категория = категория;
            Стоимость = стоимость;
        }

        public Transaction(string transaction)
        {
            var dataStrings = transaction.Split(' ');
            Дата = new Date(dataStrings[0]);
            switch (dataStrings[1])
            {
                case "Доход":
                    Тип = TypeOfTransaction.Доход;
                    break;
                case "Расход":
                    Тип = TypeOfTransaction.Расход;
                    break;
            }

            Категория = new Category(dataStrings[2]);
            Стоимость = double.Parse(dataStrings[3]);
        }

        public override string ToString()
        {
            return Категория + " " + Стоимость;
        }

        public string GetFullString()
        {
            return Дата + " " + Тип.ToString("g") + " " + Категория + " " + Стоимость;
        }

        public Date Дата { get; set; }
        public TypeOfTransaction Тип { get; set; }
        public Category Категория { get; set; }
        public double Стоимость { get; set; }
    }

    public enum TypeOfTransaction
    {
        Доход = 1,
        Расход = -1
    }
}