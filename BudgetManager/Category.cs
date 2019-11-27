using System.Linq;

namespace BudgetManager
{
    public class Category
    {
        public string Name { get;}
        public Category(string name)
        {
            Name = name;
        }
        public override string ToString() => Name;
        public override int GetHashCode() => Name.Aggregate(0, (current, t) => current + t);

        public override bool Equals(object obj)
        {
            var o = (Category)obj;
            return o != null && Name.Equals(o.Name);
        }
    }
}
