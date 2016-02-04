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
    internal class ConfirmChangesMessageHandler : IViewModelMessageHandler<ConfirmChanges>
	{
		private readonly IViewModelCommunication viewModelCommunication;
		private readonly ISharedState<AppointmentModifications> appointmentModificationsVariable;
	    private readonly ISharedStateReadOnly<Guid> currentMedicalPracticeId; 
        
		public ConfirmChangesMessageHandler (IViewModelCommunication viewModelCommunication, 
                                             ISharedState<AppointmentModifications> appointmentModificationsVariable,
                                             ISharedStateReadOnly<Guid> currentMedicalPracticeId)
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
