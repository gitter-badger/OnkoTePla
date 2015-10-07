using System;
using System.Runtime.Serialization;
using System.Text;
using bytePassion.Lib.FrameworkExtensions;


namespace bytePassion.Lib.TimeLib
{
    [DataContract]
	public struct Time : IComparable<Time>
    {
		public static readonly Time Dummy = new Time(0,0);

        [DataMember(Name = "Hour")]   private readonly byte hour;
        [DataMember(Name = "Minute")] private readonly byte minute;
        [DataMember(Name = "Second")] private readonly byte second;

		public Time(Time time) 
			: this(time.Hour, time.Minute, time.Second)
		{						
		}

		public Time(DateTime time) 
			: this((byte) time.Hour, (byte) time.Minute, (byte) time.Second)
		{			
		}

		public Time (uint secondsFromDayBegin)
		{

			if (secondsFromDayBegin > 86399)
				throw new ArgumentException("parameter secondsFromDayBegin has to be in range from 0 to 86399");

			var timeSeconds = secondsFromDayBegin % 60;
			var restTime    = (secondsFromDayBegin - timeSeconds) / 60;
			var timeMinutes = restTime % 60;
			var timeHour    = (restTime - timeMinutes) / 60;

			hour   = (byte) timeHour;
			minute = (byte) timeMinutes;
			second = (byte) timeSeconds;

			SecondsFromDayBegin = secondsFromDayBegin;
		}

		public Time(byte hour, byte minute, byte second=0)
		{
			if (hour   > 23) throw new ArgumentException("parameter hour has to be in the range from 0 to 23");
			if (minute > 59) throw new ArgumentException("parameter minute has to be in the range from 0 to 59");
			if (second > 59) throw new ArgumentException("parameter second has to be in the range from 0 to 59");

			this.hour = hour;
			this.minute = minute;
			this.second = second;

			SecondsFromDayBegin = (uint)(hour * 3600 + minute * 60 + second);
		}

		#region Properties: Hour / Minute / Second / SecondsFromDayBegin

		public byte Hour   { get { return hour;   }}
		public byte Minute { get { return minute; }}
		public byte Second { get { return second; }}

	    public uint SecondsFromDayBegin { get; }

		#endregion

		#region operators: == , != , < , > . <= , >= , + , -

	    public static bool operator ==(Time t1, Time t2) => EqualsExtension.EqualsForEqualityOperator(t1, t2);
	    public static bool operator !=(Time t1, Time t2) => !(t1 == t2);

	    public static bool operator <(Time t1, Time t2) => t1.SecondsFromDayBegin < t2.SecondsFromDayBegin;
	    public static bool operator >(Time t1, Time t2) => t1.SecondsFromDayBegin > t2.SecondsFromDayBegin;

	    public static bool operator <=(Time t1, Time t2) => t1 < t2 || t1 == t2;
	    public static bool operator >=(Time t1, Time t2) => t1 > t2 || t1 == t2;

	    public static Time operator +(Time t, Duration d) => new Time(t.SecondsFromDayBegin + d.Seconds);
	    public static Time operator -(Time t, Duration d) => new Time(t.SecondsFromDayBegin - d.Seconds);

		#endregion

		public int CompareTo (Time other)
		{
			if (this > other) return 1;
			if (this == other) return 0;
			return -1;
		}

		#region ToString / Equals / GetHashCOde		

		public override bool Equals (object obj)
		{
			return this.Equals(obj, (time1, time2) => time1.SecondsFromDayBegin == time2.SecondsFromDayBegin);
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

		#region static: Parse / IsDummy
		

		public static Time Parse(string s)
		{
			var elements = s.Split(':');

			if (elements.Length == 2)
			{
				var hour   = Byte.Parse(elements[0]);
				var minute = Byte.Parse(elements[1]);

				return new Time(hour, minute);
			}

			if (elements.Length == 3)
			{
				var hour    = Byte.Parse(elements[0]);
				var minute  = Byte.Parse(elements[1]);
				var seconds = Byte.Parse(elements[2]);

				return new Time(hour, minute, seconds);
			}

			throw new FormatException("expected Format: hh:mm or hh:mm:ss");
		}

		public static bool IsDummy(Time t)
		{
			return t == Dummy;
		}		

		#endregion
	}	
}
