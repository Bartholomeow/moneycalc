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
            foreach (var expenseCategory in _account.ExpenseCategories)
            {
                CategoriesListBox.Items.Add(expenseCategory);
            }
            foreach (var incomeCategory in _account.IncomeCategories)
            {
                CategoriesListBox.Items.Add(incomeCategory);
            }
            StartPeriod.DisplayDateStart = _account.RegistrationDate;
            EndPeriod.DisplayDateStart = _account.RegistrationDate;
            StartPeriod.DisplayDateEnd = Date.Now;
            EndPeriod.DisplayDateEnd = Date.Now;
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
            var category = CategoriesListBox.SelectedItems.Cast<Category>().ToList();
            DataGrid.ItemsSource = _account.GetTransactionsOfCategoryAtPeriod(category, date1, date2);
        }
    }
}
