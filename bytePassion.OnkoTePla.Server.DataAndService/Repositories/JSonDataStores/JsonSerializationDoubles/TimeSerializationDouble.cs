using bytePassion.Lib.TimeLib;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.JSonDataStores.JsonSerializationDoubles
{
	public class TimeSerializationDouble
	{
		public TimeSerializationDouble()
		{			
		}

		public TimeSerializationDouble(Time time)
		{
			Hour   = time.Hour;
			Minute = time.Minute;
			Second = time.Second;
		}

		public byte Hour   { get; set; }
		public byte Minute { get; set; }
		public byte Second { get; set; }

		public Time GetTime()
		{
			return new Time(Hour, Minute, Second);
		}
	}
}