using System.Windows;
using System.Windows.Media;

namespace BudgetManager
{
    public partial class AddCategoryWindow
    {
        private readonly Account _account;
        private readonly TypeOfCategory type;
        public AddCategoryWindow(TypeOfCategory type)
        {
            InitializeComponent();
            _account = Account.GetAccount();
            this.type = type;
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
            int CheckAddCategory = _account.Categories.Count;
            _account.AddCategory(new Category(AddCategoryTextBox.Text, type));
            if (CheckAddCategory != _account.Categories.Count)
                MessageBox.Show($"Добавлена категория {AddCategoryTextBox.Text}");
            else
                MessageBox.Show($"Категория {AddCategoryTextBox.Text} уже существует ");
            Close();
        }

        private void AddCategoryTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            AddCategoryTextBox.Foreground = Brushes.Black;
            AddCategoryTextBox.Text = "";
        }
    }
}