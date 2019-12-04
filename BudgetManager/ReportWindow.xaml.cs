using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BudgetManager
{
    /// <summary>
    /// Логика взаимодействия для ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
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
                    case TypeOfCategory.Доход:
                        CategoriesListBox.Items.Add(new ListBoxItem {Content = category, Foreground = Brushes.MediumSeaGreen});
                        break;
                    case TypeOfCategory.Расход:
                        CategoriesListBox.Items.Add(new ListBoxItem { Content = category, Foreground = Brushes.Crimson });
                        break;
                }
            }

            StartPeriod.DisplayDateStart = _account.RegistrationDate;
            StartPeriod.DisplayDateEnd = Date.Now;
            EndPeriod.DisplayDateStart = _account.RegistrationDate;
            EndPeriod.DisplayDateEnd = Date.Now;
        }

        private void DateCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            StartPeriod.SelectedDate = _account.RegistrationDate;
            EndPeriod.SelectedDate = Date.Now;
            StartPeriod.IsEnabled = false;
            EndPeriod.IsEnabled = false;
        }

        private void DateCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            StartPeriod.IsEnabled = true;
            EndPeriod.IsEnabled = true;
        }

        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            var date1 = new Date((DateTime)(StartPeriod.SelectedDate));
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
            var date1 = new Date((DateTime)(StartPeriod.SelectedDate));
            var date2 = new Date((DateTime)(EndPeriod.SelectedDate));
            var category = (from object selectedItem in CategoriesListBox.SelectedItems select (Category)((ListBoxItem)selectedItem).Content).ToList();
            TransactionsListView.ItemsSource = _account.GetTransactionsOfCategoryAtPeriod(category, date1, date2);
            _account.Balance = _account.GetBalance();
        }
    }
}
