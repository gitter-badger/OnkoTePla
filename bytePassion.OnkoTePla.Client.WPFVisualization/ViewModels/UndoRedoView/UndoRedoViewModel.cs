using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.OnkoTePla.Client.Core.Readmodels;
using bytePassion.OnkoTePla.Client.WPFVisualization.Global;
using bytePassion.OnkoTePla.Client.WPFVisualization.UserNotificationService;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;
using MahApps.Metro.Controls.Dialogs;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.UndoRedoView
{
	public class UndoRedoViewModel : IUndoRedoViewModel
	{
		private enum ButtonMode
		{
			EditsAvailable,
			StartOfEditMode,
			ViewMode
		}

		private readonly IViewModelCommunication viewModelCommunication;
		private readonly SessionAndUserSpecificEventHistory sessionAndUserSpecificEventHistory;		

		private AppointmentModifications currentAppointmentModifications;
		private ButtonMode currentButtonMode;
				

		public UndoRedoViewModel(IViewModelCommunication viewModelCommunication,
								 SessionAndUserSpecificEventHistory sessionAndUserSpecificEventHistory)
		{
			this.viewModelCommunication = viewModelCommunication;
			this.sessionAndUserSpecificEventHistory = sessionAndUserSpecificEventHistory;
			
			currentButtonMode = ButtonMode.ViewMode;
			currentAppointmentModifications = null;

			Undo = new Command(UndoAction, UndoPossible);
			Redo = new Command(RedoAction, RedoPossible);			

			var appointmentModificationsVariable = viewModelCommunication.GetGlobalViewModelVariable<AppointmentModifications>(
				Constants.CurrentModifiedAppointmentVariable
			);
			appointmentModificationsVariable.StateChanged += OnAppointmentModificationsVariableChanged;

			sessionAndUserSpecificEventHistory.PropertyChanged += OnSessionAndUserSpecificEventHistoryChanged;
		}		

		private bool RedoPossible()
		{
			switch (currentButtonMode)
			{
				case ButtonMode.EditsAvailable:  
				case ButtonMode.StartOfEditMode: return currentAppointmentModifications.RedoPossible;
				case ButtonMode.ViewMode:		 return sessionAndUserSpecificEventHistory.RedoPossible;
			}

			return false;
		}

		private bool UndoPossible()
		{
			switch (currentButtonMode)
			{
				case ButtonMode.EditsAvailable:  
				case ButtonMode.StartOfEditMode: return true;
				case ButtonMode.ViewMode:		 return sessionAndUserSpecificEventHistory.UndoPossible;
			}

			return false;
		}

		private void OnAppointmentModificationsVariableChanged(AppointmentModifications newAppointmentModifications)
		{
			if (newAppointmentModifications == null)
				currentButtonMode = ButtonMode.ViewMode;
			else
			{
				currentButtonMode = ButtonMode.StartOfEditMode;
				currentAppointmentModifications = newAppointmentModifications;
				currentAppointmentModifications.PropertyChanged += OnCurrentAppointmentModificationsChanged;
			}

			((Command)Undo).RaiseCanExecuteChanged();
			((Command)Redo).RaiseCanExecuteChanged();
		}

		private void OnSessionAndUserSpecificEventHistoryChanged (object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			switch (propertyChangedEventArgs.PropertyName)
			{			
				case nameof(SessionAndUserSpecificEventHistory.UndoPossible):
				{
					((Command)Undo).RaiseCanExecuteChanged();
					break;
				}
				case nameof(SessionAndUserSpecificEventHistory.RedoPossible):
				{
					((Command)Redo).RaiseCanExecuteChanged();
					break;
				}
			}
		}

		private void OnCurrentAppointmentModificationsChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			switch (propertyChangedEventArgs.PropertyName)
			{
				case nameof(AppointmentModifications.BeginTime):
				case nameof(AppointmentModifications.EndTime):
				case nameof(AppointmentModifications.CurrentLocation):
				case nameof(AppointmentModifications.Description):
				{
					currentButtonMode = currentAppointmentModifications.UndoPossible 
											? ButtonMode.EditsAvailable 
											: ButtonMode.StartOfEditMode;
					break;
				}
				case nameof(AppointmentModifications.UndoPossible):
				{
					((Command)Undo).RaiseCanExecuteChanged();
					break;
				}
				case nameof(AppointmentModifications.RedoPossible):
				{
					((Command)Redo).RaiseCanExecuteChanged();
					break;
				}
			}
		}

		private async void RedoAction()
		{
			switch (currentButtonMode)
			{
				case ButtonMode.StartOfEditMode:
				case ButtonMode.EditsAvailable:
			    {
			        currentAppointmentModifications.Redo();
                    break;
			    }
				case ButtonMode.ViewMode:
			    {                   
                    sessionAndUserSpecificEventHistory.Redo();                                           
                    break;
			    }
			}
		}

		private async void UndoAction()
		{
			switch (currentButtonMode)
			{
				case ButtonMode.EditsAvailable:
			    {
			        currentAppointmentModifications.Undo();
                    break;
			    }
				case ButtonMode.StartOfEditMode:
			    {
			        viewModelCommunication.Send(new RejectChanges());
                    break;
			    }
				case ButtonMode.ViewMode:
			    {
                    var dialog = new UserDialogBox("", 
                                                   sessionAndUserSpecificEventHistory.GetUndoActionMessage(),
                                                   MessageBoxButton.OKCancel, 
                                                   MessageBoxImage.Question);

                    var result = await dialog.ShowMahAppsDialog();

                    if (result == MessageDialogResult.Affirmative)
                    {
                        sessionAndUserSpecificEventHistory.Undo();
                    }                        
                    break;
			    }
			}
		}

		public ICommand Undo { get; }
		public ICommand Redo { get; }
	}
}
