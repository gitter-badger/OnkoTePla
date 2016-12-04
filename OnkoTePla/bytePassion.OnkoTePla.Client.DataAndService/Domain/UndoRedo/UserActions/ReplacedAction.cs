using System;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.Commands;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSrv;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.UndoRedo.Helper;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.UndoRedo.UserActions
{
	public class ReplacedAction : IUserAction
	{
		private readonly ICommandService commandService;
		private readonly ReplaceAppointment replaceAppointmentCommand;
		private readonly Patient patient;
		private readonly TherapyPlace originalTherapyPlace;
		private readonly TherapyPlace newTherapyPlace;

		public ReplacedAction(ICommandService commandService,
							  ReplaceAppointment replaceAppointmentCommand,
							  Patient patient,
							  TherapyPlace originalTherapyPlace,
							  TherapyPlace newTherapyPlace)
		{
			this.commandService = commandService;
			this.replaceAppointmentCommand = replaceAppointmentCommand;
			this.patient = patient;
			this.originalTherapyPlace = originalTherapyPlace;
			this.newTherapyPlace = newTherapyPlace;
		}

		public void Undo(Action<bool> operationResultCallback, Action<string> errorCallback)
		{
			commandService.TryReplaceAppointment(operationResultCallback,
												 replaceAppointmentCommand.DestinationAggregateId,
												 replaceAppointmentCommand.SourceAggregateId,
												 replaceAppointmentCommand.PatientId,
												 replaceAppointmentCommand.NewDescription,
												 replaceAppointmentCommand.OriginalDescription,
												 replaceAppointmentCommand.NewDate,
												 replaceAppointmentCommand.OriginalDate,
												 replaceAppointmentCommand.NewStartTime,
												 replaceAppointmentCommand.OriginalStartTime,
												 replaceAppointmentCommand.NewEndTime,
												 replaceAppointmentCommand.OriginalEndTime,
												 replaceAppointmentCommand.NewTherapyPlaceId,
												 replaceAppointmentCommand.OriginalTherapyPlaceId,
												 replaceAppointmentCommand.NewLabelId,
												 replaceAppointmentCommand.OriginalLabelId,
												 replaceAppointmentCommand.OriginalAppointmendId,
												 ActionTag.UndoAction, 
												 errorCallback);
		}

		public void Redo(Action<bool> operationResultCallback, Action<string> errorCallback)
		{
			commandService.TryReplaceAppointment(operationResultCallback,
												 replaceAppointmentCommand.SourceAggregateId,
												 replaceAppointmentCommand.DestinationAggregateId,
												 replaceAppointmentCommand.PatientId,
												 replaceAppointmentCommand.OriginalDescription,
												 replaceAppointmentCommand.NewDescription,
												 replaceAppointmentCommand.OriginalDate,
												 replaceAppointmentCommand.NewDate,
												 replaceAppointmentCommand.OriginalStartTime,
												 replaceAppointmentCommand.NewStartTime,
												 replaceAppointmentCommand.OriginalEndTime,
												 replaceAppointmentCommand.NewEndTime,
												 replaceAppointmentCommand.OriginalTherapyPlaceId,
												 replaceAppointmentCommand.NewTherapyPlaceId,
												 replaceAppointmentCommand.OriginalLabelId,
												 replaceAppointmentCommand.NewLabelId,
												 replaceAppointmentCommand.OriginalAppointmendId,
												 ActionTag.RedoAction, 
												 errorCallback);
		}

		public string GetUndoMsg ()
		{
			if (replaceAppointmentCommand.SourceAggregateId.Date == replaceAppointmentCommand.DestinationAggregateId.Date)
			{
				return UndoStringGenerator.ForReplacedEvent(patient,
															replaceAppointmentCommand.NewDate,
															replaceAppointmentCommand.NewStartTime,
															replaceAppointmentCommand.NewEndTime,
															newTherapyPlace,
															replaceAppointmentCommand.OriginalStartTime,
															replaceAppointmentCommand.OriginalEndTime,
															originalTherapyPlace);
			}
			else
			{
				return UndoStringGenerator.ForDividedReplacedEvent(patient,
															       replaceAppointmentCommand.NewDate,
																   replaceAppointmentCommand.OriginalDate,
															       replaceAppointmentCommand.NewStartTime,
																   replaceAppointmentCommand.OriginalStartTime,
															       replaceAppointmentCommand.NewEndTime,
																   replaceAppointmentCommand.OriginalEndTime,
																   newTherapyPlace,															       															       
															       originalTherapyPlace);
			}
					
		}
		
		public string GetRedoMsg ()
		{
			if (replaceAppointmentCommand.SourceAggregateId.Date == replaceAppointmentCommand.DestinationAggregateId.Date)
			{
				return RedoStringGenerator.ForReplacedEvent(patient,
															replaceAppointmentCommand.NewDate,
															replaceAppointmentCommand.NewStartTime,
															replaceAppointmentCommand.NewEndTime,
															newTherapyPlace,
															replaceAppointmentCommand.OriginalStartTime,
															replaceAppointmentCommand.OriginalEndTime,
															originalTherapyPlace);
			}
			else
			{
				return RedoStringGenerator.ForDividedReplacedEvent(patient,
																   replaceAppointmentCommand.NewDate,
																   replaceAppointmentCommand.OriginalDate,
																   replaceAppointmentCommand.NewStartTime,
																   replaceAppointmentCommand.OriginalStartTime,
																   replaceAppointmentCommand.NewEndTime,
																   replaceAppointmentCommand.OriginalEndTime,
																   newTherapyPlace,
																   originalTherapyPlace);
			}
		}
	}
}