using System;
using System.Text;
using bytePassion.Lib.FrameworkExtensions;


namespace bytePassion.Lib.TimeLib
{
	public class Time
	{
		private readonly byte hour;
		private readonly byte minute;		

		public Time(DateTime time)
		{
			hour   = (byte) time.Hour;
			minute = (byte) time.Minute;			
		}

		public Time(byte hour, byte minute)
		{
			this.hour = hour;
			this.minute = minute;
		}

		public byte Hour   { get { return hour;   }}
		public byte Minute { get { return minute; }}

		public override bool Equals (object obj)
		{
			return this.Equals(obj, (time1, time2) => time1.hour == time2.hour &&
													  time1.minute == time2.minute);
		}

		public override int GetHashCode ()
		{
			return hour.GetHashCode() ^ minute.GetHashCode();
		}

		public static Duration operator - (Time t1, Time t2)
		{
			throw new NotImplementedException();
		}

		public override string ToString ()
		{
			var builder = new StringBuilder();

			if (hour < 10)
				builder.Append('0');

			builder.Append(hour);
			builder.Append(':');

			if (minute < 10)
				builder.Append('0');

			builder.Append(minute);

			return builder.ToString();
		}

		public static Time Parse(string s)
		{
			var elements = s.Split(':');

			if (elements.Length != 2)
				throw new FormatException("expected Format: hh:mm");

			var hour   = Byte.Parse(elements[0]);
			var minute = Byte.Parse(elements[1]);

			return new Time(hour, minute);
		}
	}
}
