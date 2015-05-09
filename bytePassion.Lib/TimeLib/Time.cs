using System;
using System.Globalization;


namespace bytePassion.Lib.TimeLib
{
	public class Time
	{
		private readonly int hour;
		private readonly int minute;

		private readonly DateTime dateTime;

		public Time(DateTime time)
		{
			hour = time.Hour;
			minute = time.Minute;

			dateTime = time;
		}

		public Time(int hour, int minute)
		{
			dateTime = new DateTime(0,0,0,hour, minute, 0);
		}

		public int Hour   { get { return hour;   }}
		public int Minute { get { return minute; }}

		public override bool Equals (object obj)
		{

			Func<Time, Time, bool> timeCompareFunc = (time1, time2) => time1.hour == time2.hour &&
																	   time1.minute == time2.minute;

			return Generic.Equals(this, obj, timeCompareFunc);
		}

		public override int GetHashCode ()
		{
			return hour.GetHashCode() ^ minute.GetHashCode();
		}

		public override string ToString ()
		{
			// TODO: CultureInfo nicht hard coden!
			return dateTime.ToString("t", new CultureInfo("de-DE"));
		}
	}
}
