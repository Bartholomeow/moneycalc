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
        public Date(string date)
        {
            string[] d = date.Split('.');
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
            Date o = (Date)obj;
            return (Day == o.Day && Month == o.Month && Year == o.Year);
        }
        public override int GetHashCode()
        {
            string hash = Day + Month + Year + "";
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
            else if (x.Year < y.Year) return -1;
            else if (x.Month > y.Month) return 1;
            else if (x.Month < y.Month) return -1;
            else if (x.Day > y.Day) return 1;
            else if (x.Day > y.Day) return -1;
            else return 0;
        }
    }
}
