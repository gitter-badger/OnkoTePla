using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.Lib.TimeLib;


namespace bytePassion.OnkoTePla.Contracts.Infrastructure
{
	public class HoursOfOpening
	{
		public static HoursOfOpening CreateDefault ()
		{
			var defaultOpeningTime = new Time( 8,0);
			var defaultClosingTime = new Time(17,0);

			return new HoursOfOpening(defaultOpeningTime, defaultOpeningTime, defaultOpeningTime, defaultOpeningTime,
									  defaultOpeningTime, defaultOpeningTime, defaultOpeningTime,
									  defaultClosingTime, defaultClosingTime, defaultClosingTime, defaultClosingTime,
									  defaultClosingTime, defaultClosingTime, defaultClosingTime,
									  true, true, true, true, true, false, false,
									  new List<Date>(), new List<Date>());
		}

		public HoursOfOpening(Time openingTimeMonday, Time openingTimeTuesday,  Time openingTimeWednesday, Time openingTimeThursday, 
							  Time openingTimeFriday, Time openingTimeSaturday, Time openingTimeSunday, 
							  Time closingTimeMonday, Time closingTimeTuesday,  Time closingTimeWednesday, Time closingTimeThursday,
							  Time closingTimeFriday, Time closingTimeSaturday, Time closingTimeSunday, 
							  bool isOpenOnMonday, bool isOpenOnTuesday,  bool isOpenOnWednesday, bool isOpenOnThursday, 
							  bool isOpenOnFriday, bool isOpenOnSaturday, bool isOpenOnSunday, 
							  IReadOnlyList<Date> additionalClosedDays, IReadOnlyList<Date> additionalOpenedDays)
		{
			OpeningTimeMonday    = openingTimeMonday;
			OpeningTimeTuesday   = openingTimeTuesday;
			OpeningTimeWednesday = openingTimeWednesday;
			OpeningTimeThursday  = openingTimeThursday;
			OpeningTimeFriday    = openingTimeFriday;
			OpeningTimeSaturday  = openingTimeSaturday;
			OpeningTimeSunday    = openingTimeSunday;

			ClosingTimeMonday    = closingTimeMonday;
			ClosingTimeTuesday   = closingTimeTuesday;
			ClosingTimeWednesday = closingTimeWednesday;
			ClosingTimeThursday  = closingTimeThursday;
			ClosingTimeFriday    = closingTimeFriday;
			ClosingTimeSaturday  = closingTimeSaturday;
			ClosingTimeSunday    = closingTimeSunday;

			IsOpenOnMonday       = isOpenOnMonday;
			IsOpenOnTuesday      = isOpenOnTuesday;
			IsOpenOnWednesday    = isOpenOnWednesday;
			IsOpenOnThursday     = isOpenOnThursday;
			IsOpenOnFriday       = isOpenOnFriday;
			IsOpenOnSaturday     = isOpenOnSaturday;
			IsOpenOnSunday       = isOpenOnSunday;

			AdditionalClosedDays = additionalClosedDays;
			AdditionalOpenedDays = additionalOpenedDays;
		}


		public Time OpeningTimeMonday    { get; }
		public Time OpeningTimeTuesday   { get; }
		public Time OpeningTimeWednesday { get; }
		public Time OpeningTimeThursday  { get; }
		public Time OpeningTimeFriday    { get; }
		public Time OpeningTimeSaturday  { get; }
		public Time OpeningTimeSunday    { get; }

		public Time ClosingTimeMonday    { get; }
		public Time ClosingTimeTuesday   { get; }
		public Time ClosingTimeWednesday { get; }
		public Time ClosingTimeThursday  { get; }
		public Time ClosingTimeFriday    { get; }
		public Time ClosingTimeSaturday  { get; }
		public Time ClosingTimeSunday    { get; }

		public bool IsOpenOnMonday    { get; }
		public bool IsOpenOnTuesday   { get; }
		public bool IsOpenOnWednesday { get; }
		public bool IsOpenOnThursday  { get; }
		public bool IsOpenOnFriday    { get; }
		public bool IsOpenOnSaturday  { get; }
		public bool IsOpenOnSunday    { get; }

		public IReadOnlyList<Date> AdditionalClosedDays { get; }
		public IReadOnlyList<Date> AdditionalOpenedDays { get; }

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
