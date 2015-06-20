using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Contracts.Appointments
{
	public sealed class Appointment
	{
		private readonly Patient      patient;

		private string       description;
		private Date         day;
		private Time         startTime;
		private Time         endTime;
		private TherapyPlace therapyPlace;
	    private Room		 room;

	    

	    public Appointment (Patient patient, TherapyPlace therapyPlace, Room room,
						    Date day, Time startTime, Time endTime)
		{
			this.patient      = patient;
			this.therapyPlace = therapyPlace;
			this.room         = room;
		    this.day          = day;
			this.startTime    = startTime;
			this.endTime      = endTime;				        
		}

		#region propertys (Patient / StartTime / EndTime)

		public Patient Patient { get { return patient; } }

		public string Description
		{
			get { return description;  }
			set { description = value; }
		}

		public Date Day       { get { return day;       }}
		public Time StartTime { get { return startTime; }}
		public Time EndTime   { get { return endTime;   }}

		public TherapyPlace TherapyPlace { get { return therapyPlace; } }

//		public Duration Duration
//		{
//			get { return new Duration(); EndTime.Subtract(StartTime); }
//		}

		public Room Room { get { return room; }}

		#endregion

		#region operations

//		public Appointment MoveToAnotherTherapyChair (TherapyPlace newTherapyPlace)
//		{
//			return new Appointment(Patient, newTherapyPlace, Room, StartTime, EndTime);
//		}

//		public Appointment MoveStartTimeAndKeepDuration (DateTime newStartTime)
//		{
//			return new Appointment(Patient, TherapyPlace, Room,
//								  newStartTime, newStartTime.Add(Duration));
//		}

		// TODO: MoveStartTimeAndKeepEndTime
		// TODO: MoveEndTimeAndKeepStartTime

		#endregion

		#region ToString / HashCode / Equals

		public override string ToString()
		{
			return "[" + patient + 
						" am " + day +
						" von " + startTime +
						" bis " + endTime +
						" in raum " + room +
						" an platz " + therapyPlace + "]";
		}

		public override bool Equals(object obj)
		{
			return this.Equals(obj, (appointment1, appointment2) => appointment1.Patient.Equals(appointment2.Patient) &&
																    appointment1.Description.Equals(appointment2.Description) &&
																	appointment1.Day.Equals(appointment2.Day) &&
																	appointment1.StartTime.Equals(appointment2.StartTime) &&
																	appointment1.EndTime.Equals(appointment2.EndTime) &&
																	appointment1.TherapyPlace.Equals(appointment2.TherapyPlace) &&
																	appointment1.Room.Equals(appointment2.Room));
		}

		public override int GetHashCode()
		{
			return Patient.GetHashCode() ^
			       Description.GetHashCode() ^
			       Day.GetHashCode() ^
			       StartTime.GetHashCode() ^
			       EndTime.GetHashCode() ^
			       TherapyPlace.GetHashCode() ^
			       Room.GetHashCode();
		}

		#endregion
	}
}