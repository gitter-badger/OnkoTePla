using System.Collections.ObjectModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.GridContainer
{
	public class GridContainerViewModelSampleData : IGridContainerViewModel 
	{
		public ObservableCollection<AppointmentGridViewModel> LoadedAppointmentGrids { get; }
		public AppointmentGridViewModel CurrentDisplayedAppointmentGridViewModel { get; }
	}
}
