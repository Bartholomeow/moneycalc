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
                    int k;
                    account.RegistrationDate = new Date(sr.ReadLine() ?? throw new InvalidOperationException());
                    account.Balance = int.Parse(sr.ReadLine() ?? throw new InvalidOperationException());
                    var n = int.Parse(sr.ReadLine() ?? throw new InvalidOperationException());
                    for (byte i = 0; i < n; i++)
                    {
                        account.ExpenseCategories.Add(new Category(sr.ReadLine()));
                    }

                    n = int.Parse(sr.ReadLine() ?? throw new InvalidOperationException());
                    for (byte i = 0; i < n; i++)
                    {
                        account.IncomeCategories.Add(new Category(sr.ReadLine()));
                    }

                    n = int.Parse(sr.ReadLine() ?? throw new InvalidOperationException());
                    for (byte i = 0; i < n; i++)
                    {
                        var date = new Date(sr.ReadLine());
                        account.ExpensesAtDay.Add(date, new ObservableCollection<(Category, double)>());
                        k = int.Parse(sr.ReadLine() ?? throw new InvalidOperationException());
                        for (byte j = 0; j < k; j++)
                        {
                            var categoryCost = sr.ReadLine()?.Split(' ');
                            if (categoryCost == null) continue;
                            var category = new Category(categoryCost[0]);
                            var cost = int.Parse(categoryCost[1]);
                            account.ExpensesAtDay[date].Add((category, cost));
                        }
                    }

                    n = int.Parse(sr.ReadLine() ?? throw new InvalidOperationException());
                    for (byte i = 0; i < n; i++)
                    {
                        var date = new Date(sr.ReadLine());
                        account.IncomesAtDay.Add(date, new ObservableCollection<(Category, double)>());
                        k = int.Parse(sr.ReadLine() ?? throw new InvalidOperationException());
                        for (byte j = 0; j < k; j++)
                        {
                            var categoryCost = sr.ReadLine()?.Split(' ');
                            if (categoryCost == null) continue;
                            var category = new Category(categoryCost[0]);
                            var cost = int.Parse(categoryCost[1]);
                            account.IncomesAtDay[date].Add((category, cost));
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Create(path);
            }
            catch
            {
                var s = "" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
                Move(path, path.Remove(path.Length - 4, 4) + s + ".txt");
                Create(path);
            }
            return account;
        }

        //Метод сохранения данных об аккаунте в файле.
        public static void AccountWriter(string path)
        {
            var account = Account.GetAccount();
            using (var sw = new StreamWriter(path, false, Encoding.GetEncoding("windows-1251")))
            {
                sw.WriteLine(account.RegistrationDate);

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

                sw.WriteLine(account.ExpensesAtDay.Count);
                foreach (var tuple in account.ExpensesAtDay)
                {
                    sw.WriteLine(tuple.Key);
                    sw.WriteLine(tuple.Value.Count);
                    foreach (var (category, cost) in tuple.Value)
                    {
                        sw.WriteLine(category + " " + cost);
                    }
                }
                sw.WriteLine(account.IncomesAtDay.Count);
                foreach (var tuple in account.IncomesAtDay)
                {
                    sw.WriteLine(tuple.Key);
                    sw.WriteLine(tuple.Value.Count);
                    foreach (var (category, cost) in tuple.Value)
                    {
                        sw.WriteLine(category + " " + cost);
                    }
                }
            }
        }
    }
}

