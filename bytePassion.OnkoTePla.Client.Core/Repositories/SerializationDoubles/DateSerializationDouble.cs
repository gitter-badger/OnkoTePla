using System.Runtime.Serialization;
using bytePassion.Lib.TimeLib;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.SerializationDoubles
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

		[DataMember(Name = "Day")]   public byte   Day   { get; set; }
		[DataMember(Name = "Month")] public byte   Month { get; set; }
		[DataMember(Name = "Year")]  public ushort Year  { get; set; }
	}
}