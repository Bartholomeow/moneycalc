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
    /// Логика взаимодействия для Income.xaml
    /// </summary>
    public partial class IncomeWindow : Window
    {

        Account account;
        public IncomeWindow(Account account)
        {
            this.account = account;
            InitializeComponent();
            foreach (var category in account.IncomeCategories)
            {
                Button button = new Button()
                {
                    Content = category,
                    Height=50
                };
                button.Click += _button_Click;
                categoriesPanel.Children.Add(button);
            }
        }

        private void _button_Click(object sender, RoutedEventArgs e)
        {
            if (incomeTextBox.Text == "")
            {
                MessageBox.Show("Введите доход.");
                return;
            }
            Button button = (Button)sender;
            int cost = int.Parse(incomeTextBox.Text);
            Category category = (Category)button.Content;
            account.GetIncome(new Income(category, cost));
            MessageBox.Show($"Зачислено: {cost} \nКатегория: {category}");
            incomeTextBox.Text = "";
        }
    }
}
