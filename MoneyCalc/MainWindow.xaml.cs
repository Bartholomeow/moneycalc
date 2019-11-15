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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoneyCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Account account;
        public MainWindow()
        {
            account = Account.getAccount();
            DataContext = account;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IncomeWindow income = new IncomeWindow(account);
            income.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ExpenseWindow expense = new ExpenseWindow(account);
            expense.ShowDialog();
        }
    }
}
