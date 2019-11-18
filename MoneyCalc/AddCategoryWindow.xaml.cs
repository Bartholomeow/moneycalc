using System.Windows;

namespace MoneyCalc
{
    /// <summary>
    /// Логика взаимодействия для AddCategoryWindow.xaml
    /// </summary>
    public partial class AddCategoryWindow
    {
        private readonly Account _account;
        private readonly int _incomeOrExpense;
        public AddCategoryWindow(int incomeOrExpense)
        {
            _account = Account.GetAccount();
            _incomeOrExpense = incomeOrExpense;
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddCategoryTextBox.Text == "")
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
    }
}
