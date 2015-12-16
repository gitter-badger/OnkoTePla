using System.Collections.ObjectModel;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Types.SemanticTypes;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.GridContainer
{
	public interface IGridContainerViewModel : IViewModel,
											   IViewModelCommunicationDeliverer
	{
		ObservableCollection<IAppointmentGridViewModel> LoadedAppointmentGrids { get; }
		
		int CurrentDisplayedAppointmentGridIndex { get; }

		Size ReportedGridSize { set; }
	}
}
