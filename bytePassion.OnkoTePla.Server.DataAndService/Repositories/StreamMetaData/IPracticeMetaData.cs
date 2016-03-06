using System;
using System.Collections.Generic;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.StreamMetaData
{
	public interface IPracticeMetaData
	{
		Guid MedicalPracticeId { get; }

		Date FirstAppointmentDate { get; }
		Date LastAppointmentDate  { get; }

		IDictionary<Guid, IList<Date>> AppointmentsForPatient    { get; }
		IDictionary<Date, ushort>      AppointmentExistenceIndex { get; }

		void AddEventToMetaData(DomainEvent domainEvent);
	}
}