using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace BudgetManager
{
    public partial class MainWindow
    {
        private Account _account;
        public MainWindow()
        {
            _account = Serializer.AccountReader("config.txt");
            InitializeComponent();
            DataConfiguration();
        }

        private void Transaction_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var transactionWindow = button.Name == "IncomeButton" ? new TransactionWindow(1) : new TransactionWindow(0);
            transactionWindow.ShowDialog();
            SumOfIncomesTextBlock.Text = _account.GetSumOfIncomesAtDate(Date.Now).ToString();
            SumOfExpensesTextBlock.Text = _account.GetSumOfExpensesAtDate(Date.Now).ToString();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Serializer.AccountWriter("config.txt");
        }

        private void DatePicker_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DatePicker.SelectedDate == null) return;
            var date = new Date((DateTime)DatePicker.SelectedDate);
            if (date < _account.RegistrationDate)
            {
                DatePicker.SelectedDate = _account.RegistrationDate;
                MessageBox.Show("Данные об указанном периоде отсутствуют.");
                return;
            }
            if (date > Date.Now)
            {
                DatePicker.SelectedDate = Date.Now;
                MessageBox.Show("Данные об указанном периоде отсутствуют.");
                return;
            }
            TransactionConfiguration(date);
        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            if (DatePicker.SelectedDate == Date.Now)
            {
                MessageBox.Show("Данные об указанном периоде отсутствуют.");
                return;
            }
            if (DatePicker.SelectedDate != null)
                DatePicker.SelectedDate = ((DateTime) DatePicker.SelectedDate).AddDays(1);
        }

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            if (DatePicker.SelectedDate == _account.RegistrationDate)
            {
                MessageBox.Show("Данные об указанном периоде отсутствуют.");
                return;
            }
            if (DatePicker.SelectedDate != null)
                DatePicker.SelectedDate = ((DateTime) DatePicker.SelectedDate).AddDays(-1);
        }

        private void SynchMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var synchronizationWindow = new SynchronizationWindow();
            synchronizationWindow.ShowDialog();
            _account = Account.GetAccount();
            DataConfiguration();
            TransactionConfiguration(Date.Now);
        }

        private void DeleteMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var messageBoxResult = MessageBox.Show("Вы уверены, что хотите очистить данные?", "Подтверждение", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
                _account = Account.DeleteData();
            DataConfiguration();
            TransactionConfiguration(Date.Now);
        }

        public void DataConfiguration()
        {
            DataContext = _account;
            DatePicker.SelectedDate = Date.Now;
            DatePicker.DisplayDateEnd = Date.Now;
            DatePicker.DisplayDateStart = _account.RegistrationDate;
        }

        public void TransactionConfiguration(Date date)
        {
            if (!_account.IncomesAtDay.ContainsKey(date))
            {
                _account.IncomesAtDay.Add(date, new ObservableCollection<(Category, int)>());
            }
            if (!_account.ExpensesAtDay.ContainsKey(date))
            {
                _account.ExpensesAtDay.Add(date, new ObservableCollection<(Category, int)>());
            }
            IncomesListBox.ItemsSource = _account.IncomesAtDay[date];
            ExpensesListBox.ItemsSource = _account.ExpensesAtDay[date];
            SumOfIncomesTextBlock.Text = _account.GetSumOfIncomesAtDate(date).ToString();
            SumOfExpensesTextBlock.Text = _account.GetSumOfExpensesAtDate(date).ToString();
        }
    }
}

