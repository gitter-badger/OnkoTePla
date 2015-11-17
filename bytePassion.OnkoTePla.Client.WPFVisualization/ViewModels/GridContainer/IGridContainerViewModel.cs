using bytePassion.Lib.Communication.ViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid;
using System.Collections.ObjectModel;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.GridContainer
{
	public interface IGridContainerViewModel : IViewModel,
											   IViewModelCommunicationDeliverer
	{
		ObservableCollection<IAppointmentGridViewModel> LoadedAppointmentGrids { get; }
		
		int CurrentDisplayedAppointmentGridIndex { get; }
	}
}
