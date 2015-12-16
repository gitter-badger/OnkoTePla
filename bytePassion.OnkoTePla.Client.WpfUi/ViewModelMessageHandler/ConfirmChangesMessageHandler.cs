using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;
using System;
using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessageHandler
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
					AppointmentViewModelCollection,
					currentAppointmentModification.OriginalAppointment.Id,
					new Dispose()						
				);

				viewModelCommunication.SendTo(
					AppointmentGridViewModelCollection,
					new AggregateIdentifier(appointmentModificationsVariable.Value.CurrentLocation.PlaceAndDate.Date,
                                            currentMedicalPracticeId.Value),
					new CreateNewAppointmentFromModificationsAndSendToCommandBus()
				);
			}
			else
			{
				viewModelCommunication.SendTo(
					AppointmentGridViewModelCollection,
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
