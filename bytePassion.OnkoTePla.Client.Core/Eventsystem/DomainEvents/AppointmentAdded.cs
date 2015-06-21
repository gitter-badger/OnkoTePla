using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents.Eventbase;


namespace bytePassion.OnkoTePla.Client.Core.Eventsystem.DomainEvents
{
	public class AppointmentAdded : DomainEvent
	{

		private readonly CreateAppointmentData createAppointmentData;

		public AppointmentAdded(AggregateIdentifier aggregateId, uint aggregateVersion, 
								Guid userId, Tuple<Date, Time> timeStamp,
								CreateAppointmentData createAppointmentData)
			: base(aggregateId, aggregateVersion, userId, timeStamp)
		{
			this.createAppointmentData = createAppointmentData;			
		}

		public CreateAppointmentData CreateAppointmentData
		{
			get { return createAppointmentData; }
		}
	}
}
