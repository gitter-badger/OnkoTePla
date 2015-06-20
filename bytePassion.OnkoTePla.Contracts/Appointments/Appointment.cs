using System;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Contracts.Appointments
{
	public class Appointment
	{
		private readonly Patient      patient;

		private readonly string       description;
		private readonly Date         day;
		private readonly Time         startTime;
		private readonly Time         endTime;
		private readonly TherapyPlace therapyPlace;
		private readonly Guid         id;
	    

	    public Appointment (Patient patient, string description, TherapyPlace therapyPlace, 
						    Date day, Time startTime, Time endTime, Guid id)
		{
			this.patient      = patient;
			this.therapyPlace = therapyPlace;			
		    this.day          = day;
			this.startTime    = startTime;
			this.endTime      = endTime;
		    this.description  = description;
			this.id           = id;
		}

		#region properties: Patient / Description / Day / StartTime / EndTime / TherapyPlace

		public Patient      Patient      { get { return patient;      }}
		public string       Description  { get { return description;  }}
		public Date         Day          { get { return day;          }}
		public Time         StartTime    { get { return startTime;    }}
		public Time         EndTime      { get { return endTime;      }}
		public TherapyPlace TherapyPlace { get { return therapyPlace; }}
		public Guid         Id           { get { return id;           }}				

		#endregion

		#region properties: Duration 

		public Duration Duration
		{
			get { return Time.GetDurationBetween(StartTime, EndTime); }
		}

		#endregion

		#region operations

		// TODO hier modifications

		#endregion

		#region ToString / HashCode / Equals

		public override string ToString()
		{
			return "[" + patient + 
						" am " + day +
						" von " + startTime +
						" bis " + endTime +						
						" an platz " + therapyPlace + "]";
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