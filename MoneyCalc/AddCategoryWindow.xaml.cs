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
        public AddCategoryWindow(Account account, int incomeOrExpense)
        {
            _account = account;
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
            switch (_incomeOrExpense)
            {
                case 1:
                    _account.IncomeCategories.Add(new Category(AddCategoryTextBox.Text));
                    break;
                case 0:
                    _account.ExpenseCategories.Add(new Category(AddCategoryTextBox.Text));
                    break;
            }
            MessageBox.Show($"Добавлена категория {AddCategoryTextBox.Text}");
            Close();
        }
    }
}
