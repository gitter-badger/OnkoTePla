using System;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Domain;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSrv
{
	public interface ICommandService
	{
		void TryAddNewAppointment(Action<bool> operationResultCallback, 
								  AggregateIdentifier location, Guid patientId, string description, Time startTime, 
								  Time endTime, Guid therapyPlaceId, Guid labelId, Guid appointmentId, ActionTag actionTag,
								  Action<string> errorCallback);

		
		void TryReplaceAppointment(Action<bool> operationResultCallback, 
								   AggregateIdentifier sourceLocation, AggregateIdentifier destinationLocation,
								   Guid patientId,
								   string originalDescription, string newDescription,
								   Date   originalDate,          Date newDate,
								   Time   originalStartTime,     Time newStartTime,
								   Time   originalEndTime,       Time newEndTime,
								   Guid   originalTherapyPlaceId,Guid newTherapyPlaceId,
								   Guid   originalLabelId,       Guid newLabelId,
								   Guid   originalAppointmendId,
								   ActionTag actionTag,
								   Action<string> errorCallback);
		
		void TryDeleteAppointment(Action<bool> operationResultCallback,
								  AggregateIdentifier location, Guid patientId,
								  Guid removedAppointmentId, string removedAppointmentDescription,
								  Time removedAppointmentStartTime, Time removedAppointmentEndTime,
								  Guid removedAppointmentTherapyPlaceId,Guid removedAppointmentLabelId,
								  ActionTag actionTag, Action<string> errorCallback);
	}
}