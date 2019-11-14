using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Runtime.CompilerServices;

namespace MoneyCalc
{
    public class Account : INotifyPropertyChanged
    {
        private static Account account;
        private Account() 
        {
            Balance = 0;
            ExpenseCategories = new List<Category>();
            IncomeCategories = new List<Category>();
            ExpensesForDays = new SortedDictionary <Date, Dictionary<Category, int>>(new DateComparer());
            IncomesForDays = new SortedDictionary<Date, Dictionary<Category, int>>(new DateComparer());
            ExpenseCategories.Add(new Category("Транспорт"));
            ExpenseCategories.Add(new Category("Еда"));
            ExpenseCategories.Add(new Category("Счета"));
            ExpenseCategories.Add(new Category("Подарки"));
            ExpenseCategories.Add(new Category("Развлечения"));
            ExpenseCategories.Add(new Category("Жилье"));
            IncomeCategories.Add(new Category("Зарплата"));
            IncomeCategories.Add(new Category("Сбережения"));
            IncomeCategories.Add(new Category("Депозит"));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public static Account getAccount()
        {
            if (account == null)
                account = new Account();
            return account;
        }
        private int _balance;
        public int Balance 
        {
            get { return _balance; }
            set { _balance = value;
                OnPropertyChanged("Balance");
            }
        }
        public List<Category> ExpenseCategories { get; set; }
        public List<Category> IncomeCategories { get; set; }
        public SortedDictionary<Date, Dictionary<Category, int>> ExpensesForDays { get; set; }
        public SortedDictionary<Date, Dictionary<Category, int>> IncomesForDays { get; set; }
        public void GetIncome(Income income)
        {
            var date = DateTime.Now;
            Date myDate = new Date(date.Day, date.Month, date.Year);
            if(!IncomesForDays.ContainsKey(myDate))
            {
                IncomesForDays.Add(myDate, new Dictionary<Category, int>());
            }
            if(!IncomesForDays[myDate].ContainsKey(income.Category))
            {
                IncomesForDays[myDate].Add(income.Category, 0);
            }
            IncomesForDays[myDate][income.Category] += income.Cost;
            Balance += income.Cost;
        }
        public void GetExpense(Expense expense)
        {
            var date = DateTime.Now;
            Date myDate = new Date(date.Day, date.Month, date.Year);
            if (!ExpensesForDays.ContainsKey(myDate))
            {
                ExpensesForDays.Add(myDate, new Dictionary<Category, int>());
            }
            if (!ExpensesForDays[myDate].ContainsKey(expense.Category))
            {
                ExpensesForDays[myDate].Add(expense.Category, 0);
            }
            ExpensesForDays[myDate][expense.Category] += expense.Cost;
            Balance -= expense.Cost;
        }
        //public int GetSumOfExpensesAtDate(Date date)
        //{
        //    if(!ExpensesForDays.ContainsKey(date))
        //    {
        //        ExpensesForDays.Add(date, new List<Expense>());
        //    }
        //    var expenses = ExpensesForDays[date];
        //    int sumOfExpensesAtDate = 0;
        //    foreach (var expense in expenses)
        //    {
        //        sumOfExpensesAtDate += expense.Cost;
        //    }
        //    return sumOfExpensesAtDate;
        //}
        //public int GetSumOfExpensesInCategoryAtDate(Date date, Category category)
        //{
        //    if (!ExpensesForDays.ContainsKey(date))
        //    {
        //        ExpensesForDays.Add(date, new List<Expense>());
        //    }
        //    var expenses = ExpensesForDays[date];
        //    int sumOfExpensesAtDate = 0;
        //    foreach (var expense in expenses)
        //    {
        //        sumOfExpensesAtDate += expense.Cost;
        //    }
        //    return sumOfExpensesAtDate;
        //}
        //public int GetSumOfIncomesAtDate(Date date)
        //{
        //    if (!IncomesForDays.ContainsKey(date))
        //    {
        //        IncomesForDays.Add(date, new List<Income>());
        //    }
        //    var incomes = IncomesForDays[date];
        //    int sumOfIncomesAtDate = 0;
        //    foreach (var income in incomes)
        //    {
        //        sumOfIncomesAtDate += income.Cost;
        //    }
        //    return sumOfIncomesAtDate;
        //}
    }
}
