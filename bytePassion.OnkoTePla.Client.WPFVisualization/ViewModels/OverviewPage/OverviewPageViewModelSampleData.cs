using System.ComponentModel;
using System.Windows.Input;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.ChangeConfirmationView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateDisplay;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.GridContainer;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MedicalPracticeSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.UndoRedoView;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OverviewPage
{
	public class OverviewPageViewModelSampleData : IOverviewPageViewModel
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

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
