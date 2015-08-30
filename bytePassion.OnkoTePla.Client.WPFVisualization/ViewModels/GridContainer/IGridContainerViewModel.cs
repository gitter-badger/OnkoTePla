using System.Collections.ObjectModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.GridContainer
{
	public interface IGridContainerViewModel
	{
		ObservableCollection<AppointmentGridViewModel> LoadedAppointmentGrids { get; }

		AppointmentGridViewModel CurrentDisplayedAppointmentGridViewModel { get; }
	}
}
