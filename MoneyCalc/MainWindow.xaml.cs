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
using System.Text.Json;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace MoneyCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Account _account;
        public MainWindow()
        {
            _account = Serializer.AccountReader("config.txt");
            DataContext = _account;
            InitializeComponent();
        }

        private void transactionClick(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            var transactionWindow = button.Name == "incomeButton" ? new TransactionWindow(1) : new TransactionWindow(0);

            transactionWindow.ShowDialog();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Serializer.AccountWriter("config.txt", _account);
        }
    }
}
