using System;
using System.Globalization;
using System.Text;
using bytePassion.Lib.FrameworkExtensions;


namespace bytePassion.Lib.TimeLib
{
	public struct Date
	{		
		public static readonly Date Dummy = new Date(0,0,0);

		private readonly byte day;
		private readonly byte month;
		private readonly ushort year;

		public Date(DateTime dateTime)
		{			
			day   =   (byte) dateTime.Day;
			month =   (byte) dateTime.Month;
			year  = (ushort) dateTime.Year;
		}

		public Date(byte day, byte month, ushort year)
		{
			this.day   = day;
			this.month = month;
			this.year  = year;
		}

		public override bool Equals(object obj)
		{					
			return this.Equals(obj, (date1, date2) => date1.day == date2.day &&
												      date1.month == date2.month &&
													  date1.year == date2.year);
		}

		public override int GetHashCode()
		{
			return year.GetHashCode() ^ month.GetHashCode() ^ day.GetHashCode();
		}

		public static bool operator == (Date d1, Date d2)
		{
			return d1.Equals(d2);
		}

		public static bool operator != (Date d1, Date d2)
		{
			return !(d1 == d2);
		}

		/// <summary>
		/// return Date as String in format: dd.mm.yyyy
		/// </summary>
		/// <returns>Date in format dd.mm.yyyy</returns>
		public override string ToString()
		{
			var builder = new StringBuilder();

			if (day < 10)
				builder.Append('0');

			builder.Append(day);
			builder.Append('.');

			if (month < 10)
				builder.Append('0');

			builder.Append(month);
			builder.Append('.');
			builder.Append(year);

			return builder.ToString();
		}

		public string GetDisplayString(CultureInfo cultureInfo)
		{
			var dateTime = new DateTime(year, month, day);
			return dateTime.ToString("d", cultureInfo);
		}

		public int Day   { get { return day;   }}
		public int Month { get { return month; }}
		public int Year  { get { return year;  }}

		public static Date Parse(string s)
		{
			var elements = s.Split('.');

			if (elements.Length != 3)
				throw new FormatException("expected Format: dd.mm.yyyy");

			var day   = Byte.Parse(elements[0]);
			var month = Byte.Parse(elements[1]);
			var year  = UInt16.Parse(elements[2]);

			return new Date(day, month, year);
		}

		public static bool IsDumme(Date d)
		{
			return d == Dummy;
		}
	}
}