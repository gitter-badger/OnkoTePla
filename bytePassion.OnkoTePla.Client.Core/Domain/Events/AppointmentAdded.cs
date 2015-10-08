using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Client.Core.Eventsystem;


namespace bytePassion.OnkoTePla.Client.Core.Domain.Events
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
