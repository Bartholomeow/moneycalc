﻿using System;
using System.Collections.Generic;

namespace BudgetManager
{
    public class Date
    {
        public int Day { get; }
        public int Month { get; }
        public int Year { get; }
        public Date(int day, int month, int year)
        {
            Day = day;
            Month = month;
            if ((int) Math.Floor(Math.Log10(year)) + 1 == 2)
            {
                Year = 2000 + year;
            }
            else
            {
                Year = year;
            }
        }
        public Date(DateTime date) : this(date.Day, date.Month, date.Year) { }
        public static Date Now => new Date(DateTime.Now);

        public Date(string date) 
        {
            var dateString = date.Split('.');
            Day = int.Parse(dateString[0]);
            Month = int.Parse(dateString[1]);
            var year = int.Parse(dateString[2]);
            if ((int) Math.Floor(Math.Log10(year)) + 1 == 2)
            {
                Year = 2000 + year;
            }
            else
            {
                Year = year;
            }
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

        public static bool operator <(Date x, Date y)
        {
            var dateComparer = new DateComparer();
            var result = dateComparer.Compare(x, y);
            return result == -1;
        }

        public static bool operator <=(Date x, Date y)
        {
            var dateComparer = new DateComparer();
            var result = dateComparer.Compare(x, y);
            return result == -1 || result == 0;
        }

        public static bool operator >(Date x, Date y)
        {
            var dateComparer = new DateComparer();
            var result = dateComparer.Compare(x, y);
            return result == 1;
        }

        public static bool operator >=(Date x, Date y)
        {
            var dateComparer = new DateComparer();
            var result = dateComparer.Compare(x, y);
            return result == 1 || result == 0;
        }

        public static implicit operator DateTime(Date date)
        {
            return new DateTime(date.Year,date.Month,date.Day);
        }
    }

    public class DateComparer :IComparer<Date>
    {
        public int Compare(Date x, Date y)
        {
            if (y != null && (x != null && x.Year > y.Year)) return 1;
            if (y != null && (x != null && x.Year < y.Year)) return -1;
            if (y != null && (x != null && x.Month > y.Month)) return 1;
            if (y != null && (x != null && x.Month < y.Month)) return -1;
            if (y != null && (x != null && x.Day > y.Day)) return 1;
            if (y != null && (x != null && x.Day < y.Day)) return -1;
            return 0;
        }
    }
}
