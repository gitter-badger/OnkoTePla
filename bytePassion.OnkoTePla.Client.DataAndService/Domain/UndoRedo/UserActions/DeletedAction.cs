using System;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.Commands;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSrv;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.UndoRedo.Helper;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;
using bytePassion.OnkoTePla.Contracts.Patients;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.UndoRedo.UserActions
{
	public class DeletedAction : IUserAction
	{
		private readonly ICommandService commandService;
		private readonly DeleteAppointment deleteAppointmentCommand;
		private readonly Patient patient;

		public DeletedAction (ICommandService commandService,
							  DeleteAppointment deleteAppointmentCommand,
							  Patient patient)
		{
			this.commandService = commandService;
			this.deleteAppointmentCommand = deleteAppointmentCommand;
			this.patient = patient;
		}
		
		public void Undo(Action<bool> operationResultCallback, Action<string> errorCallback)
		{
			commandService.TryAddNewAppointment(operationResultCallback,
												deleteAppointmentCommand.AggregateId,
												deleteAppointmentCommand.PatientId,
												deleteAppointmentCommand.RemovedAppointmentDescription,
												deleteAppointmentCommand.RemovedAppointmentStartTime,
												deleteAppointmentCommand.RemovedAppointmentEndTime,
												deleteAppointmentCommand.RemovedAppointmentTherapyPlaceId,
												deleteAppointmentCommand.RemovedAppointmentLabelId,
												deleteAppointmentCommand.RemovedAppointmentId,
												ActionTag.UndoAction,
												errorCallback);
		}

		public void Redo(Action<bool> operationResultCallback, Action<string> errorCallback)
		{
			commandService.TryDeleteAppointment(operationResultCallback,
												deleteAppointmentCommand.AggregateId,
												deleteAppointmentCommand.PatientId,
												deleteAppointmentCommand.RemovedAppointmentId,
												deleteAppointmentCommand.RemovedAppointmentDescription,
												deleteAppointmentCommand.RemovedAppointmentStartTime,
												deleteAppointmentCommand.RemovedAppointmentEndTime,
												deleteAppointmentCommand.RemovedAppointmentTherapyPlaceId,
												deleteAppointmentCommand.RemovedAppointmentLabelId,												
												ActionTag.RedoAction,
												errorCallback);
		}
		
		public string GetUndoMsg ()
		{
			return UndoStringGenerator.ForDeletedEvent(patient, 
													   deleteAppointmentCommand.AggregateId.Date,
													   deleteAppointmentCommand.RemovedAppointmentStartTime,
													   deleteAppointmentCommand.RemovedAppointmentEndTime);
		}

		public string GetRedoMsg ()
		{
			return RedoStringGenerator.ForDeletedEvent(patient,
													   deleteAppointmentCommand.AggregateId.Date,
													   deleteAppointmentCommand.RemovedAppointmentStartTime,
													   deleteAppointmentCommand.RemovedAppointmentEndTime);
		}
	}
}