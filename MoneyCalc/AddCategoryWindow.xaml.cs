using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MoneyCalc
{
    /// <summary>
    /// Логика взаимодействия для AddCategoryWindow.xaml
    /// </summary>
    public partial class AddCategoryWindow : Window
    {
        Account account;
        int incomeOrExpense;
        public AddCategoryWindow(Account account, int incomeOrExpense)
        {
            this.account = account;
            this.incomeOrExpense = incomeOrExpense;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (addCategoryTextbox.Text == "")
            {
                MessageBox.Show("Введите название категории.");
                return;
            }
            if(incomeOrExpense == 1)
            {
                account.IncomeCategories.Add(new Category(addCategoryTextbox.Text));
            }
            else if(incomeOrExpense == 0)
            {
                account.ExpenseCategories.Add(new Category(addCategoryTextbox.Text));
            }
            MessageBox.Show($"Добавлена категория {addCategoryTextbox.Text}");
            this.Close();
        }
    }
}
