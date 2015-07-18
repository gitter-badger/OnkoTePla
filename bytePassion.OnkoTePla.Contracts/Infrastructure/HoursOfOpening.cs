using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using bytePassion.Lib.TimeLib;


namespace bytePassion.OnkoTePla.Contracts.Infrastructure
{
    [DataContract]
	public class HoursOfOpening
	{
        [DataMember(Name = "OpeningTimeMonday")]
		private readonly Time openingTimeMonday;

        [DataMember(Name = "OpeningTimeTuesday")]
		private readonly Time openingTimeTuesday;

        [DataMember(Name = "OpeningTimeWednesday")]
		private readonly Time openingTimeWednesday;

        [DataMember(Name = "OpeningTimeThursday")]
		private readonly Time openingTimeThursday;

        [DataMember(Name = "OpeningTimeFriday")]
		private readonly Time openingTimeFriday;

        [DataMember(Name = "OpeningTimeSaturday")]
		private readonly Time openingTimeSaturday;

        [DataMember(Name = "OpeningTimeSunday")]
		private readonly Time openingTimeSunday;

        [DataMember(Name = "ClosingTimeMonday")]
		private readonly Time closingTimeMonday;

        [DataMember(Name = "ClosingTimeTuesday")]
		private readonly Time closingTimeTuesday;

        [DataMember(Name = "ClosingTimeWednesday")]
		private readonly Time closingTimeWednesday;

        [DataMember(Name = "ClosingTimeThursday")]
		private readonly Time closingTimeThursday;

        [DataMember(Name = "ClosingTimeFriday")]
		private readonly Time closingTimeFriday;

        [DataMember(Name = "ClosingTimeSaturday")]
		private readonly Time closingTimeSaturday;

        [DataMember(Name = "ClosingTimeSunday")]
		private readonly Time closingTimeSunday;

        [DataMember(Name = "IsOpenOnMonday")]
		private readonly bool isOpenOnMonday;

        [DataMember(Name = "IsOpenOnTuesday")]
		private readonly bool isOpenOnTuesday;

        [DataMember(Name = "IsOpenOnWednesday")]
		private readonly bool isOpenOnWednesday;

        [DataMember(Name = "IsOpenOnThursday")]
		private readonly bool isOpenOnThursday;

        [DataMember(Name = "IsOpenOnFriday")]
		private readonly bool isOpenOnFriday;

        [DataMember(Name = "IsOpenOnSaturday")]
		private readonly bool isOpenOnSaturday;

        [DataMember(Name = "IsOpenOnSunday")]
		private readonly bool isOpenOnSunday;

        [DataMember(Name = "AdditionalClosedDays")]
		private readonly IReadOnlyList<Date> additionalClosedDays;

        [DataMember(Name = "AdditionalOpenedDays")]
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
