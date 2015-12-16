using bytePassion.Lib.TimeLib;
using System;


namespace bytePassion.OnkoTePla.Core.Domain.AppointmentLogic
{
    public struct CreateAppointmentData
	{
		public CreateAppointmentData(Guid patientId, string description, 
									 Time startTime, Time endTime, Date day, 
									 Guid therapyPlaceId, Guid appointmendId)
		{
			PatientId = patientId;
			Description = description;
			StartTime = startTime;
			EndTime = endTime;
			Day = day;
			TherapyPlaceId = therapyPlaceId;
			AppointmentId = appointmendId;
		}

		public Guid   PatientId      { get; }
		public string Description    { get; }
		public Date   Day            { get; }
		public Time   StartTime      { get; }
		public Time   EndTime        { get; }
		public Guid   TherapyPlaceId { get; }
		public Guid   AppointmentId  { get; }
	}
}
