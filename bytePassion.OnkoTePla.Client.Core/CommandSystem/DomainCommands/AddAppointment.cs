using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.CommandSystem.DomainCommands.CommandBase;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Domain.AppointmentLogic;


namespace bytePassion.OnkoTePla.Client.Core.CommandSystem.DomainCommands
{
	public class AddAppointment : DomainCommand
	{
		private readonly CreateAppointmentData createAppointmentData;		

		public AddAppointment(AggregateIdentifier aggregateId, uint aggregateVersion, Guid userId, 
							  Guid patientId, string description, 
							  Time startTime, Time endTime, 
							  Guid therapyPlaceId)
			: base(aggregateId, aggregateVersion, userId)
		{
			createAppointmentData = new CreateAppointmentData(patientId, description, 
															  startTime, endTime, AggregateId.Date, 
															  therapyPlaceId, Guid.NewGuid());
		}

		public CreateAppointmentData CreateAppointmentData
		{
			get { return createAppointmentData; }
		}	
	}
}
