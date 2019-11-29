using System.Windows;
using System.Windows.Media;

namespace BudgetManager
{
    public partial class AddCategoryWindow
    {
        private readonly Account _account;
        private readonly byte _incomeOrExpense;
        public AddCategoryWindow(byte incomeOrExpense)
        {
            InitializeComponent();
            _account = Account.GetAccount();
            _incomeOrExpense = incomeOrExpense;
            AddCategoryTextBox.Foreground = Brushes.LightGray;
            AddCategoryTextBox.Text = "Не более 12 букв.";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddCategoryTextBox.Text == "" || AddCategoryTextBox.Text == "Не более 12 букв.")
            {
                MessageBox.Show("Введите название категории.");
                return;
            }
            if (_incomeOrExpense == 1)
                _account.AddIncomeCategory(AddCategoryTextBox.Text);
            else _account.AddExpenseCategory(AddCategoryTextBox.Text);
            MessageBox.Show($"Добавлена категория {AddCategoryTextBox.Text}");
            Close();
        }

        private void AddCategoryTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            AddCategoryTextBox.Foreground = Brushes.Black;
            AddCategoryTextBox.Text = "";
        }
    }
}