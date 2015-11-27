using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.WPFVisualization.UserNotificationService;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;
using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessageHandler
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
					AppointmentViewModelCollection,      // do nothing but
					originalAppointmentId,				 // deleting the temporarly
					new Dispose()                        // created Appointment
				);                                       //
			}
			else
			{
				viewModelCommunication.SendTo(
					AppointmentViewModelCollection,
					originalAppointmentId,
					new RestoreOriginalValues()
				);
			}		
		}
	}
}
