using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfUtils.Commands;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AddAppointmentDialog;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.ChangeConfirmationView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateDisplay;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.GridContainer;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MedicalPracticeSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelector;

using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OverviewPage
{
	public class OverviewPageViewModel : IOverviewPageViewModel
	{
		private bool changeConfirmationVisible;
		private bool addAppointmentPossible;

		public OverviewPageViewModel(IDateDisplayViewModel dateDisplayViewModel,
                                     IMedicalPracticeSelectorViewModel medicalPracticeSelectorViewModel, 
									 IRoomFilterViewModel roomFilterViewModel, 
									 IDateSelectorViewModel dateSelectorViewModel, 
									 IGridContainerViewModel gridContainerViewModel, 
									 IChangeConfirmationViewModel changeConfirmationViewModel,
									 IViewModelCommunication viewModelCommunication)
		{
			DateDisplayViewModel = dateDisplayViewModel;
			MedicalPracticeSelectorViewModel = medicalPracticeSelectorViewModel;
			RoomFilterViewModel = roomFilterViewModel;
			DateSelectorViewModel = dateSelectorViewModel;
			GridContainerViewModel = gridContainerViewModel;
			ChangeConfirmationViewModel = changeConfirmationViewModel;			

			ChangeConfirmationVisible = false;
			AddAppointmentPossible = true;

			var currentModifiedAppointmentVariable = viewModelCommunication.GetGlobalViewModelVariable<AppointmentModifications>(
				CurrentModifiedAppointmentVariable	
			);
			
			currentModifiedAppointmentVariable.StateChanged += OnCurrentModifiedAppointmentVariableChanged;

			ShowAddAppointmentDialog = new Command(() =>
			{
				var dialogWindow = new Views.AddAppointmentDialog
				{
					Owner = Application.Current.MainWindow,
					DataContext = new AddAppointmentDialogViewModel()
				};

				dialogWindow.ShowDialog();
			});
		}

		private void OnCurrentModifiedAppointmentVariableChanged(AppointmentModifications appointment)
		{
			ChangeConfirmationVisible = appointment != null;
			AddAppointmentPossible = appointment == null;
		}

		public IDateDisplayViewModel             DateDisplayViewModel             { get; }
		public IMedicalPracticeSelectorViewModel MedicalPracticeSelectorViewModel { get; }
		public IRoomFilterViewModel              RoomFilterViewModel              { get; }
		public IDateSelectorViewModel            DateSelectorViewModel            { get; }
		public IGridContainerViewModel           GridContainerViewModel           { get; }
		public IChangeConfirmationViewModel      ChangeConfirmationViewModel      { get; }

		public ICommand ShowAddAppointmentDialog { get; }

		public bool ChangeConfirmationVisible
		{
			get { return changeConfirmationVisible; }
			private set { PropertyChanged.ChangeAndNotify(this, ref changeConfirmationVisible, value); }
		}

		public bool AddAppointmentPossible
		{
			get { return addAppointmentPossible; }
			private set { PropertyChanged.ChangeAndNotify(this, ref addAppointmentPossible, value); }
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
