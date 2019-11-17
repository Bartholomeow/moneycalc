using System.Collections.Generic;

namespace MoneyCalc
{
    public class Date
    {
        public int Day { get; }
        public int Month { get; }
        public int Year { get; }
        public Date(string date)
        {
            var d = date.Split('.');
            Day = int.Parse(d[0]);
            Month = int.Parse(d[1]);
            Year = int.Parse(d[2]);
        }
        public Date(int day, int month, int year)
        {
            Day = day;
            Month = month;
            Year = year;
        }
        public override bool Equals(object obj)
        {
            var o = (Date)obj;
            return o != null && (Day == o.Day && Month == o.Month && Year == o.Year);
        }
        public override int GetHashCode()
        {
            var hash = Day + Month + Year + "";
            return int.Parse(hash);
        }
        public override string ToString()
        {
            return Day + "." + Month + "." + Year;
        }

    }
    public class DateComparer : IComparer<Date>
    {
        public int Compare(Date x, Date y)
        {
            if (x.Year > y.Year) return 1;
            if (x.Year < y.Year) return -1;
            if (x.Month > y.Month) return 1;
            if (x.Month < y.Month) return -1;
            if (x.Day > y.Day) return 1;
            if (x.Day > y.Day) return -1;
            return 0;
        }
    }
}
