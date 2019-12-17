using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace BudgetManager
{
    public partial class PageSwitcher : Window
    {
        public PageSwitcher()
        {
            Serializer.AccountReader("config.txt");
            InitializeComponent();
            Switcher.PageSwitcher = this;
            Switcher.Switch(new MainMenu());
        }

        public void Navigate(UserControl nextPage)
        {
            Content = nextPage;
        }

        private void PageSwitcher_OnClosing(object sender, CancelEventArgs e)
        {
            Serializer.AccountWriter("config.txt");
        }
    }

    public static class Switcher
    {
        public static PageSwitcher PageSwitcher;

        public static void Switch(UserControl newPage)
        {
            PageSwitcher.Navigate(newPage);
        }
    }
}
