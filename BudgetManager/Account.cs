using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BudgetManager
{
    public class Account : INotifyPropertyChanged
    {
        //Date первого захода в приложение.
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

        public double GetBalance() => (from t in Data select t.Cost * (int)t.Category.TypeOfCategory).Sum();

        //Конструктор, который позволяет создать только один экземпляр класса.
        private static Account _account;
        public static Account GetAccount()
        {
            return _account ?? (_account = new Account());
        }

        //Полное удаление данных.
        public static Account DeleteData()
        {
            _account = new Account();
            return _account;
        }

        private Account()
        {
            Balance = 0;
            RegistrationDate = Date.Now;
            Categories = new ObservableCollection<Category>();
            Data = new List<Transaction>();
        }
        //Реализация интерфейса INotifyPropertyChanged, позволяющая привязывать поле "баланс" к элементам WPF.
        public event PropertyChangedEventHandler PropertyChanged;
        //Данные о всех транзакциях в виде [Date, Тип транзакции, Category, Cost].
        public List<Transaction> Data { get; set; }

        //Получение списка транзаций за период.
        public List<string> GetTransactionsAtPeriod(Date date1, Date date2, TypeOfCategory typeOfCategory) =>
            Data
                .Where(t => t.Category.TypeOfCategory == typeOfCategory && t.Date <= date2 && t.Date >= date1)
                .GroupBy(t => t.Category).Select(t => t.Key + " " + t.Sum(x => x.Cost)).ToList();

        //Получение суммы транзакций за период.
        public double GetSumOfTransactionsAtPeriod(Date date1, Date date2, TypeOfCategory typeOfCategory) => (from t in Data where (t.Category.TypeOfCategory == typeOfCategory && t.Date <= date2 && t.Date >= date1) select t.Cost).ToList().Sum();

        //Вычисление транзакций по определенной категории за определенный период.
        public List<Transaction> GetTransactionsOfCategoryAtPeriod(List<Category> category, Date date1, Date date2) =>
            (from t in Data where (t.Date <= date2 && t.Date >= date1 && category.Contains(t.Category)) select t)
            .ToList();

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        //Списки категорий расходов и доходов.
        public ObservableCollection<Category> Categories { get; set; }

        public List<Category> GetCategories(TypeOfCategory typeOfCategory)
        {
            return (from categories in Categories where categories.TypeOfCategory == typeOfCategory select categories).ToList();
        }

        //Методы добавления и удаления категорий.
        public void AddCategory(Category category)
        {
            if (!Categories.Any(t => t.Name == category.Name && t.TypeOfCategory == category.TypeOfCategory))
                Categories.Add(category);
        }
        public void DeleteCategory(Category category)
        {
            Categories.Remove(category);
        }

        //Добавление данных о транзакции.
        public void AddTransaction(Transaction transaction)
        {
            Balance += transaction.Cost * (int)transaction.TypeOfCategory;
            Data.Add(transaction);
        }

        //Удаление данных о транзакции
        public void DeleteTransaction(Transaction transaction)
        {
            Data.Remove(transaction);
        }
    }
}
