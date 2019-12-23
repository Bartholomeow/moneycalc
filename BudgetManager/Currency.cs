namespace BudgetManager
{
    public class Currency
    {
        public long Rubles { get; set; }

        public int Pennies { get; set; }

        public Currency(int rubles)
        {
            Rubles = rubles;
            Pennies = 0;
        }

        public Currency(long rubles, int pennies)
        {
            Rubles = rubles + (pennies / 100);
            Pennies = pennies % 100;
        }

        public Currency(double value)
        {
            Rubles = (long) value;
            Pennies = (int)(value * 100 % 100);
        }

        public static Currency operator +(Currency a, Currency b)
        {
            var money = new Currency((double)a + (double)b);
            return money;
        }
        public static Currency operator -(Currency a, Currency b)
        {
            var money = new Currency((double)a - (double)b);
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
        public static implicit operator Currency(long x)
        {
            return new Currency(x);
        }
        public static explicit operator double(Currency currency)
        {
            return currency.Rubles + currency.Pennies / 100.0;
        }
    }
}
