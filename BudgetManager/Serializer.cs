using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using static System.IO.File;

namespace BudgetManager
{
    public static class Serializer
    {
        //Метод взятия данных об аккаунте из файла.
        public static void AccountReader(string path)
        {
            var account = Account.DeleteData();
            try
            {
                using (var sr = new StreamReader(path, Encoding.GetEncoding("windows-1251")))
                {
                    account.RegistrationDate = new Date(sr.ReadLine() ?? throw new InvalidOperationException());
                    var k = int.Parse(sr.ReadLine() ?? throw new InvalidOperationException());
                    for (int i = 0; i < k; i++)
                    {
                        var category = sr.ReadLine()?.Split(' ');
                        if (category != null) account.AddCategory(new Category(category[0], category[1]));
                    }

                    string data;
                    while ((data = sr.ReadLine()) != null)
                    {
                        account.Data.Add(new Transaction(data));
                    }

                    account.Balance = account.GetBalance();
                }
            }
            catch
            {
                var categories = new List<Category>()
                {
                    new Category("Еда", TypeOfCategory.Расход), new Category("Жилье", TypeOfCategory.Расход),
                    new Category("Здоровье", TypeOfCategory.Расход),
                    new Category("Кафе", TypeOfCategory.Расход), new Category("Транспорт", TypeOfCategory.Расход),
                    new Category("Машина", TypeOfCategory.Расход),
                    new Category("Одежда", TypeOfCategory.Расход), new Category("Развлечения", TypeOfCategory.Расход),
                    new Category("Связь", TypeOfCategory.Расход),
                    new Category("Такси", TypeOfCategory.Расход), new Category("Счета", TypeOfCategory.Расход),
                    new Category("Спорт", TypeOfCategory.Расход),
                    new Category("Зарплата", TypeOfCategory.Доход), new Category("Сбережения", TypeOfCategory.Доход),
                    new Category("Подарок", TypeOfCategory.Доход),
                    new Category("Депозиты", TypeOfCategory.Доход)
                };
                foreach (var category in categories)
                {
                    account.AddCategory(category);
                }
                Create(path);
            }
        }

        //Метод записи данных об аккаунте в файл.
        public static void AccountWriter(string path)
        {
            var account = Account.GetAccount();
            using (var sw = new StreamWriter(path, false, Encoding.GetEncoding("windows-1251")))
            {
                sw.WriteLine(account.RegistrationDate);
                sw.WriteLine(account.Categories.Count);
                foreach (var category in account.Categories)
                {
                    sw.WriteLine(category.Name + " " + category.TypeOfCategory.ToString("g"));
                }
                foreach (var transaction in account.Data)
                {
                    sw.WriteLine(transaction);
                }
            }
        }
    }
}