using System;


namespace xIT.OnkoTePla.Client.Core.DataObjects
{
	public sealed class Appointment
	{
		private readonly Patient  _patient;
		private readonly DateTime _startTime;
		private readonly DateTime _endTime;

		public Appointment(Patient patient, DateTime startTime, DateTime endTime)
		{
			_patient   = patient;
			_startTime = startTime;
			_endTime   = endTime;
		}

		#region propertys (Patient / StartTime / EndTime)

		public Patient  Patient   { get { return _patient;   }}
		public DateTime StartTime { get { return _startTime; }}
		public DateTime EndTime   { get { return _endTime;   } }

		public TimeSpan Duration
		{
			get { return EndTime.Subtract(StartTime); }
		}

		#endregion

		#region operations

		public Appointment MoveStartTimeAndKeepDuration(DateTime newStartTime)
		{
			return new Appointment(Patient, newStartTime, newStartTime.Add(Duration));
		}

		#endregion
	}
}
