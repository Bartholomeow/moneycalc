using System.Linq;

namespace BudgetManager
{
    public class Category
    {
        public string Name { get;}
        public TypeOfCategory TypeOfCategory { get; }

        public Category(string name, TypeOfCategory typeOfCategory)
        {
            Name = name;
            TypeOfCategory = typeOfCategory;
        }
        public Category(string name, string typeOfCategory)
        {
            Name = name;
            switch (typeOfCategory)
            {
                case "Доход":
                    TypeOfCategory = TypeOfCategory.Доход;
                    break;
                case "Расход":
                    TypeOfCategory = TypeOfCategory.Расход;
                    break;
            }
        }
        public override string ToString() => Name;
        public override int GetHashCode() => Name.Aggregate(0, (current, c) => current + c) * (int)TypeOfCategory;

        public override bool Equals(object obj)
        {
            var o = (Category)obj;
            return o != null && Name.Equals(o.Name) && TypeOfCategory.Equals(o.TypeOfCategory);
        }
    }
}
