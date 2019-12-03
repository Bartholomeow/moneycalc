using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace BudgetManager
{
    public partial class MainWindow
    {
        private Account _account;
        //Период за который будут выводиться данные
        private string _selectedTypeOfDate;
        private Date _startPeriod;
        private Date _endPeriod;
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Serializer.AccountWriter("config.txt");
        }
        public MainWindow()
        {
            _account = Serializer.AccountReader("config.txt");
            InitializeComponent();
            StartWindowConfiguration();
        }

        private void StartWindowConfiguration()
        {
            DataContext = _account;
            _startPeriod = Date.Now;
            _endPeriod = Date.Now;
            _selectedTypeOfDate = "День";
            PeriodComboBox.SelectedIndex = 0;
            PeriodConfiguration();
            DataConfiguration();
        }
        private void PeriodConfiguration()
        {
            PeriodTextBlock.Text = _startPeriod + " - " + _endPeriod;
        }
        private void DataConfiguration()
        {
            if (IncomesListBox == null)
            {
                return;
            }
            IncomesListBox.ItemsSource = _account.GetIncomesAtPeriod(_startPeriod, _endPeriod);
            ExpensesListBox.ItemsSource = _account.GetExpensesAtPeriod(_startPeriod, _endPeriod);
            SumOfExpensesTextBlock.Text = _account.GetSumOfExpensesAtPeriod(_startPeriod, _endPeriod).ToString();
            SumOfIncomesTextBlock.Text = _account.GetSumOfIncomesAtPeriod(_startPeriod, _endPeriod).ToString();
        }
        private void Transaction_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var transactionWindow = button.Name == "IncomeButton" ? new TransactionWindow(TypeOfTransaction.Доход) : new TransactionWindow(TypeOfTransaction.Расход);
            transactionWindow.ShowDialog();
            DataConfiguration();
        }
        private void ChangeDateButtonClick(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            var coefficient = 1;
            var date = Date.Now;
            if (button.Name == "LeftDateButton")
            {
                coefficient = -1;
                date = _account.RegistrationDate;
            }

            if (_startPeriod <= date && _endPeriod >= date)
            {
                MessageBox.Show("Данные об указанном периоде отсутствуют.");
                return;
            }

            switch (_selectedTypeOfDate)
            {
                case "День":
                    _startPeriod = new Date(((DateTime)_startPeriod).AddDays(coefficient * 1));
                    _endPeriod = new Date(((DateTime)_endPeriod).AddDays(coefficient * 1));
                    break;
                case "Неделя":
                    _startPeriod = new Date(((DateTime)_startPeriod).AddDays(coefficient * 7));
                    _endPeriod = new Date(((DateTime)_endPeriod).AddDays(coefficient * 7));
                    break;
                case "Месяц":
                    _startPeriod = new Date(((DateTime)_startPeriod).AddMonths(coefficient * 1));
                    _endPeriod = new Date(((DateTime)_endPeriod).AddMonths(coefficient * 1));
                    break;
                case "Год":
                    _startPeriod = new Date(((DateTime)_startPeriod).AddYears(coefficient * 1));
                    _endPeriod = new Date(((DateTime)_endPeriod).AddYears(coefficient * 1));
                    break;
                case "Определенный день":
                    _startPeriod = new Date(((DateTime)_startPeriod).AddDays(coefficient * 1));
                    _endPeriod = new Date(((DateTime)_endPeriod).AddDays(coefficient * 1));
                    break;
            }
            DataConfiguration();
            PeriodConfiguration();
        }

        private void SynchMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var synchronizationWindow = new SynchronizationWindow();
            synchronizationWindow.ShowDialog();
            _account = Account.GetAccount();
            StartWindowConfiguration();
        }

        private void DeleteMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var messageBoxResult = MessageBox.Show("Вы уверены, что хотите очистить данные?", "Подтверждение", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
                _account = Account.DeleteData();
            StartWindowConfiguration();
        }

        private void PeriodComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            var selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            _selectedTypeOfDate = selectedItem.Content.ToString();
            var date = DateTime.Now;
            switch (_selectedTypeOfDate)
            {
                case "День":
                    _startPeriod = Date.Now;
                    _endPeriod = Date.Now;
                    break;
                case "Неделя":
                {
                    var dayOfWeek = (int) date.DayOfWeek;
                    _startPeriod = new Date(date.AddDays(1 - dayOfWeek));
                    _endPeriod = new Date(date.AddDays(7 - dayOfWeek));
                    break;
                }
                case "Месяц":
                {
                    var month = date.Month;
                    var year = date.Year;
                    var daysInMonth = DateTime.DaysInMonth(year, month);
                    _startPeriod = new Date(1, month, year);
                    _endPeriod = new Date(daysInMonth, month, year);
                    break;
                }
                case "Год":
                {
                    var year = date.Year;
                    _startPeriod = new Date(1,1,year);
                    _endPeriod = new Date(31, 12, year);
                    break;
                }
                case "Определенный день":
                {
                        var calendar = new Calendar(_account.RegistrationDate, Date.Now);
                        calendar.ShowDialog();
                        _selectedTypeOfDate = "День";
                        PeriodComboBox.SelectedIndex = 0;
                        _startPeriod = calendar.date;
                        _endPeriod = calendar.date;
                        break;
                }
            }
            DataConfiguration();
            PeriodConfiguration();
        }

        private void ReportMenu_OnClick(object sender, RoutedEventArgs e)
        {
            var reportWindow = new ReportWindow();
            reportWindow.ShowDialog();
        }
    }
}

