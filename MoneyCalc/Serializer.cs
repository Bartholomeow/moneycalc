using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MoneyCalc
{
    public static class Serializer
    {
        //Метод взятия данных об аккаунте из файла.
        public static Account AccountReader(string path)
        {
            var account = Account.GetAccount();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            try
            {
                using var sr = new StreamReader(path, Encoding.GetEncoding("windows-1251"));
                int k;
                account.Balance = int.Parse(sr.ReadLine() ?? throw new InvalidOperationException());
                var n = int.Parse(sr.ReadLine() ?? throw new InvalidOperationException());
                for (int i = 0; i < n; i++)
                {
                    account.ExpenseCategories.Add(new Category(sr.ReadLine()));
                }
                n = int.Parse(sr.ReadLine() ?? throw new InvalidOperationException());
                for (int i = 0; i < n; i++)
                {
                    account.IncomeCategories.Add(new Category(sr.ReadLine()));
                }
                n = int.Parse(sr.ReadLine() ?? throw new InvalidOperationException());
                for (int i = 0; i < n; i++)
                {
                    var date = new Date(sr.ReadLine());
                    account.ExpensesForDays.Add(date, new Dictionary<Category, int>());
                    k = int.Parse(sr.ReadLine() ?? throw new InvalidOperationException());
                    for (int j = 0; j < k; j++)
                    {
                        string[] categoryCost = sr.ReadLine()?.Split(' ');
                        if (categoryCost == null) continue;
                        var category = new Category(categoryCost[0]);
                        var cost = int.Parse(categoryCost[1]);
                        account.ExpensesForDays[date].Add(category, cost);
                    }
                }
                n = int.Parse(sr.ReadLine() ?? throw new InvalidOperationException());
                for (int i = 0; i < n; i++)
                {
                    var date = new Date(sr.ReadLine());
                    account.IncomesForDays.Add(date, new Dictionary<Category, int>());
                    k = int.Parse(sr.ReadLine() ?? throw new InvalidOperationException());
                    for (int j = 0; j < k; j++)
                    {
                        string[] categoryCost = sr.ReadLine()?.Split(' ');
                        if (categoryCost == null) continue;
                        var category = new Category(categoryCost[0]);
                        var cost = int.Parse(categoryCost[1]);
                        account.IncomesForDays[date].Add(category, cost);
                    }
                }
            }
            catch(FileNotFoundException)
            {
                File.Create(path);
            }
            catch
            {
                var s = "" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
                File.Move(path, path.Remove(path.Length - 4, 4) + s + ".txt");
                File.Create(path);
            }
            return account;
        }
        //Метод сохранения данных об аккаунте в файле.
        public static void AccountWriter(string path, Account account)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using var sw = new StreamWriter(path, false,  Encoding.GetEncoding("windows-1251"));
            sw.WriteLine(account.Balance);
            sw.WriteLine(account.ExpenseCategories.Count);
            foreach (var category in account.ExpenseCategories)
            {
                sw.WriteLine(category);
            }
            sw.WriteLine(account.IncomeCategories.Count);
            foreach (var category in account.IncomeCategories)
            {
                sw.WriteLine(category);
            }
            sw.WriteLine(account.ExpensesForDays.Count);
            foreach (var (date, categoriesCost) in account.ExpensesForDays)
            {
                sw.WriteLine(date);
                sw.WriteLine(categoriesCost.Count);
                foreach (var (category, cost) in categoriesCost)
                {
                    sw.WriteLine(category + " " + cost);
                }
            }
            sw.WriteLine(account.IncomesForDays.Count);
            foreach (var (date, categoriesCost) in account.IncomesForDays)
            {
                sw.WriteLine(date);
                sw.WriteLine(categoriesCost.Count);
                foreach (var (category, cost) in categoriesCost)
                {
                    sw.WriteLine(category + " " + cost);
                }
            }
        }
    }
}
