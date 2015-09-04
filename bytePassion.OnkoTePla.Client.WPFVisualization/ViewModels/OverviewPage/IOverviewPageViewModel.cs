using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.GridContainer;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MedicalPracticeSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelector;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OverviewPage
{
	public interface IOverviewPageViewModel
	{
		IMedicalPracticeSelectorViewModel MedicalPracticeSelectorViewModel { get; }
		IRoomFilterViewModel            RoomFilterViewModel            { get; }
		IDateSelectorViewModel            DateSelectorViewModel            { get; }
		IGridContainerViewModel           GridContainerViewModel           { get; }	
	}
}
