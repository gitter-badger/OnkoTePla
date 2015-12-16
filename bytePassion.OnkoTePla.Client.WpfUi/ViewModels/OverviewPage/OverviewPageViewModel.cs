using bytePassion.Lib.Communication.State;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.Utils;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.ChangeConfirmationView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateDisplay;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.GridContainer;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MedicalPracticeSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.UndoRedoView;
using System.ComponentModel;
using System.Windows.Input;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OverviewPage
{
    public class OverviewPageViewModel : ViewModel, 
                                         IOverviewPageViewModel
	{
        private readonly IGlobalStateReadOnly<AppointmentModifications> appointmentModificationsVariable;

        private bool changeConfirmationVisible;
		private bool addAppointmentPossible;
		private bool disabledOverlayVisible;

		public OverviewPageViewModel(IDateDisplayViewModel dateDisplayViewModel,
									 IMedicalPracticeSelectorViewModel medicalPracticeSelectorViewModel, 
									 IRoomFilterViewModel roomFilterViewModel, 
									 IDateSelectorViewModel dateSelectorViewModel, 
									 IGridContainerViewModel gridContainerViewModel, 
									 IChangeConfirmationViewModel changeConfirmationViewModel, 									
									 IUndoRedoViewModel undoRedoViewModel,									 
                                     IWindowBuilder<Views.AddAppointmentDialog> dialogBuilder,
                                     IGlobalStateReadOnly<AppointmentModifications> appointmentModificationsVariable)
		{
		    this.appointmentModificationsVariable = appointmentModificationsVariable;
		    DateDisplayViewModel = dateDisplayViewModel;
			MedicalPracticeSelectorViewModel = medicalPracticeSelectorViewModel;
			RoomFilterViewModel = roomFilterViewModel;
			DateSelectorViewModel = dateSelectorViewModel;
			GridContainerViewModel = gridContainerViewModel;
			ChangeConfirmationViewModel = changeConfirmationViewModel;
			UndoRedoViewModel = undoRedoViewModel;

			ChangeConfirmationVisible = false;
			AddAppointmentPossible = true;
			DisabledOverlayVisible = false;


            appointmentModificationsVariable.StateChanged += OnCurrentModifiedAppointmentVariableChanged;			

			ShowAddAppointmentDialog = new Command(() =>
			{
				DisabledOverlayVisible = true;

				var dialogWindow = dialogBuilder.BuildWindow();
				dialogWindow.ShowDialog();

				DisabledOverlayVisible = false;
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
		public IUndoRedoViewModel                UndoRedoViewModel                { get; }

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

		public bool DisabledOverlayVisible
		{
			get { return disabledOverlayVisible; }
			private set { PropertyChanged.ChangeAndNotify(this, ref disabledOverlayVisible, value); }
		}
		
        protected override void CleanUp()
        {
            appointmentModificationsVariable.StateChanged -= OnCurrentModifiedAppointmentVariableChanged;
        }
        public override event PropertyChangedEventHandler PropertyChanged;
    }
}
