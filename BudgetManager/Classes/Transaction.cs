namespace BudgetManager
{
    public class Transaction
    {
        public Transaction(Date date, Category category, double cost)
        {
            Date = date;
            Category = category;
            Cost = cost;
            TypeOfCategory = category.TypeOfCategory;
        }

        public Transaction(string transaction)
        {
            var dataStrings = transaction.Split(' ');
            Date = new Date(dataStrings[0]);
            Category = new Category(dataStrings[1], dataStrings[2]);
            TypeOfCategory = Category.TypeOfCategory;
            Cost = double.Parse(dataStrings[3]);
        }

        public override string ToString()
        {
            return Date + " " +  Category.Name + " " + TypeOfCategory.ToString("g") + " " + Cost;
        }
        public Date Date { get;}
        public Category Category { get;}
        public TypeOfCategory TypeOfCategory { get; }
        public double Cost { get;}
    }
}