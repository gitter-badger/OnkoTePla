using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;

namespace bytePassion.OnkoTePla.Contracts.Appointments
{
	public class AppointmentTransferData
	{
		public AppointmentTransferData(Guid patientId, string description, Date day, 
									   Time startTime, Time endTime, Guid therapyPlaceId, Guid id)
		{
			PatientId = patientId;
			Description = description;
			Day = day;
			StartTime = startTime;
			EndTime = endTime;
			TherapyPlaceId = therapyPlaceId;
			Id = id;
		}

		public AppointmentTransferData(Appointment appointment)
			: this (appointment.Patient.Id, appointment.Description, 
					appointment.Day,        appointment.StartTime,  
					appointment.EndTime,    appointment.TherapyPlace.Id, 
					appointment.Id)
		{			
		}

		public Guid   PatientId      { get; }
		public string Description    { get; }
		public Date   Day            { get; }
		public Time   StartTime      { get; }
		public Time   EndTime        { get; }
		public Guid   TherapyPlaceId { get; }
		public Guid   Id             { get; }

		public Appointment GetAppointment(Func<Guid, Patient> patientRepository,
										  Func<Guid, TherapyPlace> therapyPlaceRepository)
		{
			return new Appointment(patientRepository(PatientId),
								   Description,
								   therapyPlaceRepository(TherapyPlaceId),
								   Day,
								   StartTime,
								   EndTime,
								   Id);
		}
	}
}
