using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.GridContainer;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MedicalPracticeSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelector;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OverviewPage
{
	public class OverviewPageViewModelSampleData : IOverviewPageViewModel
	{
		public OverviewPageViewModelSampleData()
		{
			MedicalPracticeSelectorViewModel = new MedicalPracticeSelectorViewModelSampleData();
			RoomFilterViewModel            = new RoomFilterViewModelSampleData();
			DateSelectorViewModel            = new DateSelectorViewModelSampleData();
			GridContainerViewModel           = new GridContainerViewModelSampleData();
		}

		public IMedicalPracticeSelectorViewModel MedicalPracticeSelectorViewModel { get; }
		public IRoomFilterViewModel            RoomFilterViewModel            { get; }
		public IDateSelectorViewModel            DateSelectorViewModel            { get; }
		public IGridContainerViewModel           GridContainerViewModel           { get; }
	}
}
