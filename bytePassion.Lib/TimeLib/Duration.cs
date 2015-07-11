using System;
using bytePassion.Lib.FrameworkExtensions;


namespace bytePassion.Lib.TimeLib
{

	public struct Duration
	{
		public static readonly Duration Zero = new Duration(0);

		private readonly uint seconds;

		public Duration(TimeSpan timeSpan)
		{
			seconds = (uint) timeSpan.Seconds;
		}

		public Duration(uint seconds)
		{
			this.seconds = seconds;
		}
	
		public uint Seconds { get { return seconds; }}

		public override bool Equals (object obj)
		{		
			return this.Equals(obj, (duration1, duration2) => duration1.seconds == duration2.seconds);			
		}

		public override int GetHashCode ()
		{
			return Seconds.GetHashCode();
		}

		public override string ToString ()
		{
			return Seconds.ToString();
		}
	}
}
