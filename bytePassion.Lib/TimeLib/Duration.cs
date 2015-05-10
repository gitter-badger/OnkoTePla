using System;
using bytePassion.Lib.FrameworkExtensions;


namespace bytePassion.Lib.TimeLib
{

	public class Duration
	{
		private readonly int minutes;

		public Duration(TimeSpan timeSpan)
		{
			minutes = timeSpan.Minutes;
		}

		public Duration(int minutes)
		{
			this.minutes = minutes;
		}
	
		public int Minutes { get { return minutes; }}

		public override bool Equals (object obj)
		{		
			return this.Equals(obj, (duration1, duration2) => duration1.minutes == duration2.minutes);			
		}

		public override int GetHashCode ()
		{
			return Minutes.GetHashCode();
		}

		public override string ToString ()
		{
			return Minutes.ToString();
		}
	}
}
