using System.Windows.Input;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.DateDisplay;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.DateSelector;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.GridContainer;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MedicalPracticeSelector;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.RoomSelector;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.UndoRedoView;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.OverviewPage
{
	internal interface IOverviewPageViewModel : IViewModel
	{
		IDateDisplayViewModel             DateDisplayViewModel             { get; }
		IMedicalPracticeSelectorViewModel MedicalPracticeSelectorViewModel { get; }
		IRoomFilterViewModel              RoomFilterViewModel              { get; }
		IDateSelectorViewModel            DateSelectorViewModel            { get; }
		IGridContainerViewModel           GridContainerViewModel           { get; }			
		IUndoRedoViewModel                UndoRedoViewModel                { get; }

		ICommand ShowAddAppointmentDialog { get; }
		ICommand ShowPrintDialog          { get; }

		bool ChangeConfirmationVisible { get; }
		bool AddAppointmentPossible    { get; }		
	}
}
