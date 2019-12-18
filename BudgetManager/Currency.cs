namespace BudgetManager
{
    public class Currency
    {
        public int Rubles { get; set; }

        public int Pennies { get; set; }

        public Currency(int rubles)
        {
            Rubles = rubles;
            Pennies = 0;
        }

        public Currency(int rubles, int pennies)
        {
            Rubles = rubles + (pennies / 100);
            Pennies = pennies % 100;
        }

        public Currency(double value)
        {
            Rubles = (int) value;
            Pennies = (int)(value * 100 % 100);
        }

        public static Currency operator +(Currency a, Currency b)
        {
            var money = new Currency(a.Rubles + b.Rubles, a.Pennies + b.Pennies);
            return money;
        }
        public static Currency operator -(Currency a, Currency b)
        {
            var money = new Currency(a.Rubles - b.Rubles, a.Pennies - b.Pennies);
            return money;
        }
        public static Currency operator +(Currency a, double b)
        {
            var b1 = new Currency(b);
            return a + b1;
        }
        public static Currency operator -(Currency a, double b)
        {
            var b1 = new Currency(b);
            return a - b1;
        }

        public override string ToString() =>
            $"{Rubles} руб. {Pennies} коп.";
        public static implicit operator Currency(double x)
        {
            return new Currency(x);
        }
        public static implicit operator Currency(int x)
        {
            return new Currency(x);
        }
    }
}
