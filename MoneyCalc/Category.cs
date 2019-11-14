using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyCalc
{
    public class Category
    {
        public string Name { get;}
        public Category(string name)
        {
            Name = name;
        }
        public override string ToString()
        {
            return Name;
        }
        public override int GetHashCode()
        {
            int hash = 0;
            for (int i = 0; i < Name.Length; i++)
            {
                hash += Name[i];
            }
            return hash;
        }
        public override bool Equals(object obj)
        {
            Category o = (Category)obj;
            return Name.Equals(o.Name);
        }
    }
}
