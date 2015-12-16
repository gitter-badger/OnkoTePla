using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.WpfUi.Global;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Core.Domain;
using System;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessageHandler
{
    public class ConfirmChangesMessageHandler : IViewModelMessageHandler<ConfirmChanges>
	{
		private readonly IViewModelCommunication viewModelCommunication;
		private readonly IGlobalState<AppointmentModifications> appointmentModificationsVariable;
	    private readonly IGlobalStateReadOnly<Guid> currentMedicalPracticeId; 
        
		public ConfirmChangesMessageHandler (IViewModelCommunication viewModelCommunication, 
                                             IGlobalState<AppointmentModifications> appointmentModificationsVariable,
                                             IGlobalStateReadOnly<Guid> currentMedicalPracticeId)
		{
		    this.viewModelCommunication = viewModelCommunication;
		    this.appointmentModificationsVariable = appointmentModificationsVariable;
		    this.currentMedicalPracticeId = currentMedicalPracticeId;
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

				viewModelCommunication.SendTo(
					Constants.AppointmentGridViewModelCollection,
					new AggregateIdentifier(appointmentModificationsVariable.Value.CurrentLocation.PlaceAndDate.Date,
                                            currentMedicalPracticeId.Value),
					new CreateNewAppointmentFromModificationsAndSendToCommandBus()
				);
			}
			else
			{
				viewModelCommunication.SendTo(
					Constants.AppointmentGridViewModelCollection,
					new AggregateIdentifier(appointmentModificationsVariable.Value.CurrentLocation.PlaceAndDate.Date,
                                            currentMedicalPracticeId.Value),
					new SendCurrentChangesToCommandBus()
				);
			}

			currentAppointmentModification.Dispose();
			appointmentModificationsVariable.Value = null;
		}
	}
}
