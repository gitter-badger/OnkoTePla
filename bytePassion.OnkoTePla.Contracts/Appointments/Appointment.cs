using System;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Contracts.Appointments
{
	public sealed class Appointment
	{
		private readonly Patient      patient;

		private string       _description;
		private DateTime     startTime;
		private DateTime     endTime;
		private TherapyPlace therapyPlace;
	    private readonly Room _room;

	    public Room Room
	    {
	        get { return _room; }
	    }

	    public Appointment (Patient patient, TherapyPlace therapyPlace, Room room,
						   DateTime startTime, DateTime endTime)
		{
			this.patient      = patient;
			this.startTime    = startTime;
			this.endTime      = endTime;
			this.therapyPlace = therapyPlace;
	        _room = room;
		}

		#region propertys (Patient / StartTime / EndTime)

		public Patient Patient { get { return patient; } }

		public string Description
		{
			get
			{
				return _description;
			}
			set { _description = value; }
		}

		public DateTime StartTime { get { return startTime; } }

		public DateTime EndTime { get { return endTime; } }

		public TherapyPlace TherapyPlace { get { return therapyPlace; } }

		public TimeSpan Duration
		{
			get { return EndTime.Subtract(StartTime); }
		}

		#endregion

		#region operations

		public Appointment MoveToAnotherTherapyChair (TherapyPlace newTherapyPlace)
		{
			return new Appointment(Patient, newTherapyPlace, Room, StartTime, EndTime);
		}

		public Appointment MoveStartTimeAndKeepDuration (DateTime newStartTime)
		{
			return new Appointment(Patient, TherapyPlace, Room,
								  newStartTime, newStartTime.Add(Duration));
		}

		// TODO: MoveStartTimeAndKeepEndTime
		// TODO: MoveEndTimeAndKeepStartTime

		#endregion
	}
}