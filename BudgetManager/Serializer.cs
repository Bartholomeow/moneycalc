using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using static System.IO.File;

namespace BudgetManager
{
    public static class Serializer
    {
        //Метод взятия данных об аккаунте из файла.
        public static Account AccountReader(string path)
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
            catch  { Create(path);}
            return account;
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

