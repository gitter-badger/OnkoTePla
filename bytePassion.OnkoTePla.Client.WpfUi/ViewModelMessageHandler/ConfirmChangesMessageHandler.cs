using System;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.CommandSrv;
using bytePassion.OnkoTePla.Client.WpfUi.Global;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessageHandler
{
	internal class ConfirmChangesMessageHandler : IViewModelMessageHandler<ConfirmChanges>
	{
		private readonly IViewModelCommunication viewModelCommunication;
		private readonly ICommandService commandService;
		private readonly ISharedState<AppointmentModifications> appointmentModificationsVariable;	    
        
		public ConfirmChangesMessageHandler (IViewModelCommunication viewModelCommunication, 
											 ICommandService commandService,
                                             ISharedState<AppointmentModifications> appointmentModificationsVariable)
		{
		    this.viewModelCommunication = viewModelCommunication;
			this.commandService = commandService;
			this.appointmentModificationsVariable = appointmentModificationsVariable;		    
		}

	    public void Process(ConfirmChanges message)
		{			
			var currentAppointmentModification = appointmentModificationsVariable.Value;

			if (currentAppointmentModification.IsInitialAdjustment)
			{
				viewModelCommunication.SendTo(
					Constants.AppointmentViewModelCollection,
					currentAppointmentModification.OriginalAppointment.Id,
					new Dispose()						
				);
				
				commandService.TryAddNewAppointment(currentAppointmentModification.CurrentLocation.PlaceAndDate,
													currentAppointmentModification.OriginalAppointment.Patient.Id,
													currentAppointmentModification.Description,
													currentAppointmentModification.BeginTime,
													currentAppointmentModification.EndTime,
													currentAppointmentModification.CurrentLocation.TherapyPlaceId,
													Guid.NewGuid(),
													ActionTag.RegularAction,
													errorMsg =>
													{
														viewModelCommunication.Send(new ShowNotification($"anlegen des Termins nicht möglich: {errorMsg}", 5));	
													});
			}
			else
			{
				if (appointmentModificationsVariable.Value == null)
					return;

				var originalAppointment = currentAppointmentModification.OriginalAppointment;

				if (originalAppointment.Description     == currentAppointmentModification.Description &&
					originalAppointment.StartTime       == currentAppointmentModification.BeginTime &&
					originalAppointment.EndTime         == currentAppointmentModification.EndTime &&
					originalAppointment.Day             == currentAppointmentModification.CurrentLocation.PlaceAndDate.Date &&
					originalAppointment.TherapyPlace.Id == currentAppointmentModification.CurrentLocation.TherapyPlaceId)
				{
					return; // no changes to report
				}

				commandService.TryReplaceAppointment(currentAppointmentModification.InitialLocation.PlaceAndDate, 
													 currentAppointmentModification.CurrentLocation.PlaceAndDate,
													 currentAppointmentModification.OriginalAppointment.Patient.Id,
													 currentAppointmentModification.Description,
													 currentAppointmentModification.CurrentLocation.PlaceAndDate.Date,
													 currentAppointmentModification.BeginTime,
													 currentAppointmentModification.EndTime,
													 currentAppointmentModification.CurrentLocation.TherapyPlaceId,
													 currentAppointmentModification.OriginalAppointment.Id,
													 currentAppointmentModification.OriginalAppointment.Day,
													 ActionTag.RegularAction, 
													 errorMsg =>
													 {
														 viewModelCommunication.Send(new ShowNotification($"veränderung des Termins nicht möglich: {errorMsg}", 5));
														 
														 viewModelCommunication.SendTo(
															Constants.AppointmentViewModelCollection,
															originalAppointment.Id,
															new RestoreOriginalValues()	 
														 );
													 });

			}

			currentAppointmentModification.Dispose();
			appointmentModificationsVariable.Value = null;
		}
	}
}
