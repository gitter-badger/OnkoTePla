using System;
using System.Runtime.Serialization;
using System.Text;
using bytePassion.Lib.FrameworkExtensions;


namespace bytePassion.Lib.TimeLib
{
    [DataContract]
	public struct Time
	{
		public static readonly Time Dummy = new Time(0,0);

        [DataMember(Name = "Hour")]
		private readonly byte hour;

        [DataMember(Name = "Minute")]
        private readonly byte minute;

        [DataMember(Name = "Second")]
		private readonly byte second;

		public Time(Time time)
		{
			hour   = time.Hour;
			minute = time.Minute;
			second = time.Second;
		}

		public Time(DateTime time)
		{
			hour   = (byte) time.Hour;
			minute = (byte) time.Minute;
			second = (byte) time.Second;
		}

		public Time(byte hour, byte minute, byte second=0)
		{
			this.hour = hour;
			this.minute = minute;
			this.second = second;
		}

		#region Properties: Hour / Minute / Second / SecondsFromDayBegin

		public byte Hour   { get { return hour;   }}
		public byte Minute { get { return minute; }}
		public byte Second { get { return second; }}

		private uint SecondsFromDayBegin
		{
			get { return (uint)(Hour*3600 + Minute*60 + Second); }
		}

		#endregion

		#region operators: == , != , < , > . <= , >= , + , -

		public static bool operator ==(Time t1, Time t2)
		{
			return t1.Equals(t2);
		}

		public static bool operator !=(Time t1, Time t2)
		{
			return !(t1 == t2);
		}

		public static bool operator <(Time t1, Time t2)
		{
			return t1.SecondsFromDayBegin < t2.SecondsFromDayBegin;
		}

		public static bool operator >(Time t1, Time t2)
		{
			return t1.SecondsFromDayBegin > t2.SecondsFromDayBegin;
		}

		public static bool operator <=(Time t1, Time t2)
		{
			return t1 < t2 || t1 == t2;
		}

		public static bool operator >=(Time t1, Time t2)
		{
			return t1 > t2 || t1 == t2;
		}

		public static Time operator +(Time t, Duration d)
		{
			return GetTimeFromSecondsSinceBeginOfTheDay(t.SecondsFromDayBegin + d.Seconds);
		}

		public static Time operator -(Time t, Duration d)
		{
			return GetTimeFromSecondsSinceBeginOfTheDay(t.SecondsFromDayBegin - d.Seconds);
		}

		#endregion			

		#region ToString / Equals / GetHashCOde

		public override bool Equals (object obj)
		{
			return this.Equals(obj, (time1, time2) => time1.hour == time2.hour &&
													  time1.minute == time2.minute);
		}

		public override int GetHashCode ()
		{
			return hour.GetHashCode() ^ minute.GetHashCode();
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
			builder.Append(':');

			if (second < 10)
				builder.Append('0');

			builder.Append(second);

			return builder.ToString();
		}

		#endregion

		#region static: GetDurationBetween / Parse / IsDummy / GetTimeFromSecondsSinceBeginOfTheDay

		public static Duration GetDurationBetween (Time t1, Time t2)
		{
			return new Duration((uint)(System.Math.Abs((int)t1.SecondsFromDayBegin-(int)t2.SecondsFromDayBegin)));
		}

		public static Time Parse(string s)
		{
			var elements = s.Split(':');

			if (elements.Length == 2)
			{
				var hour = Byte.Parse(elements[0]);
				var minute = Byte.Parse(elements[1]);

				return new Time(hour, minute);
			}

			if (elements.Length == 3)
			{
				var hour = Byte.Parse(elements[0]);
				var minute = Byte.Parse(elements[1]);
				var seconds = Byte.Parse(elements[2]);

				return new Time(hour, minute, seconds);
			}

			throw new FormatException("expected Format: hh:mm");
		}

		public static bool IsDummy(Time t)
		{
			return t == Dummy;
		}

		public static Time GetTimeFromSecondsSinceBeginOfTheDay(uint seconds)
		{
			var timeSeconds = seconds % 60;
			var restTime    = (seconds - timeSeconds) / 60;
			var timeMinutes = restTime % 60;
			var timeHour    = (restTime - timeMinutes) / 60;

			return new Time((byte) timeHour, 
							(byte) timeMinutes, 
							(byte) timeSeconds);
		}

		#endregion
	}
}
