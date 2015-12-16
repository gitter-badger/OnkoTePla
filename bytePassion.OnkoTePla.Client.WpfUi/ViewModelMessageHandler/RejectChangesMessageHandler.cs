using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.WpfUi.Global;
using bytePassion.OnkoTePla.Client.WpfUi.UserNotificationService;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessageHandler
{
    public class RejectChangesMessageHandler : IViewModelMessageHandler<RejectChanges>
	{
		private readonly IViewModelCommunication viewModelCommunication;
		private readonly IGlobalState<AppointmentModifications> appointmentModificationsVariable;

		public RejectChangesMessageHandler (IViewModelCommunication viewModelCommunication, 
                                            IGlobalState<AppointmentModifications> appointmentModificationsVariable)
		{
		    this.viewModelCommunication = viewModelCommunication;
		    this.appointmentModificationsVariable = appointmentModificationsVariable;
		}

        public void Process(RejectChanges message)
		{
			ProcessMesageAsync();
		}

		private async void ProcessMesageAsync()
		{
			var currentMods = appointmentModificationsVariable.Value;
			var original = appointmentModificationsVariable.Value.OriginalAppointment;

			if (currentMods.BeginTime == original.StartTime &&
			    currentMods.EndTime == original.EndTime &&
			    currentMods.Description == original.Description &&
			    currentMods.CurrentLocation == currentMods.InitialLocation)
			{
				RejectChanges();
			}
			else
			{
				var dialog = new UserDialogBox("", "Wollen Sie alle Änderungen verwerfen?",
				                               MessageBoxButton.OKCancel, MessageBoxImage.Question);
				var result = await dialog.ShowMahAppsDialog();

				if (result == MessageDialogResult.Affirmative)
				{
					RejectChanges();
				}
			}
		}

		private void RejectChanges()
		{
			var isInitial = appointmentModificationsVariable.Value.IsInitialAdjustment;
            var originalAppointmentId = appointmentModificationsVariable.Value.OriginalAppointment.Id;

			appointmentModificationsVariable.Value.Dispose();
			appointmentModificationsVariable.Value = null;

			if (isInitial)
			{
				viewModelCommunication.SendTo(           //
					Constants.AppointmentViewModelCollection,      // do nothing but
					originalAppointmentId,				 // deleting the temporarly
					new Dispose()                        // created Appointment
				);                                       //
			}
			else
			{
				viewModelCommunication.SendTo(
					Constants.AppointmentViewModelCollection,
					originalAppointmentId,
					new RestoreOriginalValues()
				);
			}		
		}
	}
}
