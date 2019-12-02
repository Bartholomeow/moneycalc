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
                    account.Balance = double.Parse(sr.ReadLine() ?? throw new InvalidOperationException());
                    var k = int.Parse(sr.ReadLine() ?? throw new InvalidOperationException());
                    for (int i = 0; i < k; i++)
                    {
                        account.IncomeCategories.Add(new Category(sr.ReadLine()));
                    }
                    k = int.Parse(sr.ReadLine() ?? throw new InvalidOperationException());
                    for (int i = 0; i < k; i++)
                    {
                        account.ExpenseCategories.Add(new Category(sr.ReadLine()));
                    }
                    string data;
                    while ((data = sr.ReadLine()) != null)
                    {
                        account.Data.Add(new Transaction(data));
                    }
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
                sw.WriteLine(account.Balance);
                sw.WriteLine(account.IncomeCategories.Count);
                foreach (var category in account.IncomeCategories)
                {
                    sw.WriteLine(category);
                }
                sw.WriteLine(account.ExpenseCategories.Count);
                foreach (var category in account.ExpenseCategories)
                {
                    sw.WriteLine(category);
                }
                foreach (var transaction in account.Data)
                {
                    sw.WriteLine(transaction.GetFullString());
                }
            }
        }
    }
}

