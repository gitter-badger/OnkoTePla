using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic;
using bytePassion.OnkoTePla.Client.Core.Eventsystem.Base;


namespace bytePassion.OnkoTePla.Client.Core.Domain.Events
{
	public class AppointmentAdded : DomainEvent
	{

		private readonly CreateAppointmentData createAppointmentData;

		public AppointmentAdded(AggregateIdentifier aggregateId, uint aggregateVersion, 
								Guid userId, Tuple<Date, Time> timeStamp,
								CreateAppointmentData createAppointmentData)
			: base(aggregateId, aggregateVersion, userId, createAppointmentData.PatientId, timeStamp)
		{
			this.createAppointmentData = createAppointmentData;			
		}

		public CreateAppointmentData CreateAppointmentData
		{
			get { return createAppointmentData; }
		}
	}
}
