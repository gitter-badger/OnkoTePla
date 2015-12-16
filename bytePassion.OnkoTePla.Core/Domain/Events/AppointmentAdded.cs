using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Core.Eventsystem;
using System;


namespace bytePassion.OnkoTePla.Core.Domain.Events
{
    public class AppointmentAdded : DomainEvent
	{
		public AppointmentAdded(AggregateIdentifier aggregateId, uint aggregateVersion, 
								Guid userId, Tuple<Date, Time> timeStamp, ActionTag actionTag,
								CreateAppointmentData createAppointmentData)
			: base(aggregateId, aggregateVersion, userId, createAppointmentData.PatientId, timeStamp, actionTag)
		{
			CreateAppointmentData = createAppointmentData;			
		}

		public CreateAppointmentData CreateAppointmentData { get; }
	}
}
