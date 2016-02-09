using System;
using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.Utils;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ChangeConfirmationView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.DateDisplay;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.DateSelector;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.GridContainer;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MedicalPracticeSelector;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.RoomSelector;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.UndoRedoView;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.OverviewPage
{
	internal class OverviewPageViewModel : ViewModel, 
                                           IOverviewPageViewModel
	{
        private readonly ISharedStateReadOnly<AppointmentModifications> appointmentModificationsVariable;
	    private readonly Action<string> errorCallback;

	    private bool changeConfirmationVisible;
		private bool addAppointmentPossible;		

		public OverviewPageViewModel(IViewModelCommunication viewModelCommunication,
                                     IDateDisplayViewModel dateDisplayViewModel,
									 IMedicalPracticeSelectorViewModel medicalPracticeSelectorViewModel, 
									 IRoomFilterViewModel roomFilterViewModel, 
									 IDateSelectorViewModel dateSelectorViewModel, 
									 IGridContainerViewModel gridContainerViewModel, 
									 IChangeConfirmationViewModel changeConfirmationViewModel, 									
									 IUndoRedoViewModel undoRedoViewModel,									 
                                     IWindowBuilder<Views.AddAppointmentDialog> dialogBuilder,
                                     ISharedStateReadOnly<AppointmentModifications> appointmentModificationsVariable,
									 Action<string> errorCallback)
		{
		    this.appointmentModificationsVariable = appointmentModificationsVariable;
			this.errorCallback = errorCallback;
			DateDisplayViewModel = dateDisplayViewModel;
			MedicalPracticeSelectorViewModel = medicalPracticeSelectorViewModel;
			RoomFilterViewModel = roomFilterViewModel;
			DateSelectorViewModel = dateSelectorViewModel;
			GridContainerViewModel = gridContainerViewModel;
			ChangeConfirmationViewModel = changeConfirmationViewModel;
			UndoRedoViewModel = undoRedoViewModel;

			ChangeConfirmationVisible = false;
			AddAppointmentPossible = true;			

			
            appointmentModificationsVariable.StateChanged += OnCurrentModifiedAppointmentVariableChanged;			

			ShowAddAppointmentDialog = new Command(() =>
			{
				viewModelCommunication.Send(new ShowDisabledOverlay());

				var dialogWindow = dialogBuilder.BuildWindow(this.errorCallback);
				dialogWindow.ShowDialog();

                viewModelCommunication.Send(new HideDisabledOverlay());                
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
		
        protected override void CleanUp()
        {
            appointmentModificationsVariable.StateChanged -= OnCurrentModifiedAppointmentVariableChanged;
        }
        public override event PropertyChangedEventHandler PropertyChanged;
    }
}
