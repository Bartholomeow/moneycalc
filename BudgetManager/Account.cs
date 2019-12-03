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
        //Дата первого захода в приложение.
        public Date RegistrationDate;
        //Баланс.
        private double _balance;
        public double Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                OnPropertyChanged();
            }
        }
        //Конструктор, который позволяет создать только один экземпляр класса.
        private static Account _account;
        public static Account GetAccount()
        {
            return _account ?? (_account = new Account());
        }
        //Удаление данных.
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
            Data = new List<Transaction>();
        }
        //Реализация интерфейса INotifyPropertyChanged, позволяющаяя привязывать поле "баланс" к элементам WPF.
        public event PropertyChangedEventHandler PropertyChanged;
        //Данные о всех транзакциях в виде [Дата, Тип(доход=1/расход=-1), Категория, Стоимость].
        public List<Transaction> Data { get; set; }
        //Получение списка транзаций за период.
        public List<Transaction> GetExpensesAtPeriod(Date date1, Date date2) =>
            (from category in ExpenseCategories
                let cost =
                    (from t in Data
                        where (t.Тип == TypeOfTransaction.Расход && t.Дата <= date2 && t.Дата >= date1 && Equals(t.Категория, category))
                        select t.Стоимость).ToList().Sum()
                where cost != 0
                select new Transaction(category, cost)).ToList();
        public List<Transaction> GetIncomesAtPeriod(Date date1, Date date2) =>
            (from category in IncomeCategories
                let cost =
                    (from t in Data
                        where (t.Тип == TypeOfTransaction.Доход && t.Дата <= date2 && t.Дата >= date1 && Equals(t.Категория, category))
                        select t.Стоимость).ToList().Sum()
                where cost != 0
                select new Transaction(category, cost)).ToList();

        //Получение суммы транзакций за период.
        public double GetSumOfExpensesAtPeriod(Date date1, Date date2) => (from t in Data where (t.Тип == TypeOfTransaction.Расход && t.Дата <= date2 && t.Дата >= date1) select t.Стоимость).ToList().Sum();
        public double GetSumOfIncomesAtPeriod(Date date1, Date date2) => (from t in Data where (t.Тип == TypeOfTransaction.Доход && t.Дата <= date2 && t.Дата >= date1) select t.Стоимость).ToList().Sum();

        //Получение транзакций категории за период.
        public List<Transaction> GetTransactionsOfCategoryAtPeriod(List<Category> category, Date date1, Date date2) =>
            (from t in Data where (t.Дата <= date2 && t.Дата >= date1 && category.Contains(t.Категория)) select t)
            .ToList();

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        //Списки категорий расходов и доходов.
        public ObservableCollection<Category> ExpenseCategories { get; set; }
        public ObservableCollection<Category> IncomeCategories { get; set; }
        //Методы добавления и удаления категорий.
        public void AddExpenseCategory(string name) => ExpenseCategories.Add(new Category(name));
        public void AddIncomeCategory(string name) => IncomeCategories.Add(new Category(name));
        public void DeleteExpenseCategory(Category category) => ExpenseCategories.Remove(category);
        public void DeleteIncomeCategory(Category category) => IncomeCategories.Remove(category);
        //Добавление данных о транзакции.
        public void GetTransaction(Transaction transaction)
        {
            Data.Add(transaction);
            Balance += transaction.Стоимость * (int)transaction.Тип;
        }
    }
}
