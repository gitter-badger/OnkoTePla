using System;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;

using static bytePassion.Lib.FrameworkExtensions.EqualsExtension;


namespace bytePassion.OnkoTePla.Contracts.Appointments
{
	public class Appointment
	{
		public Appointment (Patient patient, string description, TherapyPlace therapyPlace, 
						    Date day, Time startTime, Time endTime, Guid id)
		{
			Patient      = patient;
			TherapyPlace = therapyPlace;			
		    Day          = day;
			StartTime    = startTime;
			EndTime      = endTime;
		    Description  = description;
			Id           = id;
		}
		
		public Patient      Patient      { get; }
		public string       Description  { get; }
		public Date         Day          { get; }
		public Time         StartTime    { get; }
		public Time         EndTime      { get; }
		public TherapyPlace TherapyPlace { get; }
		public Guid         Id           { get; }				

		public Duration Duration => new Duration(StartTime, EndTime);
		
		
		public static bool operator ==(Appointment a1, Appointment a2) => EqualsForEqualityOperator(a1,a2);
		public static bool operator !=(Appointment a1, Appointment a2) => !(a1 == a2);

		
		#region ToString / HashCode / Equals

		public override string ToString()
		{
			return $"[{Patient} am {Day} von {StartTime} bis {EndTime} an platz {TherapyPlace}]";
		}

		public override bool Equals(object obj)
		{
			return this.Equals(obj, (appointment1, appointment2) => appointment1.Id.Equals(appointment2.Id) &&
																    appointment1.Description.Equals(appointment2.Description) &&
																	appointment1.Day.Equals(appointment2.Day) &&
																	appointment1.StartTime.Equals(appointment2.StartTime) &&
																	appointment1.EndTime.Equals(appointment2.EndTime) &&
																	appointment1.TherapyPlace.Equals(appointment2.TherapyPlace));
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode() ^
			       Description.GetHashCode() ^
			       Day.GetHashCode() ^
			       StartTime.GetHashCode() ^
			       EndTime.GetHashCode() ^
			       TherapyPlace.GetHashCode();
		}

		#endregion
	}
}