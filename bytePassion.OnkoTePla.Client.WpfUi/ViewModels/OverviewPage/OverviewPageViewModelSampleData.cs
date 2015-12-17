using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.ChangeConfirmationView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.DateDisplay;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.DateSelector;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.GridContainer;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MedicalPracticeSelector;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.RoomSelector;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.UndoRedoView;
using System.ComponentModel;
using System.Windows.Input;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.OverviewPage
{
    internal class OverviewPageViewModelSampleData : IOverviewPageViewModel
	{
		public OverviewPageViewModelSampleData()
		{
			DateDisplayViewModel             = new DateDisplayViewModelSampleData();
			MedicalPracticeSelectorViewModel = new MedicalPracticeSelectorViewModelSampleData();
			RoomFilterViewModel              = new RoomFilterViewModelSampleData();
			DateSelectorViewModel            = new DateSelectorViewModelSampleData();
			GridContainerViewModel           = new GridContainerViewModelSampleData();
			ChangeConfirmationViewModel      = new ChangeConfirmationViewModelSampleData();
			UndoRedoViewModel                = new UndoRedoViewModelSampleData();

			ChangeConfirmationVisible = true;
			AddAppointmentPossible = true;
			DisabledOverlayVisible = false;
		}

		public IDateDisplayViewModel             DateDisplayViewModel             { get; }
		public IMedicalPracticeSelectorViewModel MedicalPracticeSelectorViewModel { get; }
		public IRoomFilterViewModel              RoomFilterViewModel              { get; }
		public IDateSelectorViewModel            DateSelectorViewModel            { get; }
		public IGridContainerViewModel           GridContainerViewModel           { get; }
		public IChangeConfirmationViewModel      ChangeConfirmationViewModel      { get; }
		public IUndoRedoViewModel                UndoRedoViewModel                { get; }

		public ICommand ShowAddAppointmentDialog { get; } = null;

		public bool ChangeConfirmationVisible { get; }
		public bool AddAppointmentPossible { get; }
		public bool DisabledOverlayVisible { get; }

        public void Dispose() { }
        public event PropertyChangedEventHandler PropertyChanged;	    
	}
}
