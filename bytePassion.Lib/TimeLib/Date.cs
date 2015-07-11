using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using bytePassion.Lib.FrameworkExtensions;


namespace bytePassion.Lib.TimeLib
{
    [DataContract]
	public class Date
	{		
		public static readonly Date Dummy = new Date(0,0,0);

        [DataMember(Name = "Day")]
		private readonly byte day;
        [DataMember(Name = "Month")]

        private readonly byte month;
        [DataMember(Name = "Year")]
        private readonly ushort year;

	    public Date()
	    {
	        
	    }

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

	    public static bool operator <(Date d1, Date d2)
	    {
		    if (d1.Year  > d2.Year)  return false;
		    if (d1.Month > d2.Month) return false;
		    if (d1.Day   > d2.Day)   return false;

		    return true;
	    }

		public static bool operator > (Date d1, Date d2)
		{
			if (d1.Year  < d2.Year)  return false;
			if (d1.Month < d2.Month) return false;
			if (d1.Day   < d2.Day)   return false;

			return true;
		}

	    public static bool operator <=(Date d1, Date d2)
	    {
		    return d1 < d2 || d1 == d2;
	    }

		public static bool operator >= (Date d1, Date d2)
		{
			return d1 > d2 || d1 == d2;
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

		public byte   Day   { get { return day;   }}
		public byte   Month { get { return month; }}
		public ushort Year  { get { return year;  }}

		#region static: Parse, IsDummy, GetDayOfWeekFrom

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

		public static bool IsDummy(Date d)
		{
			return d == Dummy;
		}

		public static DayOfWeek GetDayOfWeekFrom(Date d)
		{
			return new DateTime(d.Year, d.Month, d.Day).DayOfWeek;
		}		

		#endregion
	}
}