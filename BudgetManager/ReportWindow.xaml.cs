using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BudgetManager
{
    public partial class ReportWindow
    {
        private readonly Account _account;
        public ReportWindow()
        {
            _account = Account.GetAccount();
            InitializeComponent();
            foreach (var category in _account.Categories)
            {
                switch (category.TypeOfCategory)
                {
                    case TypeOfCategory.Расход:
                        CategoriesListBox.Items.Add(new ListBoxItem {Content = category, Foreground = Brushes.MediumSeaGreen});
                        break;
                    case TypeOfCategory.Доход:
                        CategoriesListBox.Items.Add(new ListBoxItem { Content = category, Foreground = Brushes.Crimson });
                        break;
                }
            }

            StartPeriod.DisplayDateStart = _account.RegistrationDate;
            StartPeriod.DisplayDateEnd = Date.Now;
            EndPeriod.DisplayDateStart = _account.RegistrationDate;
            EndPeriod.DisplayDateEnd = Date.Now;
        }
        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            if (StartPeriod.SelectedDate == null) return;
            var date1 = new Date((DateTime)(StartPeriod.SelectedDate));
            if (EndPeriod.SelectedDate == null) return;
            var date2 = new Date((DateTime)(EndPeriod.SelectedDate));
            if (date1 > date2)
            {
                MessageBox.Show("Первая дата должна быть раньше.");
                return;
            }
            if (CategoriesListBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите хотя бы одну категорию.");
                return;
            }
            var category = (from object selectedItem in CategoriesListBox.SelectedItems select (Category) ((ListBoxItem) selectedItem).Content).ToList();
            TransactionsListView.ItemsSource = _account.GetTransactionsOfCategoryAtPeriod(category, date1, date2);
        }

        private void DeleteMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var transaction = (Transaction) TransactionsListView.SelectedItem;
            _account.DeleteTransaction(transaction);
            if (StartPeriod.SelectedDate != null)
            {
                var date1 = new Date((DateTime)(StartPeriod.SelectedDate));
                if (EndPeriod.SelectedDate != null)
                {
                    var date2 = new Date((DateTime)(EndPeriod.SelectedDate));
                    var category = (from object selectedItem in CategoriesListBox.SelectedItems select (Category)((ListBoxItem)selectedItem).Content).ToList();
                    TransactionsListView.ItemsSource = _account.GetTransactionsOfCategoryAtPeriod(category, date1, date2);
                }
            }

            _account.Balance = _account.GetBalance();
        }

        private void SelectAllCategoriesButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (CategoriesListBox.SelectedItems.Count != _account.Categories.Count)
                CategoriesListBox.SelectAll();
            else
                CategoriesListBox.SelectedItem = null;
        }

        private void AllTimeButton_OnClick_(object sender, RoutedEventArgs e)
        {
            StartPeriod.SelectedDate = _account.RegistrationDate;
            EndPeriod.SelectedDate = Date.Now;
        }
    }
}
