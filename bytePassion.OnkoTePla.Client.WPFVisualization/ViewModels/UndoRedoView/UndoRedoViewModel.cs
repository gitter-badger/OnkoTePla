using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.WpfUtils.Commands;
using bytePassion.OnkoTePla.Client.WPFVisualization.Global;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;


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
		private readonly IGlobalState<AppointmentModifications> appointmentModificationsVariable;

		private AppointmentModifications currentAppointmentModifications;
		private ButtonMode currentButtonMode;
				

		public UndoRedoViewModel(IViewModelCommunication viewModelCommunication)
		{
			this.viewModelCommunication = viewModelCommunication;

			currentButtonMode = ButtonMode.ViewMode;
			currentAppointmentModifications = null;

			Undo = new Command(UndoAction, UndoPossible);
			Redo = new Command(RedoAction, RedoPossible);

			appointmentModificationsVariable = viewModelCommunication.GetGlobalViewModelVariable<AppointmentModifications>(
				Constants.CurrentModifiedAppointmentVariable
			);
			appointmentModificationsVariable.StateChanged += OnAppointmentModificationsVariableChanged;
		}

		private bool RedoPossible()
		{
			switch (currentButtonMode)
			{
				case ButtonMode.EditsAvailable:  
				case ButtonMode.StartOfEditMode: return currentAppointmentModifications.RedoPossible;
				case ButtonMode.ViewMode:		 return false; // TODO !!
			}

			return false;
		}

		private bool UndoPossible()
		{
			switch (currentButtonMode)
			{
				case ButtonMode.EditsAvailable:  
				case ButtonMode.StartOfEditMode: return true;
				case ButtonMode.ViewMode:		 return false; // TODO !!
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

		private void RedoAction()
		{
			switch (currentButtonMode)
			{
				case ButtonMode.StartOfEditMode:
				case ButtonMode.EditsAvailable:
				{
					currentAppointmentModifications.Redo();
					break;
				}
			}
		}

		private void UndoAction()
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
			}
		}

		public ICommand Undo { get; }
		public ICommand Redo { get; }
	}

}
