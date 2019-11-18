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
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace MoneyCalc
{
    /// <summary>
    /// Логика взаимодействия для TransactionWindow.xaml
    /// </summary>
    public partial class TransactionWindow : Window
    {
        private readonly int _type;
        private readonly Account _account;
        public TransactionWindow(int type)
        {
            InitializeComponent();
            _type = type;
            _account = Account.GetAccount();
            categoryListbox.ItemsSource = _type == 1 ? _account.IncomeCategories : _account.ExpenseCategories;
        }

        private void AddCategoryButton_OnClick(object sender, RoutedEventArgs e)
        {
            var addCategoryWindow = new AddCategoryWindow(_type);
            addCategoryWindow.ShowDialog();
        }

        private void Calc_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            var value = button.Content.ToString();
            if (string.IsNullOrEmpty(value))
                return;
            switch (value)
            {
                case "C":
                    transactionTextBox.Text = "0";
                    break;

                case "=":
                    try
                    {
                        transactionTextBox.Text = Calc.Calculate(transactionTextBox.Text).ToString();
                    }
                    catch
                    {
                        transactionTextBox.Text = "Error";
                    }
                    break;
                default:
                    transactionTextBox.Text = transactionTextBox.Text.TrimStart('0') + value;
                    break;
            }
        }
        private void DeleteMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var category = (Category) categoryListbox.SelectedItem;
            if (_type == 1)
            {
                _account.DeleteIncomeCategory(category);
            }
            else
            {
                _account.DeleteExpenseCategory(category);
            }
        }

        private void okButtonClick(object sender, RoutedEventArgs e)
        {
            if (transactionTextBox.Text == "")
            {
                MessageBox.Show("Введите расход / доход.");
                return;
            }
            var r = new Regex("^[0-9+*-]+$");
            if (!r.IsMatch(transactionTextBox.Text))
            {
                MessageBox.Show("Введите корректные данные в поле.");
                return;
            }
            try
            {
                transactionTextBox.Text = Calc.Calculate(transactionTextBox.Text).ToString();
            }
            catch
            {
                transactionTextBox.Text = "Error";
                MessageBox.Show("Введите корректные данные в поле.");
                return;
            }
            var cost = int.Parse(transactionTextBox.Text);
            if (categoryListbox.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите категорию.");
                return;
            }
            var category = (Category)categoryListbox.SelectedItem;
            if (_type == 1)
            {
                _account.GetIncome(new Income(category, cost));
                MessageBox.Show($"Получено: {cost} \nКатегория: {category}");
            }
            else
            {
                _account.GetExpense(new Expense(category, cost));
                MessageBox.Show($"Потрачено: {cost} \nКатегория: {category}");
            }
            transactionTextBox.Text = "0";
        }
    }
}
