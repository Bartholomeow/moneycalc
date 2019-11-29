using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace BudgetManager
{
    public partial class TransactionWindow
    {
        private readonly byte _type;
        private readonly Account _account;

        public TransactionWindow(byte type)
        {
            InitializeComponent();
            _type = type;
            _account = Account.GetAccount();
            if (CategoryListbox != null)
                CategoryListbox.ItemsSource = _type == 1 ? _account.IncomeCategories : _account.ExpenseCategories;
        }

        private void AddCategoryButton_OnClick(object sender, RoutedEventArgs e)
        {
            var addCategoryWindow = new AddCategoryWindow(_type);
            addCategoryWindow.ShowDialog();
        }

        private void Calc_Click(object sender, RoutedEventArgs e)
        {
            if (TransactionTextBox.Text == "Error")
            {
                TransactionTextBox.Clear();
            }
            var button = (Button)sender;
            var value = button.Content.ToString();
            if (string.IsNullOrEmpty(value))
                return;
            switch (value)
            {
                case "=":
                    try
                    {
                        TransactionTextBox.Text = Calc.Calculate(TransactionTextBox.Text).ToString(CultureInfo.CurrentCulture);
                    }
                    catch
                    {
                        TransactionTextBox.Text = "Error";
                    }
                    break;
                default:
                    TransactionTextBox.Text = TransactionTextBox.Text.TrimStart('0') + value;
                    break;
            }
        }

        private void DeleteMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var category = (Category)CategoryListbox.SelectedItem;
            if (_type == 1)
            {
                _account.DeleteIncomeCategory(category);
            }
            else
            {
                _account.DeleteExpenseCategory(category);
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (TransactionTextBox.Text == "" || TransactionTextBox.Text == "0")
            {
                MessageBox.Show("Введите расход / доход.");
                return;
            }
            try
            {
                TransactionTextBox.Text = Calc.Calculate(TransactionTextBox.Text).ToString(CultureInfo.CurrentCulture);
            }
            catch
            {
                TransactionTextBox.Text = "Error";
                MessageBox.Show("Введите корректные данные в поле.");
                return;
            }
            var cost = double.Parse(TransactionTextBox.Text);
            if (CategoryListbox.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите категорию.");
                return;
            }
            var category = (Category)CategoryListbox.SelectedItem;
            if (_type == 1)
            {
                _account.GetIncome((category, cost));
                MessageBox.Show($"Получено: {cost} \nКатегория: {category}");
            }
            else
            {
                _account.GetExpense((category, cost));
                MessageBox.Show($"Потрачено: {cost} \nКатегория: {category}");
            }
            TransactionTextBox.Text = "0";
        }
        private void CReapitButton_Click(object sender, RoutedEventArgs e)
        {
            if (TransactionTextBox.Text != "")
            {
                TransactionTextBox.Text = TransactionTextBox.Text.Remove(TransactionTextBox.Text.Length - 1);
            }
        }

        private void TransactionTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TransactionTextBox.Text == "Error")
            {
                TransactionTextBox.Clear();
            }
        }
    }
}