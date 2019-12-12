using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Windows.Input;

namespace BudgetManager
{
    public partial class MainWindow
    {
        private Account _account;
        //Период за который будут выводиться данные
        private TypeOfDate _selectedTypeOfDate;
        private Date _startPeriod, _endPeriod;

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
            _selectedTypeOfDate = TypeOfDate.День;
            PeriodComboBox.SelectedIndex = 0;
            PeriodConfiguration();
            DataConfiguration();
        }
        private void PeriodConfiguration()
        {
            switch (_selectedTypeOfDate)
            {
                case TypeOfDate.День:
                case TypeOfDate.ОпределённыйДень:
                    PeriodTextBlock.Text =
                        CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(((DateTime) _startPeriod).DayOfWeek).ToUpper() + ", " +
                        _startPeriod;
                    break;
                case TypeOfDate.Месяц:
                    PeriodTextBlock.Text =
                        CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(((DateTime) _startPeriod).Month).ToUpper() + ", " +
                        _startPeriod.Year;
                    break;
                case TypeOfDate.Год:
                    PeriodTextBlock.Text = _startPeriod.Year.ToString();
                    break;
                case TypeOfDate.Неделя:
                    PeriodTextBlock.Text = _startPeriod + " — " + _endPeriod;
                    break;
            }
        }
        private void DataConfiguration()
        {
            if (IncomesListBox == null)
            {
                return;
            }
            IncomesListBox.ItemsSource = _account.GetTransactionsAtPeriod(_startPeriod, _endPeriod, TypeOfCategory.Доход);
            ExpensesListBox.ItemsSource = _account.GetTransactionsAtPeriod(_startPeriod, _endPeriod, TypeOfCategory.Расход);
            SumOfExpensesTextBlock.Text = _account.GetSumOfTransactionsAtPeriod(_startPeriod, _endPeriod, TypeOfCategory.Расход).ToString();
            SumOfIncomesTextBlock.Text = _account.GetSumOfTransactionsAtPeriod(_startPeriod, _endPeriod, TypeOfCategory.Доход).ToString();
        }
        private static TypeOfDate TypeOfDateConfiguration(string typeOfDate)
        {
            switch (typeOfDate)
            {
                case "День":
                    return TypeOfDate.День;
                case "Неделя":
                    return TypeOfDate.Неделя;
                case "Месяц":
                    return TypeOfDate.Месяц;
                case "Год":
                    return TypeOfDate.Год;
                case "Определённый день":
                    return TypeOfDate.ОпределённыйДень;
                default: return TypeOfDate.День;
            }
        }
        private void SetPeriod(Date date1, Date date2)
        {
            _startPeriod = date1;
            _endPeriod = date2;
        }
        private void ChangeDate(int coefficient, Date date)
        {
            if (_startPeriod <= date && date <= _endPeriod)
            {
                MessageBox.Show("Данные об указанном периоде отсутствуют.");
                return;
            }

            switch (_selectedTypeOfDate)
            {
                case TypeOfDate.День:
                case TypeOfDate.ОпределённыйДень:
                    SetPeriod(new Date(((DateTime)_startPeriod).AddDays(coefficient * 1)), new Date(((DateTime)_endPeriod).AddDays(coefficient * 1)));
                    break;
                case TypeOfDate.Неделя:
                    SetPeriod(new Date(((DateTime)_startPeriod).AddDays(coefficient * 7)), new Date(((DateTime)_endPeriod).AddDays(coefficient * 7)));
                    break;
                case TypeOfDate.Месяц:
                    SetPeriod(new Date(((DateTime)_startPeriod).AddMonths(coefficient * 1)), new Date(((DateTime)_endPeriod).AddMonths(coefficient * 1)));
                    break;
                case TypeOfDate.Год:
                    SetPeriod(new Date(((DateTime)_startPeriod).AddYears(coefficient * 1)), new Date(((DateTime)_endPeriod).AddYears(coefficient * 1)));
                    break;
            }
            DataConfiguration();
            PeriodConfiguration();
        }
        private void Transaction_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var transactionWindow = button.Name == "IncomeButton" ? new TransactionWindow(TypeOfCategory.Доход) : new TransactionWindow(TypeOfCategory.Расход);
            transactionWindow.ShowDialog();
            DataConfiguration();
        }
        private void ChangeDateButtonClick(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "LeftDateButton":
                    ChangeDate(-1, _account.RegistrationDate);
                    break;
                case "RightDateButton":
                    ChangeDate(1, Date.Now);
                    break;
            }        
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
            if (messageBoxResult != MessageBoxResult.Yes) return;
            _account = Account.DeleteData();
            StartWindowConfiguration();
        }

        private void PeriodComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedTypeOfDate = TypeOfDateConfiguration(((ComboBoxItem)((ComboBox)sender).SelectedItem).Content.ToString());
            var date = DateTime.Now;
            switch (_selectedTypeOfDate)
            {
                case TypeOfDate.День:
                    SetPeriod(new Date(date), new Date(date));
                    break;
                case TypeOfDate.Неделя:
                {
                    var dayOfWeek = (int) date.DayOfWeek;
                    _startPeriod = new Date(date.AddDays(1 - dayOfWeek));
                    _endPeriod = new Date(date.AddDays(7 - dayOfWeek));
                    break;
                }
                case TypeOfDate.Месяц:
                {
                    var month = date.Month;
                    var year = date.Year;
                    var daysInMonth = DateTime.DaysInMonth(year, month);
                    _startPeriod = new Date(1, month, year);
                    _endPeriod = new Date(daysInMonth, month, year);
                    break;
                }
                case TypeOfDate.Год:
                {
                    var year = date.Year;
                    _startPeriod = new Date(1,1,year);
                    _endPeriod = new Date(31, 12, year);
                    break;
                }
                case TypeOfDate.ОпределённыйДень:
                {
                        var calendar = new Calendar();
                        calendar.ShowDialog();
                        _selectedTypeOfDate = TypeOfDate.День;
                        PeriodComboBox.SelectedIndex = 0;
                        _startPeriod = calendar.date;
                        _endPeriod = calendar.date;
                        break;
                }
            }
            DataConfiguration();
            PeriodConfiguration();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show(e.Key.ToString()); если нужно узнать название клавишы 
            switch (e.Key.ToString())
            {
                case "Left":
                    ChangeDate(-1, _account.RegistrationDate);
                    break;
                case "Right":
                    ChangeDate(1, Date.Now);
                    break;
            }


        }

        private void ReportMenu_OnClick(object sender, RoutedEventArgs e)
        {
            var reportWindow = new ReportWindow();
            reportWindow.ShowDialog();
            DataConfiguration();
        }
    }
    public enum TypeOfDate
    { День, Неделя, Месяц, Год, ОпределённыйДень}
}

