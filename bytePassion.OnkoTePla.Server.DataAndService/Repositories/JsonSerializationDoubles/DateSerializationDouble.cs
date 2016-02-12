using bytePassion.Lib.TimeLib;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.JsonSerializationDoubles
{

	public class DateSerializationDouble
	{						
		public DateSerializationDouble ()
		{			
		}

		public DateSerializationDouble (Date date)
		{
			Day   = date.Day;
			Month = date.Month;
			Year  = date.Year;
		}

		public byte   Day   { get; set; }
		public byte   Month { get; set; }
		public ushort Year  { get; set; }

		public Date GetDate()
		{
			return new Date(Day, Month, Year);
		}
	}
}