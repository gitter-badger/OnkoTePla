using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSystem;
using bytePassion.OnkoTePla.Contracts.Domain;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.Commands
{
	public class DeleteAppointment : DomainCommand
	{
		public DeleteAppointment(AggregateIdentifier aggregateId, uint aggregateVersion, 
								 Guid userId, Guid patientId, ActionTag actionTag, 
								 Guid removedAppointmentId, string removedAppointmentDescription, 
								 Time removedAppointmentStartTime, Time removedAppointmentEndTime, 
								 Guid removedAppointmentTherapyPlaceId)
			: base(userId, patientId, actionTag)
		{			
			AggregateId = aggregateId;
			AggregateVersion = aggregateVersion;
			RemovedAppointmentId = removedAppointmentId;
			RemovedAppointmentDescription = removedAppointmentDescription;
			RemovedAppointmentStartTime = removedAppointmentStartTime;
			RemovedAppointmentEndTime = removedAppointmentEndTime;
			RemovedAppointmentTherapyPlaceId = removedAppointmentTherapyPlaceId;			
		}
		
		public AggregateIdentifier AggregateId         { get; }
		public uint                AggregateVersion    { get; }

		public Guid   RemovedAppointmentId             { get; }
		public string RemovedAppointmentDescription    { get; }
		public Time   RemovedAppointmentStartTime      { get; }
		public Time   RemovedAppointmentEndTime        { get; }
		public Guid   RemovedAppointmentTherapyPlaceId { get; }
	}
}
