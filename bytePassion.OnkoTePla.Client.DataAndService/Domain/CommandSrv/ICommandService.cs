using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Domain;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSrv
{
	public interface ICommandService
	{
		void TryAddNewAppointment(AggregateIdentifier location, Guid patientId, string description, Time startTime, 
								  Time endTime, Guid therapyPlaceId, Guid appointmentId, ActionTag actionTag,
								  Action<string> errorCallback);

		
		void TryReplaceAppointment(AggregateIdentifier sourceLocation, AggregateIdentifier destinationLocation,
								   Guid patientId, string newDescription, Date newDate, Time newBeginTime,
								   Time newEndTime, Guid newTherapyplaceId, Guid originalAppointmentId, 
								   Date originalDate, ActionTag actionTag,
								   Action<string> errorCallback);
		
		void TryDeleteAppointment(AggregateIdentifier location, Guid appointmentId, Guid patientId, 
								  ActionTag actionTag, Action<string> errorCallback);
	}
}