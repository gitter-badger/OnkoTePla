using System.Windows.Input;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.ChangeConfirmationView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateDisplay;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.GridContainer;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MedicalPracticeSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelector;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OverviewPage
{
	public interface IOverviewPageViewModel : IViewModel
	{
		IDateDisplayViewModel             DateDisplayViewModel             { get; }
		IMedicalPracticeSelectorViewModel MedicalPracticeSelectorViewModel { get; }
		IRoomFilterViewModel              RoomFilterViewModel              { get; }
		IDateSelectorViewModel            DateSelectorViewModel            { get; }
		IGridContainerViewModel           GridContainerViewModel           { get; }	
		IChangeConfirmationViewModel      ChangeConfirmationViewModel      { get; }

		ICommand ShowAddAppointmentDialog { get; }

		bool ChangeConfirmationVisible { get; }
		bool AddAppointmentPossible { get; }
	}
}
