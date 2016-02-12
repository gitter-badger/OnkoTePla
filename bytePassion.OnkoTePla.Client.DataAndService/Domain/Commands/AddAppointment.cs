using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSystem;
using bytePassion.OnkoTePla.Contracts.Domain;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.Commands
{
	public class AddAppointment : DomainCommand
	{
		public AddAppointment(AggregateIdentifier aggregateId, uint aggregateVersion, 
							  Guid userId, ActionTag actionTag,
							  Guid patientId, string description, 
							  Time startTime, Time endTime, 
							  Guid therapyPlaceId, Guid? appointmentId = null)
			: base(userId, patientId, actionTag)
		{
			var newAppointmentId = appointmentId ?? Guid.NewGuid();

			Description = description;
			StartTime = startTime;
			EndTime = endTime;
			TherapyPlaceId = therapyPlaceId;
			AppointmentId = newAppointmentId;			
			AggregateId = aggregateId;
			AggregateVersion = aggregateVersion;
		}
				
		public string                Description      { get; }		
		public Time                  StartTime        { get; }
		public Time                  EndTime          { get; }
		public Guid                  TherapyPlaceId   { get; }
		public Guid                  AppointmentId    { get; }
		public AggregateIdentifier   AggregateId      { get; }
		public uint                  AggregateVersion { get; }
	}
}
