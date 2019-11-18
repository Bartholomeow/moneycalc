﻿using System;
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
    /// Логика взаимодействия для Income.xaml
    /// </summary>
    public partial class IncomeWindow : Window
    {

        Account account;
        public IncomeWindow(Account account)
        {
            this.account = account;
            InitializeComponent();
            buildCategoryButtons();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddCategoryWindow addCategoryWindow = new AddCategoryWindow(account, 1);
            addCategoryWindow.ShowDialog();
            categoriesPanel.Children.Clear();
            buildCategoryButtons();
        }
        public void buildCategoryButtons()
        {
            foreach (var category in account.IncomeCategories)
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
            
            if (incomeTextBox.Text == "")
            {
                MessageBox.Show("Введите доход.");
                return;
            }
            Regex r = new Regex("^[0-9]+$");
            if (!r.IsMatch(incomeTextBox.Text))
            {
                MessageBox.Show("Введите корректные данные в поле доход.");
                return;
            }
            Button button = (Button)sender;
            int cost = int.Parse(incomeTextBox.Text);
            Category category = (Category)button.Content;
            account.GetIncome(new Income(category, cost));
            MessageBox.Show($"Зачислено: {cost} \nКатегория: {category}");
            incomeTextBox.Text = "";
        }
        private void Calc_Click(object sender, RoutedEventArgs e)
        {
            Button btn = e.Source as Button;
            if (btn == null)
                return;
            string value = btn.Content.ToString();
            if (String.IsNullOrEmpty(value))
                return;
            switch (value)
            {
                case "C":
                    incomeTextBox.Text = "0";
                    break;

                case "=":
                    try
                    {
                        incomeTextBox.Text = Calc.Calculate(incomeTextBox.Text).ToString();
                    }
                    catch
                    {
                        incomeTextBox.Text = "Error";
                    }
                    break;

                case "Ок":

                default:
                    incomeTextBox.Text = incomeTextBox.Text.TrimStart('0') + value;
                    break;
            }

        }
    }
}
