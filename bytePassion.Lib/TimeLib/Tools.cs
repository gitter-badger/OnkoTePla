using System;

namespace bytePassion.Lib.TimeLib
{
	public static class Tools
	{
		public static Tuple<Date, Time> GetCurrentTimeStamp()
		{
			var currentTime = DateTime.Now;

			return new Tuple<Date, Time>(new Date(currentTime), new Time(currentTime));
		} 
	}
}
