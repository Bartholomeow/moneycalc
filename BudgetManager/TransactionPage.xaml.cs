using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace BudgetManager
{
    public partial class TransactionPage : UserControl
    {
        private readonly TypeOfCategory _type;
        private readonly Account _account;

        public TransactionPage(TypeOfCategory type)
        {
            InitializeComponent();
            _type = type;
            _account = Account.GetAccount();
            TransactionTextBlock.Text = "Введите " + type.ToString("g");
            if (CategoryListbox != null)
                CategoryConfiguration();
        }

        private void CategoryConfiguration()
        {
            CategoryListbox.ItemsSource = _account.GetCategories(_type);
        }
        private void AddCategoryButton_OnClick(object sender, RoutedEventArgs e)
        {
            var addCategoryWindow = new AddCategoryWindow(_type);
            addCategoryWindow.ShowDialog();
            CategoryConfiguration();
        }

        private void Calc_Click(object sender, RoutedEventArgs e)
        {
            if (TransactionTextBox.Text == "Error")
            {
                TransactionTextBox.Clear();
            }
            var button = (Button)sender;
            var value = button.Content.ToString();
            if (string.IsNullOrEmpty(value))
                return;
            switch (value)
            {
                case "=":
                    try
                    {
                        TransactionTextBox.Text = Calc.Calculate(TransactionTextBox.Text).ToString(CultureInfo.CurrentCulture);
                    }
                    catch
                    {
                        TransactionTextBox.Text = "Error";
                    }
                    break;
                default:
                    TransactionTextBox.Text = TransactionTextBox.Text.TrimStart('0') + value;
                    break;
            }
        }

        private void DeleteMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var category = (Category)CategoryListbox.SelectedItem;
            _account.DeleteCategory(category);
            CategoryConfiguration();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (TransactionTextBox.Text == "" || TransactionTextBox.Text == "0")
            {
                MessageBox.Show("Введите расход / доход.");
                return;
            }
            if (CategoryListbox.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите категорию.");
                return;
            }
            try
            {
                TransactionTextBox.Text = Calc.Calculate(TransactionTextBox.Text).ToString(CultureInfo.CurrentCulture);
            }
            catch
            {
                TransactionTextBox.Text = "Error";
                MessageBox.Show("Введите корректные данные в поле.");
                return;
            }
            var cost = double.Parse(TransactionTextBox.Text);
            var category = (Category)CategoryListbox.SelectedItem;
            _account.AddTransaction(new Transaction(Date.Now, category, cost));
            TransactionTextBox.Text = "0";
        }

        private void CRepeatButton_Click(object sender, RoutedEventArgs e)
        {
            if (TransactionTextBox.Text != "")
            {
                TransactionTextBox.Text = TransactionTextBox.Text.Remove(TransactionTextBox.Text.Length - 1);
            }
        }

        private void TransactionTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TransactionTextBox.Text == "Error")
            {
                TransactionTextBox.Clear();
            }
        }

        private void MainMenuButton_OnClick(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MainMenu());
        }
    }
}