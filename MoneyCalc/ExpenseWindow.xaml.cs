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
    /// Логика взаимодействия для ExpenseWindow.xaml
    /// </summary>
    public partial class ExpenseWindow : Window
    {
        Account account;
        public ExpenseWindow(Account account)
        {
            this.account = account;
            InitializeComponent();
            foreach (var category in account.ExpenseCategories)
            {
                Button button = new Button()
                {
                    Content = category,
                    Height = 50
                };
                button.Click += _button_Click;
                categoriesPanel.Children.Add(button);
            }
        }

        private void _button_Click(object sender, RoutedEventArgs e)
        {
            if (expenseTextBox.Text == "")
            {
                MessageBox.Show("Введите расход.");
                return;
            }
            Button button = (Button)sender;
            int cost = int.Parse(expenseTextBox.Text);
            Category category = (Category)button.Content;
            account.GetExpense(new Expense(category, cost));
            MessageBox.Show($"Потрачено: {cost} \nКатегория: {category}");
            expenseTextBox.Text = "";
        }
    }
}
