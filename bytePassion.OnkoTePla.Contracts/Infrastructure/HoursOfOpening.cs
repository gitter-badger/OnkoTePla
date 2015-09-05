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
        [DataMember(Name = "OpeningTimeMonday")]    private readonly Time openingTimeMonday;
        [DataMember(Name = "OpeningTimeTuesday")]   private readonly Time openingTimeTuesday;
        [DataMember(Name = "OpeningTimeWednesday")] private readonly Time openingTimeWednesday;
        [DataMember(Name = "OpeningTimeThursday")]  private readonly Time openingTimeThursday;
        [DataMember(Name = "OpeningTimeFriday")]    private readonly Time openingTimeFriday;
        [DataMember(Name = "OpeningTimeSaturday")]  private readonly Time openingTimeSaturday;
        [DataMember(Name = "OpeningTimeSunday")]    private readonly Time openingTimeSunday;
        [DataMember(Name = "ClosingTimeMonday")]    private readonly Time closingTimeMonday;
        [DataMember(Name = "ClosingTimeTuesday")]   private readonly Time closingTimeTuesday;
        [DataMember(Name = "ClosingTimeWednesday")] private readonly Time closingTimeWednesday;
        [DataMember(Name = "ClosingTimeThursday")]  private readonly Time closingTimeThursday;
        [DataMember(Name = "ClosingTimeFriday")]    private readonly Time closingTimeFriday;
        [DataMember(Name = "ClosingTimeSaturday")]  private readonly Time closingTimeSaturday;
        [DataMember(Name = "ClosingTimeSunday")]    private readonly Time closingTimeSunday;
        [DataMember(Name = "IsOpenOnMonday")]       private readonly bool isOpenOnMonday;
        [DataMember(Name = "IsOpenOnTuesday")]      private readonly bool isOpenOnTuesday;
        [DataMember(Name = "IsOpenOnWednesday")]    private readonly bool isOpenOnWednesday;
        [DataMember(Name = "IsOpenOnThursday")]     private readonly bool isOpenOnThursday;
        [DataMember(Name = "IsOpenOnFriday")]       private readonly bool isOpenOnFriday;
        [DataMember(Name = "IsOpenOnSaturday")]     private readonly bool isOpenOnSaturday;
        [DataMember(Name = "IsOpenOnSunday")]       private readonly bool isOpenOnSunday;
        [DataMember(Name = "AdditionalClosedDays")] private readonly IReadOnlyList<Date> additionalClosedDays;
        [DataMember(Name = "AdditionalOpenedDays")] private readonly IReadOnlyList<Date> additionalOpenedDays;

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


		public Time OpeningTimeMonday    => openingTimeMonday;
	    public Time OpeningTimeTuesday   => openingTimeTuesday;
	    public Time OpeningTimeWednesday => openingTimeWednesday;
	    public Time OpeningTimeThursday  => openingTimeThursday;
	    public Time OpeningTimeFriday    => openingTimeFriday;
	    public Time OpeningTimeSaturday  => openingTimeSaturday;
	    public Time OpeningTimeSunday    => openingTimeSunday;

	    public Time ClosingTimeMonday    => closingTimeMonday;
	    public Time ClosingTimeTuesday   => closingTimeTuesday;
	    public Time ClosingTimeWednesday => closingTimeWednesday;
	    public Time ClosingTimeThursday  => closingTimeThursday;
	    public Time ClosingTimeFriday    => closingTimeFriday;
	    public Time ClosingTimeSaturday  => closingTimeSaturday;
	    public Time ClosingTimeSunday    => closingTimeSunday;

	    public bool IsOpenOnMonday    => isOpenOnMonday;
	    public bool IsOpenOnTuesday   => isOpenOnTuesday;
	    public bool IsOpenOnWednesday => isOpenOnWednesday;
	    public bool IsOpenOnThursday  => isOpenOnThursday;
	    public bool IsOpenOnFriday    => isOpenOnFriday;
	    public bool IsOpenOnSaturday  => isOpenOnSaturday;
	    public bool IsOpenOnSunday    => isOpenOnSunday;

	    public IReadOnlyList<Date> AdditionalClosedDays => additionalClosedDays;
	    public IReadOnlyList<Date> AdditionalOpenedDays => additionalOpenedDays;

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
