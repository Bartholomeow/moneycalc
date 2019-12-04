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
    /// Логика взаимодействия для Calendar.xaml
    /// </summary>
    public partial class Calendar : Window
    {
        private readonly Account _account;
        public Date date = Date.Now;
        public Calendar()
        {
            InitializeComponent();
            _account = Account.GetAccount();;
            calendar.DisplayDateStart = _account.RegistrationDate;
            calendar.DisplayDateEnd = Date.Now;
        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            date = new Date((DateTime)calendar.SelectedDate);
            Close();
        }
    }
}
