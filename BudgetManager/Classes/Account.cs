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
            Categories = new ObservableCollection<Category>()
            {
                new Category("Еда", TypeOfCategory.Расход), new Category("Жилье", TypeOfCategory.Расход), new Category("Здоровье", TypeOfCategory.Расход),
                new Category("Кафе", TypeOfCategory.Расход), new Category("Транспорт", TypeOfCategory.Расход), new Category("Машина", TypeOfCategory.Расход),
                new Category("Одежда", TypeOfCategory.Расход), new Category("Развлечения", TypeOfCategory.Расход),new Category("Связь", TypeOfCategory.Расход),
                new Category("Такси", TypeOfCategory.Расход),new Category("Счета", TypeOfCategory.Расход),new Category("Спорт", TypeOfCategory.Расход),
                new Category("Зарплата", TypeOfCategory.Доход), new Category("Сбережения", TypeOfCategory.Доход), new Category("Подарок", TypeOfCategory.Доход),
                new Category("Депозиты", TypeOfCategory.Доход)
            };
            Data = new List<Transaction>();
        }
        //Реализация интерфейса INotifyPropertyChanged, позволяющая привязывать поле "баланс" к элементам WPF.
        public event PropertyChangedEventHandler PropertyChanged;
        //Данные о всех транзакциях в виде [Date, Тип транзакции, Category, Cost].
        public List<Transaction> Data { get; set; }
        //Получение списка транзаций за период.
        public List<string> GetTransactionsAtPeriod(Date date1, Date date2, TypeOfCategory typeOfCategory) =>
            (from category in Categories
                let cost =
                 (from t in Data
                  where (t.Category.TypeOfCategory == typeOfCategory && t.Date <= date2 && t.Date >= date1 && Equals(t.Category, category))
                  select t.Cost).ToList().Sum()
             where cost != 0
             orderby cost descending
             select category + " " + cost).ToList();
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
            Data.Add(transaction);
            Balance += transaction.Cost * (int)transaction.TypeOfCategory;
        }

        public void DeleteTransaction(Transaction transaction)
        {
            Data.Remove(transaction);
        }
    }
}
