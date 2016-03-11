using System;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.Commands;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSrv;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.UndoRedo.Helper;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;
using bytePassion.OnkoTePla.Contracts.Patients;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.UndoRedo.UserActions
{
	public class AddedAction : IUserAction
	{
		private readonly ICommandService commandService;		
		private readonly AddAppointment addAppointmentCommand;
		private readonly Patient patient;

		public AddedAction(ICommandService commandService,						   
						   AddAppointment addAppointmentCommand,
						   Patient patient)
		{
			this.commandService = commandService;			
			this.addAppointmentCommand = addAppointmentCommand;
			this.patient = patient;
		}

		public void Undo(Action<bool> operationResultCallback, Action<string> errorCallback)
		{
			commandService.TryDeleteAppointment(operationResultCallback,
												addAppointmentCommand.AggregateId,
												patient.Id,
												addAppointmentCommand.AppointmentId,
												addAppointmentCommand.Description,
												addAppointmentCommand.StartTime,
												addAppointmentCommand.EndTime,
												addAppointmentCommand.TherapyPlaceId,
												ActionTag.UndoAction, 
												errorCallback);
		}

		public void Redo(Action<bool> operationResultCallback, Action<string> errorCallback)
		{
			commandService.TryAddNewAppointment(operationResultCallback,
												addAppointmentCommand.AggregateId,
												addAppointmentCommand.PatientId,
												addAppointmentCommand.Description,
												addAppointmentCommand.StartTime,
												addAppointmentCommand.EndTime,
												addAppointmentCommand.TherapyPlaceId,
												addAppointmentCommand.AppointmentId,
												ActionTag.RedoAction, 
												errorCallback);
		}

		public string GetUndoMsg()
		{
			return UndoStringGenerator.ForAddedEvent(patient, 
													 addAppointmentCommand.AggregateId.Date,
													 addAppointmentCommand.StartTime, 
													 addAppointmentCommand.EndTime);
		}

		public string GetRedoMsg ()
		{
			return RedoStringGenerator.ForAddedEvent(patient,
													 addAppointmentCommand.AggregateId.Date,
													 addAppointmentCommand.StartTime,
													 addAppointmentCommand.EndTime);
		}
	}
}