using System.IO;
using System.Windows;

namespace BudgetManager
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