using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Resources.UserNotificationService;
using MahApps.Metro.Controls.Dialogs;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.UndoRedoView
{
	internal class UndoRedoViewModel : ViewModel, 
                                       IUndoRedoViewModel
	{
		private enum ButtonMode
		{
			EditsAvailable,
			StartOfEditMode,
			ViewMode		
		}

		private readonly IViewModelCommunication viewModelCommunication;
		private readonly ISharedState<AppointmentModifications> appointmentModificationsVariable;
	    private readonly ISession session;		


		private AppointmentModifications currentAppointmentModifications;
		private ButtonMode currentButtonMode;
				

		public UndoRedoViewModel(IViewModelCommunication viewModelCommunication,
								 ISharedState<AppointmentModifications> appointmentModificationsVariable, 
								 ISession session)
		{
			this.viewModelCommunication = viewModelCommunication;
			this.appointmentModificationsVariable = appointmentModificationsVariable;
			this.session = session;			

			currentButtonMode = ButtonMode.ViewMode;
			currentAppointmentModifications = null;

			Undo = new Command(UndoAction, UndoPossible);
			Redo = new Command(RedoAction, RedoPossible);			

			
			appointmentModificationsVariable.StateChanged += OnAppointmentModificationsVariableChanged;
			
			session.UndoPossibleChanged += OnUndoPossibleChanged;
			session.RedoPossibleChanged += OnRedoPossibleChanged;			
		}

	    private void OnUndoPossibleChanged(bool isUndoPossible)
	    {
			((Command)Undo).RaiseCanExecuteChanged();
		}

	    private void OnRedoPossibleChanged(bool isRedoPossible)
	    {
			((Command)Redo).RaiseCanExecuteChanged();
		}

	    private bool RedoPossible()
		{
			switch (currentButtonMode)
			{
				case ButtonMode.EditsAvailable:  
				case ButtonMode.StartOfEditMode: return currentAppointmentModifications.RedoPossible;
				case ButtonMode.ViewMode:		 return session.RedoPossible();				
			}

			return false;
		}

		private bool UndoPossible()
		{
			switch (currentButtonMode)
			{
				case ButtonMode.EditsAvailable:  
				case ButtonMode.StartOfEditMode: return true;
				case ButtonMode.ViewMode:		 return session.UndoPossible();				
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
					var dialog = new UserDialogBox("",
												   session.GetCurrentRedoActionMsg(),
												   MessageBoxButton.OKCancel);

					var result = await dialog.ShowMahAppsDialog();

					if (result == MessageDialogResult.Affirmative)
					{
						session.Redo();
					}
					
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
												   session.GetCurrentUndoActionMsg(),
												   MessageBoxButton.OKCancel);

					var result = await dialog.ShowMahAppsDialog();

					if (result == MessageDialogResult.Affirmative)
					{
						session.Undo();
					}
					break;
				}
			}
		}

		public ICommand Undo { get; }
		public ICommand Redo { get; }

		protected override void CleanUp()
		{
			appointmentModificationsVariable.StateChanged -= OnAppointmentModificationsVariableChanged;

			session.UndoPossibleChanged -= OnUndoPossibleChanged;
			session.RedoPossibleChanged -= OnRedoPossibleChanged;
		}

		public override event PropertyChangedEventHandler PropertyChanged;
	}
}
