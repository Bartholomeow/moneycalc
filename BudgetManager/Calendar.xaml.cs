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
        public Date date;
        public Calendar(Date Start, Date End)
        {
            InitializeComponent();
            calendar.DisplayDateStart = Start;
            calendar.DisplayDateEnd = End;
        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? selectedDate = calendar.SelectedDate;
            date = new Date(selectedDate.Value.Date.Day, selectedDate.Value.Date.Month, selectedDate.Value.Date.Year);
            this.Close();
        }
    }
}
