using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.Lib.TimeLib;


namespace bytePassion.OnkoTePla.Contracts.Infrastructure
{
	public class HoursOfOpening
	{
		private readonly Time openingTimeMonday;
		private readonly Time openingTimeTuesday;
		private readonly Time openingTimeWednesday;
		private readonly Time openingTimeThursday;
		private readonly Time openingTimeFriday;
		private readonly Time openingTimeSaturday;
		private readonly Time openingTimeSunday;

		private readonly Time closingTimeMonday;
		private readonly Time closingTimeTuesday;
		private readonly Time closingTimeWednesday;
		private readonly Time closingTimeThursday;
		private readonly Time closingTimeFriday;
		private readonly Time closingTimeSaturday;
		private readonly Time closingTimeSunday;

		private readonly bool isOpenOnMonday;
		private readonly bool isOpenOnTuesday;
		private readonly bool isOpenOnWednesday;
		private readonly bool isOpenOnThursday;
		private readonly bool isOpenOnFriday;
		private readonly bool isOpenOnSaturday;
		private readonly bool isOpenOnSunday;


		private readonly IReadOnlyList<Date> additionalClosedDays;
		private readonly IReadOnlyList<Date> additionalOpenedDays;

		public HoursOfOpening(Time openingTimeMonday, Time openingTimeTuesday,  Time openingTimeWednesday, Time openingTimeThursday, 
							  Time openingTimeFriday, Time openingTimeSaturday, Time openingTimeSunday, 
							  Time closingTimeMonday, Time closingTimeTuesday,  Time closingTimeWednesday, Time closingTimeThursday,
							  Time closingTimeFriday, Time closingTimeSaturday, Time closingTimeSunday, 
							  bool isOpenOnMonday, bool isOpenOnTuesday,  bool isOpenOnWednesday, bool isOpenOnThursday, 
							  bool isOpenOnFriday, bool isOpenOnSaturday, bool isOpenOnSunday, 
							  IReadOnlyList<Date> additionalClosedDays, IReadOnlyList<Date> additionalOpenedDays)
		{
			this.openingTimeMonday    = openingTimeMonday;
			this.openingTimeTuesday   = openingTimeTuesday;
			this.openingTimeWednesday = openingTimeWednesday;
			this.openingTimeThursday  = openingTimeThursday;
			this.openingTimeFriday    = openingTimeFriday;
			this.openingTimeSaturday  = openingTimeSaturday;
			this.openingTimeSunday    = openingTimeSunday;

			this.closingTimeMonday    = closingTimeMonday;
			this.closingTimeTuesday   = closingTimeTuesday;
			this.closingTimeWednesday = closingTimeWednesday;
			this.closingTimeThursday  = closingTimeThursday;
			this.closingTimeFriday    = closingTimeFriday;
			this.closingTimeSaturday  = closingTimeSaturday;
			this.closingTimeSunday    = closingTimeSunday;

			this.isOpenOnMonday       = isOpenOnMonday;
			this.isOpenOnTuesday      = isOpenOnTuesday;
			this.isOpenOnWednesday    = isOpenOnWednesday;
			this.isOpenOnThursday     = isOpenOnThursday;
			this.isOpenOnFriday       = isOpenOnFriday;
			this.isOpenOnSaturday     = isOpenOnSaturday;
			this.isOpenOnSunday       = isOpenOnSunday;

			this.additionalClosedDays = additionalClosedDays;
			this.additionalOpenedDays = additionalOpenedDays;
		}


		public Time OpeningTimeMonday    { get { return openingTimeMonday;    }}
		public Time OpeningTimeTuesday   { get { return openingTimeTuesday;   }}
		public Time OpeningTimeWednesday { get { return openingTimeWednesday; }}
		public Time OpeningTimeThursday  { get { return openingTimeThursday;  }}
		public Time OpeningTimeFriday    { get { return openingTimeFriday;    }}
		public Time OpeningTimeSaturday  { get { return openingTimeSaturday;  }}
		public Time OpeningTimeSunday    { get { return openingTimeSunday;    }}
		
		public Time ClosingTimeMonday	 { get { return closingTimeMonday;    }}
		public Time ClosingTimeTuesday	 { get { return closingTimeTuesday;   }}
		public Time ClosingTimeWednesday { get { return closingTimeWednesday; }}
		public Time ClosingTimeThursday	 { get { return closingTimeThursday;  }}
		public Time ClosingTimeFriday	 { get { return closingTimeFriday;    }}
		public Time ClosingTimeSaturday	 { get { return closingTimeSaturday;  }}
		public Time ClosingTimeSunday	 { get { return closingTimeSunday;    }}
		
		public bool IsOpenOnMonday    { get { return isOpenOnMonday;    }}
		public bool IsOpenOnTuesday	  { get { return isOpenOnTuesday;   }}
		public bool IsOpenOnWednesday { get { return isOpenOnWednesday; }}
		public bool IsOpenOnThursday  { get { return isOpenOnThursday;  }}
		public bool IsOpenOnFriday	  { get { return isOpenOnFriday;    }}
		public bool IsOpenOnSaturday  { get { return isOpenOnSaturday;  }}
		public bool IsOpenOnSunday	  { get { return isOpenOnSunday;    }}

		public IReadOnlyList<Date> AdditionalClosedDays { get { return additionalClosedDays; }}
		public IReadOnlyList<Date> AdditionalOpenedDays { get { return additionalOpenedDays; }}

		public Time GetOpeningTime(Date d)
		{
			var weekDay = Date.GetDayOfWeekFrom(d);			

			switch (weekDay)
			{
				case DayOfWeek.Monday:    return OpeningTimeMonday;
				case DayOfWeek.Tuesday:   return OpeningTimeTuesday;
				case DayOfWeek.Wednesday: return OpeningTimeWednesday;
				case DayOfWeek.Thursday:  return OpeningTimeThursday;
				case DayOfWeek.Friday:    return OpeningTimeFriday;
				case DayOfWeek.Saturday:  return OpeningTimeSaturday;
				case DayOfWeek.Sunday:    return OpeningTimeSunday;
			}

			throw new ArgumentException();
		}

		public Time GetClosingTime(Date d)
		{
			var weekDay = Date.GetDayOfWeekFrom(d);

			if (!IsOpen(weekDay, d))
				return Time.Dummy;

			switch (weekDay)
			{
				case DayOfWeek.Monday:    return ClosingTimeMonday;
				case DayOfWeek.Tuesday:   return ClosingTimeTuesday;
				case DayOfWeek.Wednesday: return ClosingTimeWednesday;
				case DayOfWeek.Thursday:  return ClosingTimeThursday;
				case DayOfWeek.Friday:    return ClosingTimeFriday;
				case DayOfWeek.Saturday:  return ClosingTimeSaturday;
				case DayOfWeek.Sunday:    return ClosingTimeSunday;
			}

			throw new ArgumentException();
		}

		public bool IsOpen(Date d)
		{
			var weekDay = Date.GetDayOfWeekFrom(d);
			return IsOpen(weekDay, d);
		}

		private bool IsOpen(DayOfWeek day, Date d)
		{
			if (AdditionalClosedDays.Contains(d))
				return false;

			if (AdditionalOpenedDays.Contains(d))
				return true;

			switch (day)
			{
				case DayOfWeek.Monday:	  return IsOpenOnMonday;
				case DayOfWeek.Tuesday:   return IsOpenOnTuesday;
				case DayOfWeek.Wednesday: return IsOpenOnWednesday;
				case DayOfWeek.Thursday:  return IsOpenOnThursday;
				case DayOfWeek.Friday:    return IsOpenOnFriday;
				case DayOfWeek.Saturday:  return IsOpenOnSaturday;
				case DayOfWeek.Sunday:    return IsOpenOnSunday;
			}

			throw new ArgumentException();
		}

		public Date GetLastOpenDayFromToday()
		{
			var currentDate = TimeTools.Today();

			var securityCounter = 0;

			while (!IsOpen(currentDate) && (securityCounter++)<1000)
				currentDate = currentDate.DayBefore();

			if (securityCounter > 990)
				throw new ArgumentException();

			return currentDate;
		}			
	}
}
