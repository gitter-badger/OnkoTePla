using System;
using System.Globalization;


namespace bytePassion.Lib.TimeLib
{
	public class Date
	{
		private readonly DateTime dateTime;
		private readonly int dateTimeHash;

		private readonly int day;
		private readonly int month;
		private readonly int year;

		public Date(DateTime dateTime)
		{
			this.dateTime = dateTime;

			day = dateTime.Day;
			month = dateTime.Month;
			year = dateTime.Year;

			dateTimeHash = year.GetHashCode() ^ 
						   month.GetHashCode() ^ 
						   day.GetHashCode();
		}

		public override bool Equals(object obj)
		{

			Func<Date, Date, bool> dateCompareFunc = (date1, date2) => date1.day == date2.day &&
																	   date1.month == date2.month &&
																	   date1.year == date2.year; 

			return Generic.Equals(this, obj, dateCompareFunc);			
		}

		public override int GetHashCode()
		{
			return dateTimeHash;
		}

		public override string ToString()
		{
			// TODO: CultureInfo nicht hard coden!
			return dateTime.ToString("d", new CultureInfo("de-DE"));
		}

		public int Day   { get { return day;   }}
		public int Month { get { return month; }}
		public int Year  { get { return year;  }}
	}
}