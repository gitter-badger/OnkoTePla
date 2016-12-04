using System;
using System.Collections.Generic;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Domain.Events;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.StreamMetaData
{
	public class PracticeMetaData : IPracticeMetaData
	{
		public PracticeMetaData(Guid medicalPracticeId,
								Date firstAppointmentDate, Date lastAppointmentDate,
								IDictionary<Guid, IList<Date>> appointmentsForPatient,
								IDictionary<Date, ushort> appointmentExistenceIndex)
		{
			FirstAppointmentDate = firstAppointmentDate;
			LastAppointmentDate = lastAppointmentDate;
			AppointmentsForPatient = appointmentsForPatient;
			AppointmentExistenceIndex = appointmentExistenceIndex;
			MedicalPracticeId = medicalPracticeId;
		}

		public PracticeMetaData(Guid medicalPracticeId)
			: this(medicalPracticeId, Date.Dummy, Date.Dummy,
				   new Dictionary<Guid, IList<Date>>(), 
				   new Dictionary<Date, ushort>())
		{			
		}

		public Guid MedicalPracticeId { get; }

		public Date FirstAppointmentDate { get; private set; }
        public Date LastAppointmentDate  { get; private set; }

        public IDictionary<Guid, IList<Date>> AppointmentsForPatient   { get; } 
        public IDictionary<Date, ushort>     AppointmentExistenceIndex { get; }
		
		public void AddEventToMetaData(DomainEvent domainEvent)
		{
			UpdateFirstAndLastDate(domainEvent.AggregateId.Date);
			UpdateAppointmentsForPatient(domainEvent.PatientId, domainEvent.AggregateId.Date);
			UpdateAppointmentExistanceIndex(domainEvent);
		}

		private void UpdateFirstAndLastDate(Date newDate)
		{			
			if (FirstAppointmentDate == Date.Dummy)
			{
				FirstAppointmentDate = newDate;
				LastAppointmentDate  = newDate;
			}
			else
			{
				if (newDate < FirstAppointmentDate)
					FirstAppointmentDate = newDate;

				if (newDate > LastAppointmentDate)
					LastAppointmentDate = newDate;
			}
		}

		private void UpdateAppointmentsForPatient(Guid patientId, Date newDate)
		{
			if (!AppointmentsForPatient.ContainsKey(patientId))
				AppointmentsForPatient.Add(patientId, new List<Date>());

			if (!AppointmentsForPatient[patientId].Contains(newDate))
				AppointmentsForPatient[patientId].Add(newDate);
		}

		private void UpdateAppointmentExistanceIndex(DomainEvent domainEvent)
		{
			var newDate = domainEvent.AggregateId.Date;

			if (!AppointmentExistenceIndex.ContainsKey(newDate))
				AppointmentExistenceIndex.Add(newDate, 0);

			if (domainEvent is AppointmentAdded)			
				AppointmentExistenceIndex[newDate]++;			

			if (domainEvent is AppointmentDeleted)
				AppointmentExistenceIndex[newDate]--;
		}
    }   
}