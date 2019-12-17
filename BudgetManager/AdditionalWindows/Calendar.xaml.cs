using System;
using System.Windows.Controls;

namespace BudgetManager
{
    public partial class Calendar
    {
        public Date Date = Date.Now;
        public Calendar()
        {
            InitializeComponent();
            var account = Account.GetAccount();;
            calendar.DisplayDateStart = account.RegistrationDate;
            calendar.DisplayDateEnd = Date.Now;
        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calendar.SelectedDate != null) Date = new Date((DateTime) calendar.SelectedDate);
            Close();
        }
    }
}
