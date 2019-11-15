using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
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
            buildCategoryButtons();
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddCategoryWindow addCategoryWindow = new AddCategoryWindow(account, 0);
            addCategoryWindow.ShowDialog();
            categoriesPanel.Children.Clear();
            buildCategoryButtons();
        }
        public void buildCategoryButtons()
        {
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
            Button buttonAdd = new Button()
            {
                Content = "Добавить категорию",
                Height = 50,
                Width = 150
            };
            buttonAdd.Click += ButtonAdd_Click;
            categoriesPanel.Children.Add(buttonAdd);
        }
        private void _button_Click(object sender, RoutedEventArgs e)
        {
            if (expenseTextBox.Text == "")
            {
                MessageBox.Show("Введите расход.");
                return;
            }
            Regex r = new Regex("^[0-9]+$");
            if (!r.IsMatch(expenseTextBox.Text))
            {
                MessageBox.Show("Введите корректные данные в поле расход.");
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
