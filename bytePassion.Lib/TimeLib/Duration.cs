using System;
using bytePassion.Lib.FrameworkExtensions;


namespace bytePassion.Lib.TimeLib
{

	public struct Duration
	{
		public static readonly Duration Zero = new Duration(0);

		public Duration(TimeSpan timeSpan)
		{
			Seconds = (uint) timeSpan.Seconds;
		}

		public Duration(uint seconds)
		{
			Seconds = seconds;
		}
	
		public uint Seconds { get; }

		public override bool   Equals (object obj) => this.Equals(obj, (duration1, duration2) => duration1.Seconds == duration2.Seconds);
		public override int    GetHashCode ()      => Seconds.GetHashCode();
		public override string ToString ()         => Seconds.ToString();
	}
}
