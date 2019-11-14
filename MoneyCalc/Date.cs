using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyCalc
{
    public class Date
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public Date(int day, int month, int year)
        {
            Day = day;
            Month = month;
            Year = year;
        }
        public override bool Equals(object obj)
        {
            Date o = (Date)obj;
            return (Day == o.Day && Month == o.Month && Year == o.Year);
        }
        public override int GetHashCode()
        {
            string hash = Day + Month + Year + "";
            return int.Parse(hash);
        }

    }
    public class DateComparer : IComparer<Date>
    {
        public int Compare(Date x, Date y)
        {
            if (x.Year > y.Year) return 1;
            else if (x.Year < y.Year) return -1;
            else if (x.Month > y.Month) return 1;
            else if (x.Month < y.Month) return -1;
            else if (x.Day > y.Day) return 1;
            else if (x.Day > y.Day) return -1;
            else return 0;
        }
    }
}
