using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MoneyCalc
{
    public partial class SynchronizationWindow
    {
        public SynchronizationWindow()
        {
            InitializeComponent();
        }

        private void RestoreButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (File.Exists("synch.txt"))
            {
                Serializer.AccountReader("synch.txt");
                MessageBox.Show("Данные восстановлены.");
                Close();
            }
            else MessageBox.Show("Остутствуют сохранённые данные.");
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            Serializer.AccountWriter("synch.txt");
            MessageBox.Show("Данные сохранены.");
            Close();
        }
    }
}
