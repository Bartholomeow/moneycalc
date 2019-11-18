using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace MoneyCalc
{
    public class Account : INotifyPropertyChanged
    {
        //Конструктор, который позволяет создать только один экземпляр класса.
        private static Account _account;
        public static Account GetAccount() => _account ??= new Account();
        private Account()
        {
            ExpenseCategories = new ObservableCollection<Category>();
            IncomeCategories = new ObservableCollection<Category>();
            ExpensesForDays = new SortedDictionary<Date, Dictionary<Category, int>>(new DateComparer());
            IncomesForDays = new SortedDictionary<Date, Dictionary<Category, int>>(new DateComparer());
        }
        //Реализация интерфейса INotifyPropertyChanged, позволяющаяя привязывать поле "баланс" к элементам WPF.
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        
        private int _balance;
        public int Balance 
        {
            get => _balance;
            set { _balance = value;
                OnPropertyChanged();
            }
        }
        //Списки категорий расходов и доходов.
        public ObservableCollection<Category> ExpenseCategories { get; set; }
        public ObservableCollection<Category> IncomeCategories { get; set; }

        public void AddExpenseCategory(string name) => ExpenseCategories.Add(new Category(name));
        public void AddIncomeCategory(string name) => IncomeCategories.Add(new Category(name));

        public void DeleteExpenseCategory(Category category) => ExpenseCategories.Remove(category);
        public void DeleteIncomeCategory(Category category) => IncomeCategories.Remove(category);
        //Словари, хранящие данные о том, сколько и в какой категории в какой день было расходов и доходов
        public SortedDictionary<Date, Dictionary<Category, int>> ExpensesForDays { get; set; }
        public SortedDictionary<Date, Dictionary<Category, int>> IncomesForDays { get; set; }
        //Методы добавления расходов и доходов на сегодняшнее число
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
