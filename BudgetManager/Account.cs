using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BudgetManager
{
    public class Account : INotifyPropertyChanged
    {
        //Дата первого захода в приложение
        public Date RegistrationDate;
        //Конструктор, который позволяет создать только один экземпляр класса.
        private static Account _account;
        public static Account GetAccount()
        {
            return _account ?? (_account = new Account());
        }
        public static Account DeleteData()
        {
            _account = new Account();
            return _account;
        }
        private Account()
        {
            Balance = 0;
            RegistrationDate = Date.Now;
            ExpenseCategories = new ObservableCollection<Category>();
            IncomeCategories = new ObservableCollection<Category>();
            ExpensesAtDay = new SortedDictionary<Date, ObservableCollection<(Category, int)>>(new DateComparer());
            IncomesAtDay = new SortedDictionary<Date, ObservableCollection<(Category, int)>>(new DateComparer());
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
        //Методы добавления на аккаунт категорий
        public void AddExpenseCategory(string name) => ExpenseCategories.Add(new Category(name));
        public void AddIncomeCategory(string name) => IncomeCategories.Add(new Category(name));

        public void DeleteExpenseCategory(Category category) => ExpenseCategories.Remove(category);
        public void DeleteIncomeCategory(Category category) => IncomeCategories.Remove(category);
        //Словари, хранящие данные о том, сколько и в какой категории в какой день было расходов и доходов
        public SortedDictionary<Date, ObservableCollection<(Category, int)>> ExpensesAtDay { get; set; }
        public SortedDictionary<Date, ObservableCollection<(Category, int)>> IncomesAtDay { get; set; }
        //Методы добавления расходов и доходов на сегодняшнее число
        public void GetIncome((Category, int) transaction)
        {
            var date = new Date(DateTime.Now);
            if(!IncomesAtDay.ContainsKey(date))
            {
                IncomesAtDay.Add(date, new ObservableCollection<(Category, int)>());
            }
            IncomesAtDay[date].Add(transaction);
            Balance += transaction.Item2;
        }

        public void GetExpense((Category, int) transaction)
        {
            var date = new Date(DateTime.Now);
            if (!ExpensesAtDay.ContainsKey(date))
            {
                ExpensesAtDay.Add(date, new ObservableCollection<(Category, int)>());
            }

            ExpensesAtDay[date].Add(transaction);
            Balance -= transaction.Item2;
        }
        public int GetSumOfExpensesAtDate(Date date) => ExpensesAtDay[date].Sum(category => category.Item2);
        public int GetSumOfIncomesAtDate(Date date) => IncomesAtDay[date].Sum(category => category.Item2);
    }
}
