using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.JsonSerializationDoubles
{
	public class HoursOfOpeningSerializationDouble
	{
		public HoursOfOpeningSerializationDouble()
		{			
		}

		public HoursOfOpeningSerializationDouble(HoursOfOpening hoursOfOpening)
		{
			OpeningTimeMonday    = new TimeSerializationDouble(hoursOfOpening.OpeningTimeMonday);
			OpeningTimeTuesday   = new TimeSerializationDouble(hoursOfOpening.OpeningTimeTuesday);
			OpeningTimeWednesday = new TimeSerializationDouble(hoursOfOpening.OpeningTimeWednesday);
			OpeningTimeThursday  = new TimeSerializationDouble(hoursOfOpening.OpeningTimeThursday);
			OpeningTimeFriday    = new TimeSerializationDouble(hoursOfOpening.OpeningTimeFriday);
			OpeningTimeSaturday  = new TimeSerializationDouble(hoursOfOpening.OpeningTimeSaturday);
			OpeningTimeSunday    = new TimeSerializationDouble(hoursOfOpening.OpeningTimeSunday);

			ClosingTimeMonday    = new TimeSerializationDouble(hoursOfOpening.ClosingTimeMonday);
			ClosingTimeTuesday   = new TimeSerializationDouble(hoursOfOpening.ClosingTimeTuesday);
			ClosingTimeWednesday = new TimeSerializationDouble(hoursOfOpening.ClosingTimeWednesday);
			ClosingTimeThursday  = new TimeSerializationDouble(hoursOfOpening.ClosingTimeThursday);
			ClosingTimeFriday    = new TimeSerializationDouble(hoursOfOpening.ClosingTimeFriday);
			ClosingTimeSaturday  = new TimeSerializationDouble(hoursOfOpening.ClosingTimeSaturday);
			ClosingTimeSunday    = new TimeSerializationDouble(hoursOfOpening.ClosingTimeSunday);

			IsOpenOnMonday    = hoursOfOpening.IsOpenOnMonday;
			IsOpenOnTuesday   = hoursOfOpening.IsOpenOnTuesday;
			IsOpenOnWednesday = hoursOfOpening.IsOpenOnWednesday;
			IsOpenOnThursday  = hoursOfOpening.IsOpenOnThursday;
			IsOpenOnFriday    = hoursOfOpening.IsOpenOnFriday;
			IsOpenOnSaturday  = hoursOfOpening.IsOpenOnSaturday;
			IsOpenOnSunday    = hoursOfOpening.IsOpenOnSunday;

			AdditionalClosedDays = hoursOfOpening.AdditionalClosedDays.Select(day => new DateSerializationDouble(day));
			AdditionalOpenedDays = hoursOfOpening.AdditionalOpenedDays.Select(day => new DateSerializationDouble(day));
		}

		public TimeSerializationDouble OpeningTimeMonday    { get; set; }
		public TimeSerializationDouble OpeningTimeTuesday   { get; set; }
		public TimeSerializationDouble OpeningTimeWednesday { get; set; }
		public TimeSerializationDouble OpeningTimeThursday  { get; set; }
		public TimeSerializationDouble OpeningTimeFriday    { get; set; }
		public TimeSerializationDouble OpeningTimeSaturday  { get; set; }
		public TimeSerializationDouble OpeningTimeSunday    { get; set; }

		public TimeSerializationDouble ClosingTimeMonday    { get; set; }
		public TimeSerializationDouble ClosingTimeTuesday   { get; set; }
		public TimeSerializationDouble ClosingTimeWednesday { get; set; }
		public TimeSerializationDouble ClosingTimeThursday  { get; set; }
		public TimeSerializationDouble ClosingTimeFriday    { get; set; }
		public TimeSerializationDouble ClosingTimeSaturday  { get; set; }
		public TimeSerializationDouble ClosingTimeSunday    { get; set; }

		public bool IsOpenOnMonday    { get; set; }
		public bool IsOpenOnTuesday   { get; set; }
		public bool IsOpenOnWednesday { get; set; }
		public bool IsOpenOnThursday  { get; set; }
		public bool IsOpenOnFriday    { get; set; }
		public bool IsOpenOnSaturday  { get; set; }
		public bool IsOpenOnSunday    { get; set; }

		public IEnumerable<DateSerializationDouble> AdditionalClosedDays { get; set; }
		public IEnumerable<DateSerializationDouble> AdditionalOpenedDays { get; set; }

		public HoursOfOpening GetHoursOfOpening()
		{
			return new HoursOfOpening(OpeningTimeMonday.GetTime(), 
									  OpeningTimeTuesday.GetTime(), 
									  OpeningTimeWednesday.GetTime(),
									  OpeningTimeThursday.GetTime(),
									  OpeningTimeFriday.GetTime(),
									  OpeningTimeSaturday.GetTime(),
									  OpeningTimeSunday.GetTime(),
									  ClosingTimeMonday.GetTime(),
									  ClosingTimeTuesday.GetTime(),
									  ClosingTimeWednesday.GetTime(),
									  ClosingTimeThursday.GetTime(),
									  ClosingTimeFriday.GetTime(),
									  ClosingTimeSaturday.GetTime(),
									  ClosingTimeSunday.GetTime(),
									  IsOpenOnMonday,
									  IsOpenOnTuesday,
									  IsOpenOnWednesday,
									  IsOpenOnThursday,
									  IsOpenOnFriday,
									  IsOpenOnSaturday,
									  IsOpenOnSunday,
									  AdditionalClosedDays.Select(dateDouble => dateDouble.GetDate()).ToList(),
									  AdditionalOpenedDays.Select(dateDouble => dateDouble.GetDate()).ToList());
		}
	}
}