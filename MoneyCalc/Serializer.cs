using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MoneyCalc
{
    public static class Serializer
    {
        public static Account AccountReader(string path)
        {
            Account account = Account.getAccount();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            try
            {
                using (var sr = new StreamReader(path, Encoding.GetEncoding("windows-1251")))
                {
                    int n, k;
                    account.Balance = int.Parse(sr.ReadLine());
                    n = int.Parse(sr.ReadLine());
                    for (int i = 0; i < n; i++)
                    {
                        account.ExpenseCategories.Add(new Category(sr.ReadLine()));
                    }
                    n = int.Parse(sr.ReadLine());
                    for (int i = 0; i < n; i++)
                    {
                        account.IncomeCategories.Add(new Category(sr.ReadLine()));
                    }
                    n = int.Parse(sr.ReadLine());
                    for (int i = 0; i < n; i++)
                    {
                        Date date = new Date(sr.ReadLine());
                        account.ExpensesForDays.Add(date, new Dictionary<Category, int>());
                        k = int.Parse(sr.ReadLine());
                        for (int j = 0; j < k; j++)
                        {
                            string[] categoryCost = sr.ReadLine().Split(' ');
                            Category category = new Category(categoryCost[0]);
                            int cost = int.Parse(categoryCost[1]);
                            account.ExpensesForDays[date].Add(category, cost);
                        }
                    }
                    n = int.Parse(sr.ReadLine());
                    for (int i = 0; i < n; i++)
                    {
                        Date date = new Date(sr.ReadLine());
                        account.IncomesForDays.Add(date, new Dictionary<Category, int>());
                        k = int.Parse(sr.ReadLine());
                        for (int j = 0; j < k; j++)
                        {
                            string[] categoryCost = sr.ReadLine().Split(' ');
                            Category category = new Category(categoryCost[0]);
                            int cost = int.Parse(categoryCost[1]);
                            account.IncomesForDays[date].Add(category, cost);
                        }
                    }
                }
            }
            catch(FileNotFoundException e)
            {
                File.Create(path);
            }
            catch
            {
                string s = "" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
                File.Move(path, path.Remove(path.Length - 4, 4) + s + ".txt");
                File.Create(path);
            }
            return account;
        }
        public static void AccountWriter(string path, Account account)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (var sw = new StreamWriter(path, false,  Encoding.GetEncoding("windows-1251")))
            {
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
                foreach (KeyValuePair<Date, Dictionary<Category, int>> dictionary in account.ExpensesForDays)
                {
                    sw.WriteLine(dictionary.Key);
                    sw.WriteLine(dictionary.Value.Count);
                    foreach (KeyValuePair<Category, int> category in dictionary.Value)
                    {
                        sw.WriteLine(category.Key + " " + category.Value);
                    }
                }
                sw.WriteLine(account.IncomesForDays.Count);
                foreach (KeyValuePair<Date, Dictionary<Category, int>> dictionary in account.IncomesForDays)
                {
                    sw.WriteLine(dictionary.Key);
                    foreach (KeyValuePair<Category, int> category in dictionary.Value)
                    {
                        sw.WriteLine(category.Key + " " + category.Value);
                    }
                }
            }
        }
    }
}
